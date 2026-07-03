using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.EntityModels;

// GLAVNA TABELA (MASTER): konsultacija
[Table("Konsultacija")]
public class KonsultacijaEntityModel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Required]
    [StringLength(60)]
    public string ImePrezimeKlijenta { get; set; } = "";

    [StringLength(60)]
    public string? NazivFirme { get; set; }

    [Required]
    [StringLength(60)]
    public string Email { get; set; } = "";

    [Required]
    [StringLength(20)]
    public string Telefon { get; set; } = "";

    [Column(TypeName = "date")]
    public DateTime DatumKonsultacije { get; set; }

    [Required]
    [StringLength(2)]
    public string IDTermin { get; set; } = "";

    [StringLength(200)]
    public string? Napomena { get; set; }

    public DateTime DatumPodnosenja { get; set; }

    // navigaciona svojstva (master-detail i veza ka šifarniku termina)
    [ForeignKey(nameof(IDTermin))]
    public TerminEntityModel? Termin { get; set; }

    public List<StavkaKonsultacijeEntityModel> Stavke { get; set; } = new();
}
