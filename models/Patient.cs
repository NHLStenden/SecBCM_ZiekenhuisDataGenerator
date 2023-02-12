using System;
using System.Collections.Generic;

namespace ZiekenhuisDataGenerator.models
{
    public class Patient
    {
        public long id;
        public string naam;
        public string adres_straat;
        public string adres_huisnr;
        public string adres_woonplaats;
        public DateTime geboortedatum;
        public string geslacht;
        public int start_gewicht;
        public string start_bloeddruk;
        public string contraindicaties;
        public string voorgeschiedenis;
        public Behandelaar behandelaar;
        public Verzekeraar verzekeraar;
        public Opleidingsniveau opleidingsniveau;
        public Dieetclassificatie dieetclassificatie;
        public Leefstijl leefstijl;

        public List<Meetwaarde> Meetwaarden;

        public Patient()
        {
        }

    }
}