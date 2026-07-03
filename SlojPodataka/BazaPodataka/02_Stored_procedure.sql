/* =====================================================================
   RAZVOJ VIŠESLOJNOG SOFTVERA - Seminarski rad
   Tema: Zakazivanje konsultacija za usluge digitalnog marketinga
   Sloj podataka - STORED PROCEDURE
   ---------------------------------------------------------------------
   Pokrenuti POSLE skripte 01 (baza i tabele moraju već da postoje).
   ===================================================================== */

USE [DigitalniMarketing]
GO

/* =====================================================================
   STORED PROCEDURE
   ===================================================================== */

-- ---------- LOGIN ----------
CREATE PROCEDURE [DajKorisnikaPoKorisnickomImenuISifri]
( @KorisnickoIme nvarchar(20),
  @Sifra         nvarchar(30) )
AS
  SELECT * FROM Korisnik
  WHERE Korisnik.KorisnickoIme = @KorisnickoIme AND Korisnik.Sifra = @Sifra
GO

-- ---------- ŠIFARNIK TERMIN ----------
CREATE PROCEDURE [DajSveTermine]
AS
  SELECT * FROM Termin ORDER BY RedniBroj
GO

-- ---------- ŠIFARNIK USLUGA ----------
CREATE PROCEDURE [DajSveUsluge]
AS
  SELECT * FROM UslugaMarketinga ORDER BY Sifra
GO

CREATE PROCEDURE [DajUsluguPoSifri]
( @Sifra nvarchar(3) )
AS
  SELECT * FROM UslugaMarketinga WHERE Sifra = @Sifra
GO

CREATE PROCEDURE [DodajUslugu]
( @Sifra nvarchar(3), @Naziv nvarchar(50), @Opis nvarchar(200) )
AS
BEGIN
  INSERT INTO UslugaMarketinga (Sifra, Naziv, Opis) VALUES (@Sifra, @Naziv, @Opis)
END
GO

CREATE PROCEDURE [IzmeniUslugu]
( @StaraSifra nvarchar(3), @Sifra nvarchar(3), @Naziv nvarchar(50), @Opis nvarchar(200) )
AS
BEGIN
  UPDATE UslugaMarketinga SET Sifra = @Sifra, Naziv = @Naziv, Opis = @Opis
  WHERE Sifra = @StaraSifra
END
GO

CREATE PROCEDURE [ObrisiUslugu]
( @Sifra nvarchar(3) )
AS
BEGIN
  DELETE FROM UslugaMarketinga WHERE Sifra = @Sifra
END
GO

-- ---------- GLAVNA TABELA: KONSULTACIJA (CRUD) ----------
-- Tabelarni prikaz sa JOIN na termin
CREATE PROCEDURE [DajSveKonsultacije]
AS
  SELECT K.ID, K.ImePrezimeKlijenta, K.NazivFirme, K.Email, K.Telefon,
         K.DatumKonsultacije, K.IDTermin, T.Naziv AS NazivTermina,
         K.Napomena, K.DatumPodnosenja
  FROM Konsultacija K
  INNER JOIN Termin T ON K.IDTermin = T.Sifra
  ORDER BY K.DatumKonsultacije, T.RedniBroj
GO

-- Prikaz pojedinačnog zapisa (celina)
CREATE PROCEDURE [DajKonsultacijuPoID]
( @ID int )
AS
  SELECT K.ID, K.ImePrezimeKlijenta, K.NazivFirme, K.Email, K.Telefon,
         K.DatumKonsultacije, K.IDTermin, T.Naziv AS NazivTermina,
         K.Napomena, K.DatumPodnosenja
  FROM Konsultacija K
  INNER JOIN Termin T ON K.IDTermin = T.Sifra
  WHERE K.ID = @ID
GO

-- Tabelarni prikaz sa FILTEROM (po datumu)
CREATE PROCEDURE [DajKonsultacijePoDatumu]
( @Datum date )
AS
  SELECT K.ID, K.ImePrezimeKlijenta, K.NazivFirme, K.Email, K.Telefon,
         K.DatumKonsultacije, K.IDTermin, T.Naziv AS NazivTermina,
         K.Napomena, K.DatumPodnosenja
  FROM Konsultacija K
  INNER JOIN Termin T ON K.IDTermin = T.Sifra
  WHERE K.DatumKonsultacije = @Datum
  ORDER BY T.RedniBroj
