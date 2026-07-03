using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public interface IUslugaRepo
{
    List<UslugaEntityModel> DajSve();
    UslugaEntityModel? DajPoSifri(string sifra);
    void Dodaj(UslugaEntityModel usluga);
    void Izmeni(UslugaEntityModel usluga);
    void Obrisi(string sifra);
}
