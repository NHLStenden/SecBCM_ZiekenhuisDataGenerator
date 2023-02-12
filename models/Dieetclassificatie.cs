namespace ZiekenhuisDataGenerator.models
{
    public class Dieetclassificatie: ReferenceTable
    {
        public long Id;
        public string? Classificatie;

        public override void assign(long id, string? classificatie)
        {
            Id = id;
            Classificatie = classificatie;
        }
    }
}