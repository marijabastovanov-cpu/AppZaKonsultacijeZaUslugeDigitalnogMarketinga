using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SlojPodataka.Data;
using SlojPodataka.Repozitorijum;
using SlojPodataka.Servisi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// EF Core
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DigitalniMarketing")));

// Repozitorijumi (Dependency Injection - interfejs -> implementacija)
builder.Services.AddScoped<IKorisnikRepo, KorisnikRepo>();
builder.Services.AddScoped<ITerminRepo, TerminRepo>();
builder.Services.AddScoped<IUslugaRepo, UslugaRepo>();
builder.Services.AddScoped<IKonsultacijaRepo, KonsultacijaRepo>();

// HttpClient klijent za REST servis
builder.Services.AddHttpClient<ServisProvereTermina>();

// Prijava (cookie autentikacija + uloge)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Nalog/Login";
        o.AccessDeniedPath = "/Nalog/Login";
    });

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
