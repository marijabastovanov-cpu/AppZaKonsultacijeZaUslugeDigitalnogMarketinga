using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.EntityModels;

// Šifarnik usluga digitalnog marketinga
[Table("UslugaMarketinga")]
public class UslugaEntityModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [StringLength(3)]
    public string Sifra { get; set; } = "";

    [Required]
    [StringLength(50)]
    public string Naziv { get; set; } = "";

    [StringLength(200)]
    public string? Opis { get; set; }
}
