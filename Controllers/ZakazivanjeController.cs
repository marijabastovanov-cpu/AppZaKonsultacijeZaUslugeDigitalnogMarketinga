using Microsoft.AspNetCore.Mvc;
using SlojPodataka.EntityModels;
using SlojPodataka.Repozitorijum;
using SlojPodataka.Servisi;
using PrezentacioniSloj.ViewModels;

namespace PrezentacioniSloj.Controllers;

// JAVNA strana - klijent zakazuje konsultaciju (bez prijave)
public class ZakazivanjeController : Controller
{
    private readonly IKonsultacijaRepo _konsultacijaRepo;
    private readonly IUslugaRepo _uslugaRepo;
    private readonly ServisProvereTermina _servis;

    public ZakazivanjeController(IKonsultacijaRepo konsultacijaRepo, IUslugaRepo uslugaRepo, ServisProvereTermina servis)
    {
        _konsultacijaRepo = konsultacijaRepo;
        _uslugaRepo = uslugaRepo;
        _servis = servis;
    }

    [HttpGet]
    public IActionResult Index() => View(new ZakazivanjeVM());

    // Korak 2: provera pravila preko REST servisa, pa prikaz termina i usluga
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProveriTermine(ZakazivanjeVM vm)
    {
        if (!ModelState.IsValid)
            return View("Index", vm);

        bool ima = await _servis.DaLiImaSlobodnihTermina(vm.DatumKonsultacije);
        if (!ima)
        {
            vm.PrikaziTermine = false;
            vm.Poruka = "Nema slobodnih termina za izabrani datum. Izaberite drugi dan.";
            return View("Index", vm);
        }

        vm.SlobodniTermini = UcitajSlobodne(vm.DatumKonsultacije);
        vm.Usluge = UcitajUsluge();
        vm.PrikaziTermine = true;
        return View("Index", vm);
    }

    // Korak 3: zakazivanje (master-detail + transakcija u repozitorijumu)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Zakazi(ZakazivanjeVM vm)
    {
        if (string.IsNullOrEmpty(vm.IzabraniTermin))
        {
            vm.PrikaziTermine = true;
            vm.Poruka = "Izaberite jedan od slobodnih termina.";
            vm.SlobodniTermini = UcitajSlobodne(vm.DatumKonsultacije);
            if (vm.Usluge.Count == 0) vm.Usluge = UcitajUsluge();
            return View("Index", vm);
        }

        // ponovna provera pravila preko servisa
        bool ima = await _servis.DaLiImaSlobodnihTermina(vm.DatumKonsultacije);
        if (!ima)
        {
            vm.PrikaziTermine = false;
            vm.Poruka = "Nema slobodnih termina za izabrani datum.";
            return View("Index", vm);
        }

        var konsultacija = new KonsultacijaEntityModel
        {
            ImePrezimeKlijenta = vm.ImePrezimeKlijenta,
            NazivFirme = vm.NazivFirme,
            Email = vm.Email,
            Telefon = vm.Telefon,
            DatumKonsultacije = vm.DatumKonsultacije.Date,
            IDTermin = vm.IzabraniTermin,
            Napomena = vm.Napomena
        };

        foreach (var u in vm.Usluge)
        {
            if (u.Izabrana)
            {
                konsultacija.Stavke.Add(new StavkaKonsultacijeEntityModel
                {
                    IDUsluga = u.Sifra,
                    Prioritet = u.Prioritet,
                    Napomena = u.Napomena
                });
            }
        }

        bool uspeh = _konsultacijaRepo.DodajSaStavkama(konsultacija);
        if (uspeh)
            return View("Uspeh", vm);

        vm.PrikaziTermine = true;
        vm.Poruka = "Zakazivanje nije uspelo. Termin je možda u međuvremenu zauzet.";
        vm.SlobodniTermini = UcitajSlobodne(vm.DatumKonsultacije);
        return View("Index", vm);
    }

    private List<TerminVM> UcitajSlobodne(DateTime datum)
    {
        return _konsultacijaRepo.DajSlobodneTermine(datum)
            .Select(t => new TerminVM { Sifra = t.Sifra, Naziv = t.Naziv })
            .ToList();
    }

    private List<UslugaIzborVM> UcitajUsluge()
    {
        return _uslugaRepo.DajSve()
            .Select(u => new UslugaIzborVM { Sifra = u.Sifra, Naziv = u.Naziv })
            .ToList();
    }
}
