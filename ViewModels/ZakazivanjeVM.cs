using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.ViewModels;

// Zakazivanje konsultacije (javno). Korak 1: podaci + datum.
// Korak 2: izbor termina + izbor usluga (master-detail).
public class ZakazivanjeVM
{
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

    [Required(ErrorMessage = "Izaberite datum.")]
    [DataType(DataType.Date)]
    [Display(Name = "Datum konsultacije")]
    public DateTime DatumKonsultacije { get; set; } = DateTime.Today;

    [Display(Name = "Napomena")]
    public string? Napomena { get; set; }

    [Display(Name = "Izabrani termin")]
    public string? IzabraniTermin { get; set; }

    public List<TerminVM> SlobodniTermini { get; set; } = new();
    public List<UslugaIzborVM> Usluge { get; set; } = new();

    public bool PrikaziTermine { get; set; }
    public string? Poruka { get; set; }
}
