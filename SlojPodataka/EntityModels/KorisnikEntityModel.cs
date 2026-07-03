using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.EntityModels;

// Nezavisna tabela korisnik (login: admin ili sekretar)
[Table("Korisnik")]
public class KorisnikEntityModel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(60)]
    public string ImePrezime { get; set; } = "";

    [Required]
    [StringLength(20)]
    public string KorisnickoIme { get; set; } = "";

    [Required]
    [StringLength(30)]
    public string Sifra { get; set; } = "";

    [Required]
    [StringLength(15)]
    public string Uloga { get; set; } = "";   // admin / sekretar
}
