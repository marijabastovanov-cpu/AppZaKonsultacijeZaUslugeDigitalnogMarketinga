using System.Text.Json;
using SlojPodataka.Repozitorijum;

namespace PoslovnaLogika.Klase;

// SLOJ POSLOVNE LOGIKE - poslovno pravilo (ograničenje broja termina)
//
// POSLOVNO PRAVILO (OGRANIČENJE):
// AKO je broj već zauzetih termina za izabrani datum >= maksimuma (parametar
// iz JSON konfiguracije), ONDA nema slobodnih termina i zakazivanje nije dozvoljeno.
//
// Pravilo kombinuje:
//   (1) STANJE iz baze  - preko repozitorijuma (poziv REPO)
//   (2) PARAMETAR        - iz JSON datoteke (parametrizacija)
public class Ogranicenja
{
    private readonly IKonsultacijaRepo _konsultacijaRepo;
    private readonly string _putanjaJson;

    public Ogranicenja(IKonsultacijaRepo konsultacijaRepo, string putanjaJson)
    {
        _konsultacijaRepo = konsultacijaRepo;
        _putanjaJson = putanjaJson;
    }

    // GLAVNA METODA PRAVILA
    public bool DaLiImaSlobodnihTermina(DateTime datum)
    {
        int maksimum = UzmiMaxTerminaIzJSON();
        int trenutno = _konsultacijaRepo.DajBrojZauzetih(datum);
        return trenutno < maksimum;
    }

    // PARAMETRIZACIJA - čita maksimalan broj termina po danu iz JSON-a
    private int UzmiMaxTerminaIzJSON()
    {
        if (!File.Exists(_putanjaJson))
            throw new Exception("JSON fajl sa ograničenjem nije pronađen.");

        string json = File.ReadAllText(_putanjaJson);
        var podaci = JsonSerializer.Deserialize<Dictionary<string, int>>(json);

        if (podaci == null || !podaci.ContainsKey("MaxTerminaPoDanu"))
            throw new Exception("MaxTerminaPoDanu nije definisano u JSON-u.");

        return podaci["MaxTerminaPoDanu"];
    }
}
