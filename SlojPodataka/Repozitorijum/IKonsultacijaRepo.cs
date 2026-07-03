using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public interface IKonsultacijaRepo
{
    List<KonsultacijaEntityModel> DajSve();
    List<KonsultacijaEntityModel> DajPoDatumu(DateTime datum);
    KonsultacijaEntityModel? DajPoId(int id);
    int DajBrojZauzetih(DateTime datum);
    List<TerminEntityModel> DajSlobodneTermine(DateTime datum);
    bool DodajSaStavkama(KonsultacijaEntityModel konsultacija);   // master-detail + transakcija
    void Izmeni(KonsultacijaEntityModel konsultacija);
    void Obrisi(int id);
}
