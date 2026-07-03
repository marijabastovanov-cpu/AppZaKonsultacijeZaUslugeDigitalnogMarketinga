using SlojPodataka.Data;
using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public class UslugaRepo : IUslugaRepo
{
    private readonly AppDbContext _context;
    public UslugaRepo(AppDbContext context) { _context = context; }

    public List<UslugaEntityModel> DajSve()
    {
        return _context.Usluge.OrderBy(u => u.Sifra).ToList();
    }

    public UslugaEntityModel? DajPoSifri(string sifra)
    {
        return _context.Usluge.Find(sifra);
    }

    public void Dodaj(UslugaEntityModel usluga)
    {
        _context.Usluge.Add(usluga);
        _context.SaveChanges();
    }

    public void Izmeni(UslugaEntityModel usluga)
    {
        _context.Usluge.Update(usluga);
        _context.SaveChanges();
    }

    public void Obrisi(string sifra)
    {
        var usluga = _context.Usluge.Find(sifra);
        if (usluga == null) return;
        _context.Usluge.Remove(usluga);
        _context.SaveChanges();
    }
}
