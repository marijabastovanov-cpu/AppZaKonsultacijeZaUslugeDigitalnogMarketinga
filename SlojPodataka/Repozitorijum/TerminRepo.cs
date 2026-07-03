using SlojPodataka.Data;
using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public class TerminRepo : ITerminRepo
{
    private readonly AppDbContext _context;
    public TerminRepo(AppDbContext context) { _context = context; }

    public List<TerminEntityModel> DajSve()
    {
        return _context.Termini.OrderBy(t => t.RedniBroj).ToList();
    }
}
