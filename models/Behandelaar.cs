namespace ZiekenhuisDataGenerator.models
{
    public class Behandelaar: ReferenceTable
    {
        public long id;
        public string naam;

        public override void assign(long id, string naam)
        {
            this.id = id;
            this.naam = naam;
        }
    }
}