GO

-- Unos master zapisa; vraća ID nove konsultacije (za master-detail transakciju)
CREATE PROCEDURE [DodajKonsultaciju]
( @ImePrezimeKlijenta nvarchar(60),
  @NazivFirme         nvarchar(60),
  @Email              nvarchar(60),
  @Telefon            nvarchar(20),
  @DatumKonsultacije  date,
  @IDTermin           nvarchar(2),
  @Napomena           nvarchar(200) )
AS
BEGIN
  INSERT INTO Konsultacija
    (ImePrezimeKlijenta, NazivFirme, Email, Telefon, DatumKonsultacije, IDTermin, Napomena)
  VALUES
    (@ImePrezimeKlijenta, @NazivFirme, @Email, @Telefon, @DatumKonsultacije, @IDTermin, @Napomena)

  SELECT CAST(SCOPE_IDENTITY() AS int) AS NoviID
END
GO

CREATE PROCEDURE [IzmeniKonsultaciju]
( @ID                 int,
  @ImePrezimeKlijenta nvarchar(60),
  @NazivFirme         nvarchar(60),
  @Email              nvarchar(60),
  @Telefon            nvarchar(20),
  @DatumKonsultacije  date,
  @IDTermin           nvarchar(2),
  @Napomena           nvarchar(200) )
AS
BEGIN
  UPDATE Konsultacija
  SET ImePrezimeKlijenta = @ImePrezimeKlijenta,
      NazivFirme = @NazivFirme,
      Email = @Email,
      Telefon = @Telefon,
      DatumKonsultacije = @DatumKonsultacije,
      IDTermin = @IDTermin,
      Napomena = @Napomena
  WHERE ID = @ID
END
GO

CREATE PROCEDURE [ObrisiKonsultaciju]
( @ID int )
AS
BEGIN
  DELETE FROM Konsultacija WHERE ID = @ID  -- stavke se brišu kaskadno (FK ON DELETE CASCADE)
END
GO

-- ---------- DETAIL: STAVKE KONSULTACIJE ----------
CREATE PROCEDURE [DajStavkeZaKonsultaciju]
( @IDKonsultacija int )
AS
  SELECT S.ID, S.IDKonsultacija, S.IDUsluga, U.Naziv AS NazivUsluge,
         S.Prioritet, S.Napomena
  FROM StavkaKonsultacije S
  INNER JOIN UslugaMarketinga U ON S.IDUsluga = U.Sifra
  WHERE S.IDKonsultacija = @IDKonsultacija
  ORDER BY S.Prioritet
GO

CREATE PROCEDURE [DodajStavku]
( @IDKonsultacija int, @IDUsluga nvarchar(3), @Prioritet int, @Napomena nvarchar(150) )
AS
BEGIN
  INSERT INTO StavkaKonsultacije (IDKonsultacija, IDUsluga, Prioritet, Napomena)
  VALUES (@IDKonsultacija, @IDUsluga, @Prioritet, @Napomena)
END
GO

CREATE PROCEDURE [ObrisiStavkeZaKonsultaciju]
( @IDKonsultacija int )
AS
BEGIN
  DELETE FROM StavkaKonsultacije WHERE IDKonsultacija = @IDKonsultacija
END
GO

-- ---------- POSLOVNO PRAVILO (OGRANIČENJE) ----------
-- Broj već zauzetih termina za dati datum
CREATE PROCEDURE [DajBrojZauzetihTerminaZaDatum]
( @Datum date )
AS
  SELECT COUNT(*) AS Zauzeto FROM Konsultacija WHERE DatumKonsultacije = @Datum
GO

-- Slobodni termini za dati datum (termini koji još nisu zakazani tog dana)
CREATE PROCEDURE [DajSlobodneTermineZaDatum]
( @Datum date )
AS
  SELECT T.Sifra, T.RedniBroj, T.VremeOd, T.VremeDo, T.Naziv
  FROM Termin T
  WHERE T.Sifra NOT IN (
        SELECT K.IDTermin FROM Konsultacija K WHERE K.DatumKonsultacije = @Datum )
  ORDER BY T.RedniBroj
GO
