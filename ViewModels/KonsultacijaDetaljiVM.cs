namespace PrezentacioniSloj.ViewModels;

public class KonsultacijaDetaljiVM
{
    public int Id { get; set; }
    public string ImePrezimeKlijenta { get; set; } = "";
    public string? NazivFirme { get; set; }
    public string Email { get; set; } = "";
    public string Telefon { get; set; } = "";
    public DateTime DatumKonsultacije { get; set; }
    public string NazivTermina { get; set; } = "";
    public string? Napomena { get; set; }
    public DateTime DatumPodnosenja { get; set; }

    public List<StavkaPrikazVM> Stavke { get; set; } = new();
}
