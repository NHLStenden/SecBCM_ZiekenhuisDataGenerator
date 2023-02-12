using System;

namespace ZiekenhuisDataGenerator.models
{
    public class Meetwaarde
    {
        public long id;
        public Patient Patient;
        public string Device;
        public string Waarde1;
        public string? Waarde2;
        public string? Waarde3;
        public string? Waarde4;
        public DateTime Timestamp;
    }
}