namespace ZiekenhuisDataGenerator.models
{
    public class Leefstijl:  ReferenceTable
    {
        public long id;
        public string leefstijl;

        public override void assign(long id, string leefstijl)
        {
            this.id = id;
            this.leefstijl = leefstijl;
        }
    }
}