using Microsoft.EntityFrameworkCore;
using SlojPodataka.EntityModels;

namespace SlojPodataka.Data;

// Centralna tačka komunikacije sa bazom (EF Core)
public class AppDbContext : DbContext
{
    public DbSet<KorisnikEntityModel> Korisnici { get; set; }
    public DbSet<TerminEntityModel> Termini { get; set; }
    public DbSet<UslugaEntityModel> Usluge { get; set; }
    public DbSet<KonsultacijaEntityModel> Konsultacije { get; set; }
    public DbSet<StavkaKonsultacijeEntityModel> Stavke { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> opcije) : base(opcije)
    {
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Kada se obriše konsultacija, brišu se i njene stavke (kao ON DELETE CASCADE u bazi)
        mb.Entity<KonsultacijaEntityModel>()
          .HasMany(k => k.Stavke)
          .WithOne(s => s.Konsultacija!)
          .HasForeignKey(s => s.IDKonsultacija)
          .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(mb);
    }
}
