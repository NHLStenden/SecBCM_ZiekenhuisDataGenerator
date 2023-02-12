using System;

namespace ZiekenhuisDataGenerator.models
{
    public class Meetwaarde
    {
        public long id;
        public Patient Patient;
        public string Device;
        public string Waarde;
        public DateTime Timestamp;
    }
}