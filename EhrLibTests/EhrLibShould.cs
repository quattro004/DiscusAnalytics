using System;
using System.IO;
using System.Linq;
using EhrLib;
using FluentAssertions;
using Xunit;

namespace EhrLibTests
{
    public class EhrLibShould
    {
        [Fact]
        public void load_pets_from_csv()
        {
            var petLoader = new PetLoader();
            var dataPath = Path.Combine(AppContext.BaseDirectory, "Data/PetData.csv");
            var pets = petLoader.Load(dataPath);

            pets.Should().NotBeNullOrEmpty();
            pets.Count().Should().Be(9); 
        }

        [Fact]
        public void load_mammals_from_csv()
        {
            var petLoader = new PetLoader();
            var dataPath = Path.Combine(AppContext.BaseDirectory, "Data/PetData.csv");
            var pets = petLoader.Load(dataPath);
            var mammals = from pet in pets
                          where pet is Mammal
                          select pet;
            mammals.Should().NotBeNullOrEmpty();
            mammals.Count().Should().Be(5); 
        }

        
        [Fact]
        public void load_reptiles_from_csv()
        {
            var petLoader = new PetLoader();
            var dataPath = Path.Combine(AppContext.BaseDirectory, "Data/PetData.csv");
            var pets = petLoader.Load(dataPath);
            var mammals = from pet in pets
                          where pet is Reptile
                          select pet;
            mammals.Should().NotBeNullOrEmpty();
            mammals.Count().Should().Be(2); 
        }

        
        [Fact]
        public void load_birds_from_csv()
        {
            var petLoader = new PetLoader();
            var dataPath = Path.Combine(AppContext.BaseDirectory, "Data/PetData.csv");
            var pets = petLoader.Load(dataPath);
            var mammals = from pet in pets
                          where pet is Bird
                          select pet;
            mammals.Should().NotBeNullOrEmpty();
            mammals.Count().Should().Be(2); 
        }

        [Fact]
        public void save_pets_to_repo()
        {
            var petLoader = new PetLoader();
            var dataPath = Path.Combine(AppContext.BaseDirectory, "Data/PetData.csv");
            var pets = petLoader.Load(dataPath);

            petLoader.Save(pets).Should().BeTrue();
            File.Exists(petLoader.PetRepoPath).Should().BeTrue();            
        }

        [Fact]
        public void analyze_pet_data()
        {
            var petResearcher = new PetResearcher();
            // Can run the VetEhr program to create the repo
            // I ran into problems with encryption on Ubuntu, File.Encrypt wasn't supported and the existing code
            // is getting a invalid padding exception.
            var results = petResearcher.Analyze("/home/reese/src/DiscusAnalytics/EhrLibTests/bin/Debug/netcoreapp3.0/PetRepo/Pets.json");

            results.NumberOfMalePets.Should().Be(6);
            results.NumberOfPetsWith4Legs.Should().Be(5);
        }
    }
}
