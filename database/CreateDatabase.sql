CREATE DATABASE IF NOT EXISTS ziekenhuisdata collate utf8_general_ci;
USE ziekenhuisdata;

CREATE OR REPLACE USER 'ziekenhuis'@'localhost' IDENTIFIED WITH mysql_native_password AS PASSWORD('ziekenhuis');
grant all on ziekenhuisdata.* to 'ziekenhuis'@'localhost';

DROP TABLE IF EXISTS tbl_meetwaarden,
    tbl_patient,
    tbl_behandelaar,
    tbl_verzekeraar,
    ref_dieetclassificatie,
    ref_opleidingsniveau,
    ref_leefstijl;

CREATE TABLE tbl_behandelaar
(
    b_idBehandelaar INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    b_naam          VARCHAR(200)
) COMMENT 'Behandelaren van patienten';

CREATE TABLE tbl_verzekeraar
(
    v_idVerzekeraar INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    v_naam          VARCHAR(200)
) COMMENT 'Verzekeraars';
CREATE UNIQUE INDEX UniqueVerzekeraarNaam ON tbl_verzekeraar (v_naam);

CREATE TABLE ref_opleidingsniveau
(
    o_idOpleidingsniveau INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    o_niveau             VARCHAR(10)
) COMMENT 'Referentietabel voor opleidingsniveau';
CREATE UNIQUE INDEX UniqueOpleidingsniveau ON ref_opleidingsniveau (o_niveau);

CREATE TABLE ref_dieetclassificatie
(
    o_iddieetclassificatie INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    o_classificatie        VARCHAR(50)
) COMMENT 'Referentietabel voor dieetclassificatie';
CREATE UNIQUE INDEX UniqueClassificatie ON ref_dieetclassificatie (o_classificatie);

CREATE TABLE ref_leefstijl
(
    l_idLeefstijl INT AUTO_INCREMENT PRIMARY KEY NOT NULL,
    l_stijl       VARCHAR(50)
) COMMENT 'Levensstijl';
CREATE UNIQUE INDEX UniqueLeefstijl ON ref_leefstijl (l_stijl);

CREATE TABLE tbl_patient
(
    p_idPatient               INT AUTO_INCREMENT NOT NULL PRIMARY KEY,
    p_naam                    VARCHAR(200)       NOT NULL,
    p_adres_straat            VARBINARY(50)      NULL,
    p_adres_huisnr            VARCHAR(10)        NULL,
    p_adres_woonplaats        VARCHAR(100)       NULL,
    p_geboortedatum           DATETIME           NULL,
    p_geslacht                CHAR               NULL,
    p_start_gewicht           INT                NULL COMMENT 'Gewicht bij aanvang van de begeleiding',
    p_start_bloeddruk         VARCHAR(20)        NULL COMMENT 'Bloeddruk bij aanvang van de begeleiding',
    p_contraindicaties        bool               NULL COMMENT 'Zijn er contra indicaties?',
    p_voorgeschiedenis        VARCHAR(30)        NULL COMMENT 'Voorgeschiedenis: trauma / psychisch',
    p_fk_idBehandelaar        INT                NULL COMMENT 'Foreign key naar de behandelaar',
    p_fk_idVerzekeraar        INT                NULL COMMENT 'Foreign key naar de verzekeraar',
    p_fk_idOpleidingsniveau   INT                NULL COMMENT 'Foreign key naar opleidingsniveau',
    p_fk_idDieetclassificatie INT                NULL COMMENT 'Foreign key naar dieetclassificatie',
    p_fk_idLeefstijl          INT                NULL COMMENT 'Foreign key naar leefstijl',
    CONSTRAINT p_fk_idBehandelaar FOREIGN KEY (p_fk_idBehandelaar) REFERENCES tbl_behandelaar (b_idBehandelaar) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT p_fk_idVerzekeraar FOREIGN KEY (p_fk_idVerzekeraar) REFERENCES tbl_verzekeraar (v_idVerzekeraar) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT p_fk_idOpleidingsniveau FOREIGN KEY (p_fk_idOpleidingsniveau) REFERENCES ref_opleidingsniveau (o_idOpleidingsniveau) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT p_fk_idDieetclassificatie FOREIGN KEY (p_fk_idDieetclassificatie) REFERENCES ref_dieetclassificatie (o_iddieetclassificatie) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT p_fk_idLeefstijl FOREIGN KEY (p_fk_idLeefstijl) REFERENCES ref_leefstijl (l_idLeefstijl) ON DELETE CASCADE ON UPDATE CASCADE
) COMMENT 'Patienten';

