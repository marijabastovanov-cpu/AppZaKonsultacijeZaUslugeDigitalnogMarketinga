using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.Tabele;

// Bazna tehnološka klasa za rad sa bazom (ADO.NET).
// Sadrži konekcioni string i zajedničke metode za izvršavanje stored procedura.
public abstract class DBUtils
{
    // konekcioni string ka bazi 
    protected readonly string konekcioniString =
        "Server=.\\SQLEXPRESS;Database=DigitalniMarketing;Trusted_Connection=True;TrustServerCertificate=True";

    // otvaranje konekcije
    protected SqlConnection DajKonekciju()
    {
        return new SqlConnection(konekcioniString);
    }

    // izvršavanje stored procedure koja vraća rezultat (SELECT) kao tabelu
    protected DataTable IzvrsiProceduru(string nazivProcedure, params SqlParameter[] parametri)
    {
        using SqlConnection konekcija = DajKonekciju();
        using SqlCommand komanda = new SqlCommand(nazivProcedure, konekcija);
        komanda.CommandType = CommandType.StoredProcedure;
        komanda.Parameters.AddRange(parametri);

        SqlDataAdapter adapter = new SqlDataAdapter(komanda);
        DataTable tabela = new DataTable();
        adapter.Fill(tabela);
        return tabela;
    }

    // izvršavanje stored procedure bez rezultata (INSERT / UPDATE / DELETE)
    protected int IzvrsiKomandu(string nazivProcedure, params SqlParameter[] parametri)
    {
        using SqlConnection konekcija = DajKonekciju();
        using SqlCommand komanda = new SqlCommand(nazivProcedure, konekcija);
        komanda.CommandType = CommandType.StoredProcedure;
        komanda.Parameters.AddRange(parametri);

        konekcija.Open();
        return komanda.ExecuteNonQuery();
    }

    // izvršavanje stored procedure koja vraća jednu vrednost (npr. novi ID, broj redova)
    protected object? IzvrsiSkalar(string nazivProcedure, params SqlParameter[] parametri)
    {
        using SqlConnection konekcija = DajKonekciju();
        using SqlCommand komanda = new SqlCommand(nazivProcedure, konekcija);
        komanda.CommandType = CommandType.StoredProcedure;
        komanda.Parameters.AddRange(parametri);

        konekcija.Open();
        return komanda.ExecuteScalar();
    }
}
