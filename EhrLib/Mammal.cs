namespace EhrLib
{
    public class Mammal : Pet
    {
        // The current data set has mammals which typically have 4 legs, however once we get more data the set could include mammals
        // with two legs like monkies or perhaps wounded dogs or cats with only 3.
        public override int NumberOfLegs => 4;
    }
}