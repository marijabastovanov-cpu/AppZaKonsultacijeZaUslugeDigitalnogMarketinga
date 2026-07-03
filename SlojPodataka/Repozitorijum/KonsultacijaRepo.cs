using Microsoft.EntityFrameworkCore;
using SlojPodataka.Data;
using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public class KonsultacijaRepo : IKonsultacijaRepo
{
    private readonly AppDbContext _context;
    public KonsultacijaRepo(AppDbContext context) { _context = context; }

    // Tabelarni prikaz (sa terminom)
    public List<KonsultacijaEntityModel> DajSve()
    {
        return _context.Konsultacije
            .Include(k => k.Termin)
            .OrderBy(k => k.DatumKonsultacije)
            .ToList();
    }

    // Filter po datumu
    public List<KonsultacijaEntityModel> DajPoDatumu(DateTime datum)
    {
        return _context.Konsultacije
            .Include(k => k.Termin)
            .Where(k => k.DatumKonsultacije == datum.Date)
            .ToList();
    }

    // Pojedinačni zapis sa svim stavkama (master + detail)
    public KonsultacijaEntityModel? DajPoId(int id)
    {
        return _context.Konsultacije
            .Include(k => k.Termin)
            .Include(k => k.Stavke)
                .ThenInclude(s => s.Usluga)
            .FirstOrDefault(k => k.Id == id);
    }

    // POSLOVNO PRAVILO: broj zauzetih termina za datum
    public int DajBrojZauzetih(DateTime datum)
    {
        return _context.Konsultacije.Count(k => k.DatumKonsultacije == datum.Date);
    }

    // Slobodni termini za dati datum
    public List<TerminEntityModel> DajSlobodneTermine(DateTime datum)
    {
        var zauzeti = _context.Konsultacije
            .Where(k => k.DatumKonsultacije == datum.Date)
            .Select(k => k.IDTermin)
            .ToList();

        return _context.Termini
            .Where(t => !zauzeti.Contains(t.Sifra))
            .OrderBy(t => t.RedniBroj)
            .ToList();
    }

    // MASTER-DETAIL UNOS U TRANSAKCIJI
    // Konsultacija (sa popunjenom listom Stavke) se upisuje kao celina.
    public bool DodajSaStavkama(KonsultacijaEntityModel konsultacija)
    {
        using var transakcija = _context.Database.BeginTransaction();
        try
        {
            konsultacija.DatumPodnosenja = DateTime.Now;
            // EF upisuje master i sve stavke iz kolekcije Stavke u istoj transakciji
            _context.Konsultacije.Add(konsultacija);
            _context.SaveChanges();

            transakcija.Commit();
            return true;
        }
        catch
        {
            transakcija.Rollback();
            return false;
        }
    }

    public void Izmeni(KonsultacijaEntityModel konsultacija)
    {
        _context.Konsultacije.Update(konsultacija);
        _context.SaveChanges();
    }

    public void Obrisi(int id)
    {
        var k = _context.Konsultacije.Find(id);
        if (k == null) return;
        _context.Konsultacije.Remove(k);   // stavke se brišu kaskadno
        _context.SaveChanges();
    }
}
