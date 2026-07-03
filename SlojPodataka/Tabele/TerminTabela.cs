using System.Data;
using Microsoft.Data.SqlClient;

namespace SlojPodataka.Tabele;

// Konkretna tehnološka klasa za tabelu Termin (nasleđuje DBUtils).
public class TerminTabela : DBUtils
{
    // svi termini
    public DataTable DajSve()
    {
        return IzvrsiProceduru("DajSveTermine");
    }

    // slobodni termini za dati datum
    public DataTable DajSlobodneZaDatum(DateTime datum)
    {
        return IzvrsiProceduru("DajSlobodneTermineZaDatum",
            new SqlParameter("@Datum", datum.Date));
    }
}
