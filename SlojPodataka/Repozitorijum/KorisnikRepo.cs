using SlojPodataka.Data;
using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public class KorisnikRepo : IKorisnikRepo
{
    private readonly AppDbContext _context;
    public KorisnikRepo(AppDbContext context) { _context = context; }

    public KorisnikEntityModel? Prijava(string korisnickoIme, string sifra)
    {
        return _context.Korisnici
            .FirstOrDefault(k => k.KorisnickoIme == korisnickoIme && k.Sifra == sifra);
    }
}
