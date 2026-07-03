using System.ComponentModel.DataAnnotations;

namespace PrezentacioniSloj.ViewModels;

public class KonsultacijeIndexVM
{
    [DataType(DataType.Date)]
    [Display(Name = "Filter po datumu")]
    public DateTime? Datum { get; set; }

    public List<KonsultacijaPregledVM> Konsultacije { get; set; } = new();
}
