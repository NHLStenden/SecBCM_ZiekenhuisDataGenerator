using System;
using System.Collections.Generic;
using ZiekenhuisDataGenerator.database;
using ZiekenhuisDataGenerator.models;

DbRepository repo = new DbRepository();
repo.GetAllReferenceTables();

/**
 * Pas onderstaande regel aan om te zorgen dat het juiste aantal patienten aangemaakt wordt (nu 50 patienten)
 */
List<Patient> patients = repo.CreateRandomPatienten(50);

foreach (Patient patient in patients)
{
    Console.WriteLine("Patient:");
    Console.WriteLine("- naam = {0}",patient.naam);
    Console.WriteLine("- adres = {0} {1} {2}",patient.adres_straat, patient.adres_huisnr, patient.adres_woonplaats);
    Console.WriteLine("- behandelaar = {0}",patient.behandelaar.naam);
    Console.WriteLine("- contraindicaties = {0}",patient.contraindicaties == "1" ? "Ja" : "Nee");
    Console.WriteLine("- verzekeraar = {0}",patient.verzekeraar.naam);
    Console.WriteLine("- geboortedatum = {0}",patient.geboortedatum.Date.ToLongDateString());
    Console.WriteLine("- geslacht = {0}",patient.geslacht);
    Console.WriteLine("- start_bloeddruk = {0}",patient.start_bloeddruk);
    Console.WriteLine("- start_gewicht = {0}",patient.start_gewicht);
    Console.WriteLine("- opleidingsniveau = {0}",patient.opleidingsniveau.niveau);
    Console.WriteLine("- dieetclassificatie = {0}",patient.dieetclassificatie.Classificatie);
    Console.WriteLine("- voorgeschiedenis = {0}",patient.voorgeschiedenis);

    Console.WriteLine("* Genereren meetwaarden");
    /**
     * Pas onderstaande regel aan om meer of minder patient meetwaarden te genereren
     * (getallen zijn minimum en maximum aantal meetwaarden per patient)
     */
    patient.Meetwaarden = repo.GenerateMeetwaardenForPatient(patient, 50, 200);
}
Console.WriteLine("--------------------------- Opslaan -----------------");
repo.SavePatientList(patients);
Console.WriteLine("--------------------------- Gereed -----------------");

