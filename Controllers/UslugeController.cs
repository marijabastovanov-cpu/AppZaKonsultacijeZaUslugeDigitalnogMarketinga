using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlojPodataka.EntityModels;
using SlojPodataka.Repozitorijum;
using PrezentacioniSloj.ViewModels;

namespace PrezentacioniSloj.Controllers;

[Authorize]
public class UslugeController : Controller
{
    private readonly IUslugaRepo _uslugaRepo;
    public UslugeController(IUslugaRepo uslugaRepo) { _uslugaRepo = uslugaRepo; }

    public IActionResult Index()
    {
        return View(_uslugaRepo.DajSve());
    }

    [HttpGet]
    public IActionResult Dodaj() => View(new UslugaVM());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Dodaj(UslugaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        try
        {
            _uslugaRepo.Dodaj(new UslugaEntityModel { Sifra = vm.Sifra, Naziv = vm.Naziv, Opis = vm.Opis });
        }
        catch
        {
            ModelState.AddModelError("", "Dodavanje nije uspelo. Šifra možda već postoji.");
            return View(vm);
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Izmeni(string id)
    {
        var u = _uslugaRepo.DajPoSifri(id);
        if (u == null) return NotFound();
        return View(new UslugaVM { Sifra = u.Sifra, StaraSifra = u.Sifra, Naziv = u.Naziv, Opis = u.Opis });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Izmeni(UslugaVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        // šifra je primarni ključ - menjamo naziv i opis postojeće usluge
        _uslugaRepo.Izmeni(new UslugaEntityModel { Sifra = vm.StaraSifra ?? vm.Sifra, Naziv = vm.Naziv, Opis = vm.Opis });
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Obrisi(string id)
    {
        try { _uslugaRepo.Obrisi(id); }
        catch { TempData["Greska"] = "Brisanje nije moguće: usluga se koristi u nekoj konsultaciji."; }
        return RedirectToAction("Index");
    }
}
