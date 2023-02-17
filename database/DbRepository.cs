using System;
using System.Collections.Generic;
using Bogus;
using MySqlConnector;
using ZiekenhuisDataGenerator.models;

namespace ZiekenhuisDataGenerator.database
{
    public class DbRepository
    {
        public List<Leefstijl> Leefstijlen;
        public List<Dieetclassificatie> Dieetclassificaties;
        public List<Opleidingsniveau> Opleidingsniveaus;
        public List<Behandelaar> Behandelaars;
        public List<Verzekeraar> Verzekeraars;

        public DbRepository()
        {
        }

        public void GetAllReferenceTables()
        {
            this.Leefstijlen = GetAllLeefstijl();
            this.Dieetclassificaties = GetAllDieetclassificaties();
            this.Opleidingsniveaus = GetAllOpleidingsniveaus();
            this.Behandelaars = GetAllBehandelaars();
            this.Verzekeraars = GetAllVerzekeraars();
        }

        public List<Patient> CreateRandomPatienten(int nrOfPatients)
        {
            List<string> geslacht = new List<string>(2);
            geslacht.Add("M");
            geslacht.Add("V");
            List<string> voorgeschiedenis = new List<string>(2);
            voorgeschiedenis.Add("Trauma");
            voorgeschiedenis.Add("Psychisch");
            Faker<Patient> patientenFaker = new Faker<Patient>()
                    .RuleFor(p => p.naam, f => f.Name.FullName())
                    .RuleFor(p => p.adres_huisnr, f => f.Random.Number(1, 100).ToString())
                    .RuleFor(p => p.adres_straat, f => f.Address.StreetName())
                    .RuleFor(p => p.adres_woonplaats, f => f.Address.City())
                    .RuleFor(p => p.start_bloeddruk, f => f.Random.Number(60, 150).ToString() +
                                                          "/" + f.Random.Number(80, 150).ToString())
                    .RuleFor(p => p.start_gewicht, f => f.Random.Number(45, 150))
                    .RuleFor(p => p.contraindicaties, f => f.Random.Number(0, 1).ToString())
                    .RuleFor(p => p.leefstijl, f => f.PickRandom(Leefstijlen))
                    .RuleFor(p => p.geslacht, f => f.PickRandom(geslacht))
                    .RuleFor(p => p.opleidingsniveau, f => f.PickRandom(Opleidingsniveaus))
                    .RuleFor(p => p.dieetclassificatie, f => f.PickRandom(Dieetclassificaties))
                    .RuleFor(p => p.behandelaar, f => f.PickRandom(Behandelaars))
                    .RuleFor(p => p.verzekeraar, f => f.PickRandom(Verzekeraars))
                    .RuleFor(p => p.voorgeschiedenis, f => f.PickRandom(voorgeschiedenis))
                    .RuleFor(p => p.geboortedatum,
                        f => f.Date.Between(
                            new DateTime(1980, 1, 1),
                            new DateTime(2002, 1, 1)))
                ;
            List<Patient> patienten = patientenFaker.Generate(nrOfPatients);

            return patienten;
        }

        public List<Leefstijl> GetAllLeefstijl()
        {
            return GetSimpleTable<Leefstijl>("ref_leefstijl", "l_idLeefstijl", "l_stijl");
        }

        public List<Verzekeraar> GetAllVerzekeraars()
        {
            return GetSimpleTable<Verzekeraar>("tbl_verzekeraar", "v_idVerzekeraar", "v_naam");
        }

        public List<Dieetclassificatie> GetAllDieetclassificaties()
        {
            return GetSimpleTable<Dieetclassificatie>("ref_dieetclassificatie", "o_iddieetclassificatie",
                "o_classificatie");
        }

        public List<Opleidingsniveau> GetAllOpleidingsniveaus()
        {
            return GetSimpleTable<Opleidingsniveau>("ref_opleidingsniveau", "o_idOpleidingsniveau", "o_niveau");
        }

        public List<Behandelaar> GetAllBehandelaars()
        {
            return GetSimpleTable<Behandelaar>("tbl_behandelaar", "b_idBehandelaar", "b_naam");
        }

        public List<T> GetSimpleTable<T>(string tablename, string idFieldname, string nameFieldname)
            where T : ReferenceTable, new()
        {
            string sql = "SELECT * FROM " + tablename;
            List<T> result = new List<T>();

            using (MySqlCommand statement = MySQLDBContext.Instance.ConstructStatement(sql))
            {
                statement.Prepare();
                MySqlDataReader data = statement.ExecuteReader();
                while (data.Read())
                {
                    T record = new T();
                    record.assign(data.GetInt32(idFieldname), data.GetString(nameFieldname));
                    result.Add(record);
                }

                data.Close();
            }

            return result;
        }

