using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlojPodataka.EntityModels;
using SlojPodataka.Repozitorijum;
using PrezentacioniSloj.ViewModels;

namespace PrezentacioniSloj.Controllers;

[Authorize]   // samo prijavljeni (admin / sekretar)
public class KonsultacijeController : Controller
{
    private readonly IKonsultacijaRepo _konsultacijaRepo;
    private readonly ITerminRepo _terminRepo;

    public KonsultacijeController(IKonsultacijaRepo konsultacijaRepo, ITerminRepo terminRepo)
    {
        _konsultacijaRepo = konsultacijaRepo;
        _terminRepo = terminRepo;
    }

    public IActionResult Index(DateTime? datum)
    {
        var vm = new KonsultacijeIndexVM { Datum = datum };
        var lista = datum.HasValue ? _konsultacijaRepo.DajPoDatumu(datum.Value) : _konsultacijaRepo.DajSve();
        vm.Konsultacije = lista.Select(MapirajPregled).ToList();
        return View(vm);
    }

    public IActionResult Detalji(int id)
    {
        var vm = UcitajDetalje(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpGet]
    public IActionResult Izmeni(int id)
    {
        var k = _konsultacijaRepo.DajPoId(id);
        if (k == null) return NotFound();

        var vm = new KonsultacijaIzmenaVM
        {
            Id = k.Id,
            ImePrezimeKlijenta = k.ImePrezimeKlijenta,
            NazivFirme = k.NazivFirme,
            Email = k.Email,
            Telefon = k.Telefon,
            DatumKonsultacije = k.DatumKonsultacije,
            IDTermin = k.IDTermin,
            Napomena = k.Napomena,
            Termini = UcitajTermine()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Izmeni(KonsultacijaIzmenaVM vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Termini = UcitajTermine();
            return View(vm);
        }

        var k = new KonsultacijaEntityModel
        {
            Id = vm.Id,
            ImePrezimeKlijenta = vm.ImePrezimeKlijenta,
            NazivFirme = vm.NazivFirme,
            Email = vm.Email,
            Telefon = vm.Telefon,
            DatumKonsultacije = vm.DatumKonsultacije.Date,
            IDTermin = vm.IDTermin,
            Napomena = vm.Napomena
        };

        try
        {
            _konsultacijaRepo.Izmeni(k);
        }
        catch
        {
            vm.Greska = "Izmena nije moguća: izabrani termin je za taj datum već zauzet.";
            vm.Termini = UcitajTermine();
            return View(vm);
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Obrisi(int id)
    {
        _konsultacijaRepo.Obrisi(id);
        return RedirectToAction("Index");
    }

    // Štampa spiska
    public IActionResult Stampa(DateTime? datum)
    {
        var vm = new KonsultacijeIndexVM { Datum = datum };
        var lista = datum.HasValue ? _konsultacijaRepo.DajPoDatumu(datum.Value) : _konsultacijaRepo.DajSve();
        vm.Konsultacije = lista.Select(MapirajPregled).ToList();
        return View(vm);
    }

    // Parametarska štampa - pojedinačni dokument
    public IActionResult StampaKonsultacije(int id)
    {
        var vm = UcitajDetalje(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    //  pomoćno 
    private static KonsultacijaPregledVM MapirajPregled(KonsultacijaEntityModel k) => new KonsultacijaPregledVM
    {
        Id = k.Id,
        ImePrezimeKlijenta = k.ImePrezimeKlijenta,
        NazivFirme = k.NazivFirme,
        Email = k.Email,
        Telefon = k.Telefon,
        DatumKonsultacije = k.DatumKonsultacije,
        NazivTermina = k.Termin != null ? k.Termin.Naziv : k.IDTermin
    };

    private KonsultacijaDetaljiVM? UcitajDetalje(int id)
    {
        var k = _konsultacijaRepo.DajPoId(id);
        if (k == null) return null;

        return new KonsultacijaDetaljiVM
        {
            Id = k.Id,
            ImePrezimeKlijenta = k.ImePrezimeKlijenta,
            NazivFirme = k.NazivFirme,
            Email = k.Email,
            Telefon = k.Telefon,
            DatumKonsultacije = k.DatumKonsultacije,
            NazivTermina = k.Termin != null ? k.Termin.Naziv : k.IDTermin,
            Napomena = k.Napomena,
            DatumPodnosenja = k.DatumPodnosenja,
            Stavke = k.Stavke.Select(s => new StavkaPrikazVM
            {
                NazivUsluge = s.Usluga != null ? s.Usluga.Naziv : s.IDUsluga,
                Prioritet = s.Prioritet,
                Napomena = s.Napomena
            }).ToList()
        };
    }

    private List<SelectListItem> UcitajTermine()
    {
        return _terminRepo.DajSve()
            .Select(t => new SelectListItem { Value = t.Sifra, Text = t.Naziv })
            .ToList();
    }
}
