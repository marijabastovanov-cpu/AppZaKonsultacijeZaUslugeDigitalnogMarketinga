/* =====================================================================
   RAZVOJ VIŠESLOJNOG SOFTVERA - Seminarski rad
   Tema: Zakazivanje konsultacija za usluge digitalnog marketinga
   Sloj podataka - kreiranje baze, tabela i stored procedura
   ---------------------------------------------------------------------
   Poslovno pravilo (OGRANIČENJE):
   AKO za izabrani datum nema slobodnih termina (svih 6 dnevnih termina
   je zauzeto) ONDA se za taj datum ne može zakazati nova konsultacija.
   Maksimalan broj termina po danu je PARAMETRIZOVAN (čita se iz XML/JSON
   u sloju servisa); ovde je strukturno ograničen na 6 (šifarnik Termin)
   uz UNIQUE(DatumKonsultacije, IDTermin).
   ===================================================================== */

USE [master]
GO

-- Ako baza vec postoji: zatvori sve konekcije i obrisi je
-- (omogucava ponovno pokretanje skripte bez gresaka)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DigitalniMarketing')
BEGIN
    ALTER DATABASE [DigitalniMarketing] SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE [DigitalniMarketing]
END
GO

CREATE DATABASE [DigitalniMarketing]
GO

USE [DigitalniMarketing]
GO

/* =====================================================================
   1. NEZAVISNA TABELA: KORISNIK  (login: admin / sekretar)
   ===================================================================== */
CREATE TABLE [dbo].[Korisnik](
    [ID]            [int] IDENTITY(1,1) NOT NULL,
    [ImePrezime]    [nvarchar](60)  NOT NULL,
    [KorisnickoIme] [nvarchar](20)  NOT NULL,
    [Sifra]         [nvarchar](30)  NOT NULL,
    [Uloga]         [nvarchar](15)  NOT NULL    -- admin | sekretar
)
GO

ALTER TABLE [dbo].[Korisnik]
ADD CONSTRAINT [PK_Korisnik] PRIMARY KEY CLUSTERED ([ID] ASC)
GO

ALTER TABLE [dbo].[Korisnik]
ADD CONSTRAINT [UQ_Korisnik_KorisnickoIme] UNIQUE ([KorisnickoIme])
GO

/* =====================================================================
   2. ŠIFARNIK: TERMIN  (6 fiksnih dnevnih termina po 60 minuta)
   ===================================================================== */
CREATE TABLE [dbo].[Termin](
    [Sifra]     [nvarchar](2)  NOT NULL,   -- T1..T6
    [RedniBroj] [int]          NOT NULL,
    [VremeOd]   [nvarchar](5)  NOT NULL,   -- 08:00
    [VremeDo]   [nvarchar](5)  NOT NULL,   -- 09:00
    [Naziv]     [nvarchar](20) NOT NULL    -- 08:00-09:00
)
GO

ALTER TABLE [dbo].[Termin]
ADD CONSTRAINT [PK_Termin] PRIMARY KEY CLUSTERED ([Sifra] ASC)
GO

/* =====================================================================
   3. ŠIFARNIK: USLUGA MARKETINGA  (katalog usluga digitalnog marketinga)
   ===================================================================== */
CREATE TABLE [dbo].[UslugaMarketinga](
    [Sifra] [nvarchar](3)   NOT NULL,   -- U01..Unn
    [Naziv] [nvarchar](50)  NOT NULL,
    [Opis]  [nvarchar](200) NULL
)
GO

ALTER TABLE [dbo].[UslugaMarketinga]
ADD CONSTRAINT [PK_UslugaMarketinga] PRIMARY KEY CLUSTERED ([Sifra] ASC)
GO

/* =====================================================================
   4. GLAVNA TABELA (MASTER): KONSULTACIJA
      - povezana relacijom sa šifarnikom TERMIN
      - UNIQUE(DatumKonsultacije, IDTermin): jedan termin = jedno
        zakazivanje po danu  ->  najviše 6 konsultacija dnevno
   ===================================================================== */
CREATE TABLE [dbo].[Konsultacija](
    [ID]                 [int] IDENTITY(1,1) NOT NULL,
    [ImePrezimeKlijenta] [nvarchar](60)  NOT NULL,
    [NazivFirme]         [nvarchar](60)  NULL,
    [Email]              [nvarchar](60)  NOT NULL,
    [Telefon]            [nvarchar](20)  NOT NULL,
    [DatumKonsultacije]  [date]          NOT NULL,
    [IDTermin]           [nvarchar](2)   NOT NULL,
    [Napomena]           [nvarchar](200) NULL,
    [DatumPodnosenja]    [datetime]      NOT NULL CONSTRAINT [DF_Konsultacija_Datum] DEFAULT (GETDATE())
)
GO

