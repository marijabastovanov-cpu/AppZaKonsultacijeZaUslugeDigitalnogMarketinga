using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.EntityModels;

// DETAIL: stavka konsultacije (izabrana usluga)
[Table("StavkaKonsultacije")]
public class StavkaKonsultacijeEntityModel
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int IDKonsultacija { get; set; }

    [Required]
    [StringLength(3)]
    public string IDUsluga { get; set; } = "";

    public int Prioritet { get; set; } = 1;

    [StringLength(150)]
    public string? Napomena { get; set; }

    // navigaciona svojstva
    [ForeignKey(nameof(IDKonsultacija))]
    public KonsultacijaEntityModel? Konsultacija { get; set; }

    [ForeignKey(nameof(IDUsluga))]
    public UslugaEntityModel? Usluga { get; set; }
}
