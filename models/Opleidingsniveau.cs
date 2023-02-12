namespace ZiekenhuisDataGenerator.models
{
    public class Opleidingsniveau: ReferenceTable
    {
        public long id;
        public string niveau;

        public override void assign(long id, string niveau)
        {
            this.id = id;
            this.niveau = niveau;
        }
    }
}