ALTER TABLE [dbo].[Konsultacija]
ADD CONSTRAINT [PK_Konsultacija] PRIMARY KEY CLUSTERED ([ID] ASC)
GO

ALTER TABLE [dbo].[Konsultacija]
ADD CONSTRAINT [FK_Konsultacija_Termin] FOREIGN KEY ([IDTermin])
REFERENCES [dbo].[Termin] ([Sifra]) ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Konsultacija]
ADD CONSTRAINT [UQ_Konsultacija_DatumTermin] UNIQUE ([DatumKonsultacije], [IDTermin])
GO

/* =====================================================================
   5. DETAIL TABELA: STAVKA KONSULTACIJE  (master-detail deo dokumenta)
      - usluge digitalnog marketinga izabrane za jednu konsultaciju
   ===================================================================== */
CREATE TABLE [dbo].[StavkaKonsultacije](
    [ID]             [int] IDENTITY(1,1) NOT NULL,
    [IDKonsultacija] [int]           NOT NULL,
    [IDUsluga]       [nvarchar](3)   NOT NULL,
    [Prioritet]      [int]           NOT NULL CONSTRAINT [DF_Stavka_Prioritet] DEFAULT (1),
    [Napomena]       [nvarchar](150) NULL
)
GO

ALTER TABLE [dbo].[StavkaKonsultacije]
ADD CONSTRAINT [PK_StavkaKonsultacije] PRIMARY KEY CLUSTERED ([ID] ASC)
GO

ALTER TABLE [dbo].[StavkaKonsultacije]
ADD CONSTRAINT [FK_Stavka_Konsultacija] FOREIGN KEY ([IDKonsultacija])
REFERENCES [dbo].[Konsultacija] ([ID]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[StavkaKonsultacije]
ADD CONSTRAINT [FK_Stavka_Usluga] FOREIGN KEY ([IDUsluga])
REFERENCES [dbo].[UslugaMarketinga] ([Sifra]) ON UPDATE CASCADE
GO

/* =====================================================================
   POČETNI PODACI (seed)
   ===================================================================== */
-- Korisnici
INSERT INTO [dbo].[Korisnik] ([ImePrezime],[KorisnickoIme],[Sifra],[Uloga]) VALUES
(N'Administrator sistema', N'admin',    N'admin123',    N'admin'),
(N'Jovana Jovanović',      N'sekretar', N'sekretar123', N'sekretar')
GO

-- Termini (6 fiksnih dnevnih termina po 60 min)
INSERT INTO [dbo].[Termin] ([Sifra],[RedniBroj],[VremeOd],[VremeDo],[Naziv]) VALUES
(N'T1', 1, N'08:00', N'09:00', N'08:00-09:00'),
(N'T2', 2, N'09:00', N'10:00', N'09:00-10:00'),
(N'T3', 3, N'10:00', N'11:00', N'10:00-11:00'),
(N'T4', 4, N'11:00', N'12:00', N'11:00-12:00'),
(N'T5', 5, N'12:00', N'13:00', N'12:00-13:00'),
(N'T6', 6, N'13:00', N'14:00', N'13:00-14:00')
GO

-- Usluge digitalnog marketinga
INSERT INTO [dbo].[UslugaMarketinga] ([Sifra],[Naziv],[Opis]) VALUES
(N'U01', N'SEO optimizacija',            N'Optimizacija sajta za pretraživače'),
(N'U02', N'Google Ads (PPC)',           N'Plaćene kampanje na Google mreži'),
(N'U03', N'Upravljanje društvenim mrežama', N'Vođenje naloga na društvenim mrežama'),
(N'U04', N'Email marketing',            N'Kreiranje i slanje email kampanja'),
(N'U05', N'Marketing sadržaja',         N'Izrada i optimizacija sadržaja'),
(N'U06', N'Veb analitika i izveštaji',  N'Praćenje i analiza rezultata'),
(N'U07', N'Influencer marketing',       N'Saradnja sa influenserima'),
(N'U08', N'Izrada veb sajta',           N'Izrada i optimizacija landing stranica'),
(N'U09', N'Video montaža',              N'Editovanje i snimanje videa, smišljanje sadržaja'),
(N'U10', N'Grafički dizajn',            N'Izrada vizuala')
GO
