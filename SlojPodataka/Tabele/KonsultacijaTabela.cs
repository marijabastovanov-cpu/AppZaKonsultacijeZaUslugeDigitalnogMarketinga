using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.Tabele;

// Konkretna tehnološka klasa za glavnu tabelu Konsultacija (nasleđuje DBUtils).
public class KonsultacijaTabela : DBUtils
{
    // sve konsultacije
    public DataTable DajSve()
    {
        return IzvrsiProceduru("DajSveKonsultacije");
    }

    // konsultacije za dati datum (filter)
    public DataTable DajPoDatumu(DateTime datum)
    {
        return IzvrsiProceduru("DajKonsultacijePoDatumu",
            new SqlParameter("@Datum", datum.Date));
    }

    // pojedinačna konsultacija po ID-u
    public DataTable DajPoID(int id)
    {
        return IzvrsiProceduru("DajKonsultacijuPoID",
            new SqlParameter("@ID", id));
    }

    // broj zauzetih termina za datum (poslovno pravilo)
    public int DajBrojZauzetih(DateTime datum)
    {
        object? rezultat = IzvrsiSkalar("DajBrojZauzetihTerminaZaDatum",
            new SqlParameter("@Datum", datum.Date));
        return rezultat == null ? 0 : Convert.ToInt32(rezultat);
    }

    // dodavanje konsultacije (vraća novi ID)
    public int Dodaj(string imePrezimeKlijenta, string? nazivFirme, string email,
                     string telefon, DateTime datumKonsultacije, string idTermin, string? napomena)
    {
        object? noviId = IzvrsiSkalar("DodajKonsultaciju",
            new SqlParameter("@ImePrezimeKlijenta", imePrezimeKlijenta),
            new SqlParameter("@NazivFirme", (object?)nazivFirme ?? DBNull.Value),
            new SqlParameter("@Email", email),
            new SqlParameter("@Telefon", telefon),
            new SqlParameter("@DatumKonsultacije", datumKonsultacije.Date),
            new SqlParameter("@IDTermin", idTermin),
            new SqlParameter("@Napomena", (object?)napomena ?? DBNull.Value));
        return Convert.ToInt32(noviId);
    }

    // izmena konsultacije
    public int Izmeni(int id, string imePrezimeKlijenta, string? nazivFirme, string email,
                      string telefon, DateTime datumKonsultacije, string idTermin, string? napomena)
    {
        return IzvrsiKomandu("IzmeniKonsultaciju",
            new SqlParameter("@ID", id),
            new SqlParameter("@ImePrezimeKlijenta", imePrezimeKlijenta),
            new SqlParameter("@NazivFirme", (object?)nazivFirme ?? DBNull.Value),
            new SqlParameter("@Email", email),
            new SqlParameter("@Telefon", telefon),
            new SqlParameter("@DatumKonsultacije", datumKonsultacije.Date),
            new SqlParameter("@IDTermin", idTermin),
            new SqlParameter("@Napomena", (object?)napomena ?? DBNull.Value));
    }

    // brisanje konsultacije (stavke se brišu kaskadno)
    public int Obrisi(int id)
    {
        return IzvrsiKomandu("ObrisiKonsultaciju",
            new SqlParameter("@ID", id));
    }
}