CREATE TABLE tbl_meetwaarden
(
    m_idMeetwaarde INT         NOT NULL PRIMARY KEY AUTO_INCREMENT,
    m_fk_idPatient INT         NOT NULL,
    m_timestamp    DATETIME    NOT NULL,
    m_device       VARCHAR(20) NOT NULL,
    m_waarde       VARCHAR(20) NULL,
    CONSTRAINT m_fk_idPatient FOREIGN KEY (m_fk_idPatient) REFERENCES tbl_patient (p_idPatient) ON DELETE CASCADE ON UPDATE CASCADE
) COMMENT 'Meetwaarden van diverse sensoren en apparatuur';

INSERT INTO tbl_behandelaar (b_naam)
VALUES ('Mien van Assen'),
       ('Janny Vermeer'),
       ('Rosa de Hoek');

INSERT INTO ref_dieetclassificatie (o_classificatie)
VALUES ('Koolhydraatarm'),
       ('Zoutarm'),
       ('Vetarm');

INSERT INTO ref_leefstijl (l_stijl)
VALUES ('Obstructieve-slaapapneusyndroom'),
       ('Overmatig alcoholgebruik'),
       ('Hoogcalorische of nachtelijke voedingsinname'),
       ('Jojo-effecten (door crashdiëten)'),
       ('Immobiliteit (trauma, ziekte, pijn)'),
       ('Armoede'),
       ('Ongezonde leefomgeving');

INSERT INTO ref_opleidingsniveau (o_niveau)
VALUES ('HBO'),
       ('WO'),
       ('MBO'),
       ('VMBO')
;
-- https://www.independer.nl/zorgverzekering/fi/intro.aspx
INSERT INTO tbl_verzekeraar (v_naam)
VALUES ('Zilveren Kruis Achmea'),
       ('De Friesland'),
       ('A.S.R'),
       ('CZ'),
       ('CZdirect'),
       ('UnitedConsumers'),
       ('Ditzo'),
       ('FBTO'),
       ('Interpolis'),
       ('HEMA'),
       ('InTwente'),
       ('Just'),
       ('Jaaah'),
       ('De Friesland Zorgverzekeraar'),
       ('VGZ'),
       ('VGZbewuzt'),
       ('ZEKUR'),
       ('VinkVink'),
       ('ZieZo van Zilveren Kruis'),
       ('Zorg en Zekerheid'),
       ('Nationale Nederlanden'),
       ('OHRA'),
       ('ONVZ'),
       ('Pro Life'),
       ('IZA Zorgverzekeraar'),
       ('Menzis')
;

CREATE OR REPLACE VIEW vw_patient_meetwaarden AS
SELECT p.p_naam             as naam,
       p.p_idPatient        as id,
       p.p_adres_huisnr     as huisnr,
       p.p_adres_straat     as straat,
       p.p_adres_woonplaats as woonplaats,
       p.p_geboortedatum    as geboortedatum,
       p.p_start_bloeddruk  as start_bloeddruk,
       p.p_start_gewicht    as start_gewicht,
       p.p_contraindicaties as contraindicaties,
       p.p_geslacht         as geslacht,
       p.p_voorgeschiedenis as voorgeschiedenis,
       m_device             as apparaat,
       m_waarde             as meetwaarde,
       m_timestamp          as datum_meetwaarde,
       v_naam               as verzekeraar,
       b_naam               as behandelaar
FROM tbl_patient patient
         LEFT JOIN ref_dieetclassificatie rd on rd.o_iddieetclassificatie = patient.p_fk_idDieetclassificatie
         LEFT JOIN ref_leefstijl rl on rl.l_idLeefstijl = patient.p_fk_idLeefstijl
         LEFT JOIN tbl_behandelaar tb on tb.b_idBehandelaar = patient.p_fk_idBehandelaar
         LEFT JOIN ref_opleidingsniveau ro on ro.o_idOpleidingsniveau = patient.p_fk_idOpleidingsniveau
         LEFT JOIN tbl_verzekeraar tv on tv.v_idVerzekeraar = patient.p_fk_idVerzekeraar
         LEFT JOIN tbl_meetwaarden tm on tm.m_fk_idPatient = patient.p_idPatient
;