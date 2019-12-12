using System;

namespace EhrLib
{
    public class Pet
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Problem { get; set; }
        public virtual int NumberOfLegs { get; }
    }
}
