using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.ViewModels;

public class LoginVM
{
    [Required(ErrorMessage = "Unesite korisničko ime.")]
    [Display(Name = "Korisničko ime")]
    public string KorisnickoIme { get; set; } = "";

    [Required(ErrorMessage = "Unesite šifru.")]
    [DataType(DataType.Password)]
    [Display(Name = "Šifra")]
    public string Sifra { get; set; } = "";

    public string? Greska { get; set; }
}
