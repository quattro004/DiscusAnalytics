using System.Collections.Generic;

namespace EhrLib
{
    public struct PetAnalysisResult
    {
        public PetAnalysisResult(int numberOfMalePets, int numberOfPetsWith4Legs, double averageAgeOfDogs, 
            IEnumerable<string> reptileProblems, IEnumerable<string> mammalNames)
        {
            NumberOfMalePets = numberOfMalePets;
            NumberOfPetsWith4Legs = numberOfPetsWith4Legs;
            AverageAgeOfDogs = averageAgeOfDogs;
            ReptileProblems = reptileProblems;
            MammalNames = mammalNames;
        }

        public int NumberOfMalePets { get; }
        public int NumberOfPetsWith4Legs { get; }
        public double AverageAgeOfDogs { get;}
        public IEnumerable<string> ReptileProblems { get; }
        public IEnumerable<string> MammalNames { get; }
    }
}