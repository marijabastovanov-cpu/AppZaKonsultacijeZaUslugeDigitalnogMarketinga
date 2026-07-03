using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SlojPodataka.Repozitorijum;
using PrezentacioniSloj.ViewModels;

namespace PrezentacioniSloj.Controllers;

public class NalogController : Controller
{
    private readonly IKorisnikRepo _korisnikRepo;
    public NalogController(IKorisnikRepo korisnikRepo) { _korisnikRepo = korisnikRepo; }

    [HttpGet]
    public IActionResult Login() => View(new LoginVM());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var korisnik = _korisnikRepo.Prijava(vm.KorisnickoIme, vm.Sifra);
        if (korisnik == null)
        {
            vm.Greska = "Pogrešno korisničko ime ili šifra.";
            return View(vm);
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, korisnik.KorisnickoIme),
            new Claim(ClaimTypes.Role, korisnik.Uloga),
            new Claim("ImePrezime", korisnik.ImePrezime)
        };
        var identitet = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identitet));

        return RedirectToAction("Index", "Konsultacije");
    }

    public async Task<IActionResult> Odjava()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
