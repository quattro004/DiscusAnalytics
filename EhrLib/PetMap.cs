using CsvHelper.Configuration;

namespace EhrLib
{
    public sealed class PetMap : ClassMap<Pet>
    {
        public PetMap()
        {
            Map(m => m.Name).Index(0);
            Map(m => m.Type).Index(1);
            Map(m => m.Age).Index(2);
            Map(m => m.Gender).Index(3);
            Map(m => m.Problem).Index(4);
        }
    }
}