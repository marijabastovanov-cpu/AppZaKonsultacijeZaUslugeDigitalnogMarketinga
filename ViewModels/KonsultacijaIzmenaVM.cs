using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PrezentacioniSloj.ViewModels;

public class KonsultacijaIzmenaVM
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Unesite ime i prezime.")]
    [Display(Name = "Ime i prezime")]
    public string ImePrezimeKlijenta { get; set; } = "";

    [Display(Name = "Naziv firme")]
    public string? NazivFirme { get; set; }

    [Required(ErrorMessage = "Unesite email.")]
    [EmailAddress(ErrorMessage = "Email nije ispravan.")]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "Unesite telefon.")]
    public string Telefon { get; set; } = "";

    [Required]
    [DataType(DataType.Date)]
    [Display(Name = "Datum konsultacije")]
    public DateTime DatumKonsultacije { get; set; }

    [Required(ErrorMessage = "Izaberite termin.")]
    [Display(Name = "Termin")]
    public string IDTermin { get; set; } = "";

    [Display(Name = "Napomena")]
    public string? Napomena { get; set; }

    public string? Greska { get; set; }
    public List<SelectListItem> Termini { get; set; } = new();
}
