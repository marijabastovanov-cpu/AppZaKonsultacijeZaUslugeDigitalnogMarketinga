namespace PrezentacioniSloj.ViewModels;

public class KonsultacijaPregledVM
{
    public int Id { get; set; }
    public string ImePrezimeKlijenta { get; set; } = "";
    public string? NazivFirme { get; set; }
    public string Email { get; set; } = "";
    public string Telefon { get; set; } = "";
    public DateTime DatumKonsultacije { get; set; }
    public string NazivTermina { get; set; } = "";
}