        public bool SaveOnePatient(Patient patient)
        {
            bool result = false;
            string sql = @"INSERT INTO tbl_patient (
            p_naam,p_adres_straat,p_adres_huisnr,p_adres_woonplaats,p_geboortedatum,p_geslacht,p_start_gewicht,p_start_bloeddruk,
            p_contraindicaties,p_voorgeschiedenis,p_fk_idBehandelaar,p_fk_idVerzekeraar,p_fk_idOpleidingsniveau,
                         p_fk_idDieetclassificatie,p_fk_idLeefstijl
            ) VALUES 
                     (
                      @naam,
                      @adres_straat,
                      @adres_huisnr,
                      @adres_woonplaats,
                      @geboortedatum,
                      @geslacht,
                      @start_gewicht,
                      @start_bloeddruk,
                      @contraindicaties,
                      @voorgeschiedenis,
                      @fk_idBehandelaar,
                      @fk_idVerzekeraar,
                      @fk_idOpleidingsniveau,
                      @fk_idDieetclassificatie,
                      @fk_idLeefstijl
                )";
            using (MySqlCommand statement = MySQLDBContext.Instance.ConstructStatement(sql))
            {
                statement.Parameters.AddWithValue("@naam", patient.naam);
                statement.Parameters.AddWithValue("@adres_straat", patient.adres_straat);
                statement.Parameters.AddWithValue("@adres_huisnr", patient.adres_huisnr);
                statement.Parameters.AddWithValue("@adres_woonplaats", patient.adres_woonplaats);
                statement.Parameters.AddWithValue("@geboortedatum", patient.geboortedatum);
                statement.Parameters.AddWithValue("@geslacht", patient.geslacht);
                statement.Parameters.AddWithValue("@start_gewicht", patient.start_gewicht);
                statement.Parameters.AddWithValue("@start_bloeddruk", patient.start_bloeddruk);
                statement.Parameters.AddWithValue("@contraindicaties", patient.contraindicaties);
                statement.Parameters.AddWithValue("@voorgeschiedenis", patient.voorgeschiedenis);
                statement.Parameters.AddWithValue("@fk_idBehandelaar", patient.behandelaar.id);
                statement.Parameters.AddWithValue("@fk_idVerzekeraar", patient.verzekeraar.id);
                statement.Parameters.AddWithValue("@fk_idOpleidingsniveau", patient.opleidingsniveau.id);
                statement.Parameters.AddWithValue("@fk_idDieetclassificatie", patient.dieetclassificatie.Id);
                statement.Parameters.AddWithValue("@fk_idLeefstijl", patient.leefstijl.id);
                statement.Prepare();
                result = statement.ExecuteNonQuery() == 1;
                patient.id = statement.LastInsertedId;
                if (result)
                {
                    SaveMeetwaardenForPatient(patient);
                }
            }

            return result;
        }

        public void SavePatientList(List<Patient> patients)
        {
            foreach (Patient patient in patients)
            {
                this.SaveOnePatient(patient);
            }
        }

        public List<Meetwaarde> GenerateMeetwaardenForPatient(Patient patient, int nrOfMeasurementsPerPatientMin,
            int nrOfMeasurementsPerPatientMax)
        {
            List<Meetwaarde> waarden = new List<Meetwaarde>();

            DateTime startDate = new DateTime(2018, 1, 1);

            string bloeddruk = patient.start_bloeddruk;
            int bloeddruk_hoog = int.Parse(bloeddruk.Split("/")[0]);
            int bloeddruk_laag = int.Parse(bloeddruk.Split("/")[1]);
            Randomizer rnd = new Randomizer();
            int nrOfMeetwaarden = rnd.Number(nrOfMeasurementsPerPatientMin, nrOfMeasurementsPerPatientMax);
            for (int i = 0; i < nrOfMeetwaarden; i++)
            {
                Meetwaarde meetwaarde = new Meetwaarde();
                meetwaarde.Patient = patient;

                switch (rnd.Number(0, 2))
                {
                    case 0:
                        meetwaarde.Device = "Bloeddruk";
                        int deviation_low = rnd.Number(-10, 15);
                        int deviation_high = rnd.Number(-10, 15);
                        meetwaarde.Waarde1 = (bloeddruk_hoog + deviation_high).ToString();
                        meetwaarde.Waarde2 = (bloeddruk_laag + deviation_low).ToString();
                        break;
                    case 1:
                        meetwaarde.Device = "Weegschaal";
                        meetwaarde.Waarde1 = (patient.start_gewicht + rnd.Number(-2, +3)).ToString();
                        break;
                    case 2:
                        meetwaarde.Device = "Bloedsuiker";
                        meetwaarde.Waarde1 = rnd.Number(3, 10).ToString();
                        break;
                }

                meetwaarde.Timestamp = startDate.AddDays(i + rnd.Number(0, 5));
                waarden.Add(meetwaarde);
            }

            return waarden;
        }

        public bool SaveOneMeetwaarde(Meetwaarde meetwaarde)
        {
            bool result = false;
            string sql =
                @"INSERT INTO tbl_meetwaarden (
                             m_fk_idPatient,
                             m_timestamp,
                             m_device,
                             m_waarde1,
                             m_waarde2,
                             m_waarde3,
                             m_waarde4
                             ) VALUES 
                     (@fk_idPatient,
                      @timestamp,
                      @device,
                      @value1,
                      @value2,
                      @value3,
                      @value4
                      )";
            using (MySqlCommand statement = MySQLDBContext.Instance.ConstructStatement(sql))
            {
                statement.Parameters.AddWithValue("@fk_idPatient", meetwaarde.Patient.id);
                statement.Parameters.AddWithValue("@timestamp", meetwaarde.Timestamp);
                statement.Parameters.AddWithValue("@device", meetwaarde.Device);
                statement.Parameters.AddWithValue("@value1", meetwaarde.Waarde1);
                statement.Parameters.AddWithValue("@value2", meetwaarde.Waarde2);
                statement.Parameters.AddWithValue("@value3", meetwaarde.Waarde3);
                statement.Parameters.AddWithValue("@value4", meetwaarde.Waarde4);
                statement.Prepare();
                result = statement.ExecuteNonQuery() == 1;
                meetwaarde.id = statement.LastInsertedId;
            }

            return result;
        }

        public bool SaveMeetwaardenForPatient(Patient patient)
        {
            bool result = false;
            foreach (Meetwaarde meetwaarde in patient.Meetwaarden)
            {
                result = this.SaveOneMeetwaarde(meetwaarde);
                if (!result) break;
            }

            return result;
        }
    }
}