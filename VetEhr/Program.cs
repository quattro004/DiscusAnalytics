using System;
using EhrLib;
using Microsoft.Extensions.Configuration;

namespace VetEhr
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
                // Usage: dotnet run petData={filePath}
                // For example I ran it like this on my Ubuntu machine: 
                //        dotnet run petData=/home/reese/src/DiscusAnalytics/EhrLibTests/Data/PetData.csv
                var builder = new ConfigurationBuilder();
                builder.AddCommandLine(args);
                var config = builder.Build();
                var petLoader = new PetLoader();
                var pets = petLoader.Load(config["petData"]);
                if (petLoader.Save(pets))
                {
                    Console.WriteLine("Loaded the pet data to the repository at {0}", petLoader.PetRepoPath);
                }
                else
                {
                    Console.WriteLine("The pet data could not be saved to the repository.");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
    }
}
