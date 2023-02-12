namespace ZiekenhuisDataGenerator.models
{
    public class Verzekeraar: ReferenceTable
    {
        public long id;
        public string naam;

        public override void assign(long id, string name){
            this.id = id;
            this.naam = name;
        }
    }
}