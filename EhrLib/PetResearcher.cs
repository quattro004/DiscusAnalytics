using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace EhrLib
{
    public class PetResearcher
    {
        public PetAnalysisResult Analyze(string petRepoPath)
        {
            if (string.IsNullOrWhiteSpace(petRepoPath))
            {
                throw new ArgumentNullException(nameof(petRepoPath));
            }
            if (!File.Exists(petRepoPath))
            {
                throw new ArgumentException(string.Format("The pet repo at {0} was not found!", petRepoPath));
            }
            try
            {
                // I ran into problems with encryption on Ubuntu, File.Encrypt wasn't supported and the existing code
                // is getting a invalid padding exception.
                // using (Aes myAes = Aes.Create())
                // {
                //     var decryptedData = DecryptStringFromBytes_Aes(File.ReadAllBytes(petRepoPath), myAes.Key, myAes.IV);
                    var petData = JsonConvert.DeserializeObject<IEnumerable<Pet>>(File.ReadAllText(petRepoPath),
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });
                    var numberOfMalePets = petData.Count(p => p.Gender.ToLower() == "m");
                    var numberOfPetsWith4Legs = petData.Count(p => p.NumberOfLegs == 4);
                    var averageAgeOfDogs = petData.Where(p => p is Dog).Average(p => p.Age);
                    var reptileProblems = from pet in petData
                                          where pet is Reptile
                                          select pet.Problem;
                    var mammalNames = from pet in petData
                                      where pet is Mammal
                                      select pet.Name;
                    return new PetAnalysisResult(numberOfMalePets, numberOfPetsWith4Legs, averageAgeOfDogs, reptileProblems, mammalNames);
                // }
            }
            catch (Exception exc)
            {
                // TODO: after adding logging log the error
                Debug.WriteLine(exc.ToString());
            }
            
            return default(PetAnalysisResult);
        }

// I got the encrypt/decrypt code from this example: 
// https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes.-ctor?view=netcore-3.0
// At first I tried using File.Decrypt but it wasn't supported on Linux.
//
        private static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }
            return plaintext;
        }
    }
}