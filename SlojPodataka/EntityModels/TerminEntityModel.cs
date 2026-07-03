using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka.EntityModels;

// Šifarnik termina (6 fiksnih dnevnih termina)
[Table("Termin")]
public class TerminEntityModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]  // šifra se ne generiše automatski
    [StringLength(2)]
    public string Sifra { get; set; } = "";

    public int RedniBroj { get; set; }

    [Required]
    [StringLength(5)]
    public string VremeOd { get; set; } = "";

    [Required]
    [StringLength(5)]
    public string VremeDo { get; set; } = "";

    [Required]
    [StringLength(20)]
    public string Naziv { get; set; } = "";
}
