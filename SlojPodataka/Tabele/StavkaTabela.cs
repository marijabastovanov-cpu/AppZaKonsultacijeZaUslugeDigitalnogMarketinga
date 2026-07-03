using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.Tabele;

// Konkretna tehnološka klasa za tabelu StavkaKonsultacije - delovi (nasleđuje DBUtils).
public class StavkaTabela : DBUtils
{
    // stavke jedne konsultacije
    public DataTable DajZaKonsultaciju(int idKonsultacija)
    {
        return IzvrsiProceduru("DajStavkeZaKonsultaciju",
            new SqlParameter("@IDKonsultacija", idKonsultacija));
    }

    // dodavanje stavke (jedne izabrane usluge)
    public int Dodaj(int idKonsultacija, string idUsluga, int prioritet, string? napomena)
    {
        return IzvrsiKomandu("DodajStavku",
            new SqlParameter("@IDKonsultacija", idKonsultacija),
            new SqlParameter("@IDUsluga", idUsluga),
            new SqlParameter("@Prioritet", prioritet),
            new SqlParameter("@Napomena", (object?)napomena ?? DBNull.Value));
    }
}
