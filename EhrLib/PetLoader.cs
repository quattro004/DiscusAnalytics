using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace EhrLib
{
    public class PetLoader
    {
        public PetLoader()
        {
            var petRepoDir = Path.GetDirectoryName(PetRepoPath);
            if (!Directory.Exists(petRepoDir))
            {
                Directory.CreateDirectory(petRepoDir);
            }
        } 

        public string PetRepoPath => Path.Combine(AppContext.BaseDirectory, "PetRepo/Pets.json");

        // See https://joshclose.github.io/CsvHelper/examples/reading/ for examples on reading with the CsvHelper library
        //
        public IEnumerable<Pet> Load(string dataPath)
        {
            if (string.IsNullOrWhiteSpace(dataPath))
            {
                throw new ArgumentNullException(nameof(dataPath));
            }
            if (!File.Exists(dataPath))
            {
                throw new ArgumentException("The file located at {0} doesn't exist", dataPath);            
            }
            var pets = new List<Pet>();
            using (var reader = new StreamReader(dataPath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<PetMap>();
                csv.Read();
                csv.ReadHeader();
                
                while (csv.Read())
                {
                    var petType = csv.GetField(1).ToLower();
                    switch (petType)
                    {
                        case "dog":
                            pets.Add(csv.GetRecord<Dog>());
                            break;

                        case "cat":
                            pets.Add(csv.GetRecord<Mammal>());
                            break;

                        case "parrot":
                        case "cockatiel":
                            pets.Add(csv.GetRecord<Bird>());
                            break;

                        case "lizard":
                            pets.Add(csv.GetRecord<Reptile>());
                            break;

                        case "snake":
                            pets.Add(csv.GetRecord<Snake>());
                            break;

                        default:
                            throw new InvalidOperationException(string.Format("Unknown pet type {0}.", petType));
                    }
                }
            }
            return pets;
        }
        
        public bool Save(IEnumerable<Pet> pets)
        {
            if (null == pets)
            {
                throw new ArgumentNullException(nameof(pets));
            }
            try 
            {
                using (Aes myAes = Aes.Create())
                {
                    var petData = JsonConvert.SerializeObject(pets, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });
                    // byte[] encrypted = EncryptStringToBytes_Aes(petData, myAes.Key, myAes.IV);
                    // File.WriteAllBytes(PetRepoPath, encrypted);
                    File.WriteAllText(PetRepoPath, petData);
                    return true;
                }
            }
            catch (Exception exc)
            {
                // TODO: after adding logging log the error
                Debug.WriteLine(exc.ToString());
                return false;
            }
        }

// I got the encrypt/decrypt code from this example: 
// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes.-ctor?view=netcore-3.0
// At first I tried using File.Encrypt but it wasn't supported on Linux.
//
        private static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            
            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                            swEncrypt.Flush();
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }
    }
}