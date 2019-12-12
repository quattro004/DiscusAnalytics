using System;
using EhrLib;
using Microsoft.Extensions.Configuration;


namespace VetEhrResearch
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Used the following as a simple example to parse the command line:
                // https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.commandlineconfigurationextensions.addcommandline?view=dotnet-plat-ext-3.0#Microsoft_Extensions_Configuration_CommandLineConfigurationExtensions_AddCommandLine_Microsoft_Extensions_Configuration_IConfigurationBuilder_System_String___
                //
                // Usage: dotnet run petRepo={filePath}
                // For example I ran it like this on my Ubuntu machine: 
                //        dotnet run petRepo=/home/reese/src/DiscusAnalytics/VetEhr/bin/Debug/netcoreapp3.0/PetRepo/Pets.json
                var builder = new ConfigurationBuilder();
                builder.AddCommandLine(args);
                var config = builder.Build();
                var petResearcher = new PetResearcher();
                var results = petResearcher.Analyze(config["petRepo"]);
                
                Console.WriteLine("There are {0} pets that are male.", results.NumberOfMalePets);
                Console.WriteLine("There are {0} pets that have 4 legs.", results.NumberOfPetsWith4Legs);
                Console.WriteLine("The average age of dogs is {0}.", results.AverageAgeOfDogs);
                Console.WriteLine("All the repile problems are:\r\n{0}", results.ReptileProblems);
                Console.WriteLine("The names of all mammal pets are:\r\n{0}", results.NumberOfMalePets);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
