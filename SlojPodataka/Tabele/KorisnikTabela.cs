using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.Tabele;

// Konkretna tehnološka klasa za tabelu Korisnik (nasleđuje DBUtils).
public class KorisnikTabela : DBUtils
{
    // prijava - poziva stored proceduru sa korisničkim imenom i šifrom
    public DataTable Prijava(string korisnickoIme, string sifra)
    {
        return IzvrsiProceduru("DajKorisnikaPoKorisnickomImenuISifri",
            new SqlParameter("@KorisnickoIme", korisnickoIme),
            new SqlParameter("@Sifra", sifra));
    }
}
