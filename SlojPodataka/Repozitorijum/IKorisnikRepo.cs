using SlojPodataka.EntityModels;

namespace SlojPodataka.Repozitorijum;

public interface IKorisnikRepo
{
    KorisnikEntityModel? Prijava(string korisnickoIme, string sifra);
}
