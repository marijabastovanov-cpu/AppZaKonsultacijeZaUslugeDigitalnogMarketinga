using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.ViewModels;

// Jedna usluga u listi za izbor (master-detail deo)
public class UslugaIzborVM
{
    public string Sifra { get; set; } = "";
    public string Naziv { get; set; } = "";

    [Display(Name = "Izaberi")]
    public bool Izabrana { get; set; }

    [Display(Name = "Prioritet")]
    public int Prioritet { get; set; } = 1;

    [Display(Name = "Napomena")]
    public string? Napomena { get; set; }
}
