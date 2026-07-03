using System.Text.Json;

namespace SlojPodataka.Servisi;

// KLIJENTSKI SERVIS - poziva REST API preko HttpClient-a i vraća rezultat (bool).
public class ServisProvereTermina
{
    private readonly HttpClient _http;

   
    private const string BaznaAdresa = "http://localhost:5034";

    public ServisProvereTermina(HttpClient http)
    {
        _http = http;
    }

    public async Task<bool> DaLiImaSlobodnihTermina(DateTime datum)
    {
        string url = $"{BaznaAdresa}/api/ProveraApi?datum={datum:yyyy-MM-dd}";
        HttpResponseMessage response = await _http.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception($"Greška kod provere: {response.StatusCode}");

        string json = await response.Content.ReadAsStringAsync();
        var rezultat = JsonSerializer.Deserialize<Dictionary<string, bool>>(
            json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return rezultat != null && rezultat.ContainsKey("ima") && rezultat["ima"];
    }
}
