// Klijentska validacija forme za zakazivanje (regularni izrazi)
document.addEventListener("DOMContentLoaded", function () {
    const forma = document.getElementById("formaZakazivanje");
    if (!forma) return;

    forma.addEventListener("submit", function (e) {
        const ime = forma.querySelector("[name='ImePrezimeKlijenta']");
        const email = forma.querySelector("[name='Email']");
        const telefon = forma.querySelector("[name='Telefon']");

        // regex pravila
        const regexIme = /^[A-Za-zČĆŽŠĐčćžšđ]+(\s[A-Za-zČĆŽŠĐčćžšđ]+)+$/;     // ime i prezime
        const regexEmail = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;                        // email
        const regexTelefon = /^0\d{8,9}$/;                                     // npr. 0641234567

        if (!regexIme.test(ime.value.trim())) {
            alert("Unesite ime i prezime (dve reči, samo slova).");
            ime.focus(); e.preventDefault(); return false;
        }
        if (!regexEmail.test(email.value.trim())) {
            alert("Email nije u ispravnom formatu.");
            email.focus(); e.preventDefault(); return false;
        }
        if (!regexTelefon.test(telefon.value.trim())) {
            alert("Telefon mora počinjati sa 0 i imati 9-10 cifara (npr. 0641234567).");
            telefon.focus(); e.preventDefault(); return false;
        }
        return true;
    });
});
