using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.Tabele;

// Konkretna tehnološka klasa za tabelu UslugaMarketinga (nasleđuje DBUtils).
public class UslugaTabela : DBUtils
{
    // sve usluge
    public DataTable DajSve()
    {
        return IzvrsiProceduru("DajSveUsluge");
    }

    // usluga po šifri
    public DataTable DajPoSifri(string sifra)
    {
        return IzvrsiProceduru("DajUsluguPoSifri",
            new SqlParameter("@Sifra", sifra));
    }

    // dodavanje usluge
    public int Dodaj(string sifra, string naziv, string? opis)
    {
        return IzvrsiKomandu("DodajUslugu",
            new SqlParameter("@Sifra", sifra),
            new SqlParameter("@Naziv", naziv),
            new SqlParameter("@Opis", (object?)opis ?? DBNull.Value));
    }

    // izmena usluge
    public int Izmeni(string staraSifra, string sifra, string naziv, string? opis)
    {
        return IzvrsiKomandu("IzmeniUslugu",
            new SqlParameter("@StaraSifra", staraSifra),
            new SqlParameter("@Sifra", sifra),
            new SqlParameter("@Naziv", naziv),
            new SqlParameter("@Opis", (object?)opis ?? DBNull.Value));
    }

    // brisanje usluge
    public int Obrisi(string sifra)
    {
        return IzvrsiKomandu("ObrisiUslugu",
            new SqlParameter("@Sifra", sifra));
    }
}
