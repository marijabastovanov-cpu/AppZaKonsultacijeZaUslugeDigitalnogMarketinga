using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.ViewModels;

public class UslugaVM
{
    [Required(ErrorMessage = "Unesite šifru.")]
    [StringLength(3, ErrorMessage = "Šifra može imati najviše 3 znaka.")]
    [Display(Name = "Šifra")]
    public string Sifra { get; set; } = "";

    public string? StaraSifra { get; set; }

    [Required(ErrorMessage = "Unesite naziv.")]
    [Display(Name = "Naziv usluge")]
    public string Naziv { get; set; } = "";

    [Display(Name = "Opis")]
    public string? Opis { get; set; }
}
