using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common;

namespace StreamCypher
{
    public class Program
    {
        static void Main(string[] args)
        {
            DeleteOldFiles();

            var fileName = ConsoleMessager.AskFilename();

            // converting a file to binary sequence
            var binaryFile = FileWorker.GetBinaryFile(fileName);

            // generating a key
            var binaryKey = GenerateKey(binaryFile.Count);

            // ask series length
            var k = ConsoleMessager.AskBlockLength(binaryFile.Count);

            // frequency test for key
            binaryKey = FrequencyTest(binaryKey,k,"key");

            // frequency test for file before any manipulation
            FrequencyTest(binaryFile,k, "file before encryption");

            // a number to divide key length on it
            var number = ConsoleMessager.AskKeyDivideOnNumber();

            // vername manipulations
            var cryptedFile = CypherLogic(binaryKey.GetRange(0,binaryKey.Count / number), binaryFile, fileName);

            // Frequency test for encrypted file
            FrequencyTest(cryptedFile,k, "file after encryption");

            // Corelation tests
            CorelationTest(binaryKey,"key");
            CorelationTest(binaryFile,"file before encryption");
            CorelationTest(cryptedFile,"file after encryption");
            
            
        }

        /// <summary>
        /// Task 1. Key generation
        /// </summary>
        /// <param name="binaryFileLength">length of file in binary format</param>
        /// <returns>list int</returns>
        private static List<short> GenerateKey(int binaryFileLength)
        {
            var polinomNumbers = ConsoleMessager.AskPolinomNumbers();

            var keyWorker = new KeyWorker(new Random());

            // generating registr and save his initial state to file
            var forRegistr = new StreamWriter("registr.txt", true);
            var registr = keyWorker.GenerateRegistr(31, forRegistr);
            forRegistr.Close();
            Console.WriteLine();
            Console.WriteLine("Registr has been randomized and now placed in registr.txt");

            // generating a key (m-sequence)
            var forKeyGenerating = new StreamWriter("key.txt", true);
            var binaryKey = keyWorker.GenerateKey(registr, binaryFileLength, polinomNumbers.ElementAt(1), polinomNumbers.ElementAt(0), forKeyGenerating);
            forKeyGenerating.Close();
            Console.WriteLine();
            Console.WriteLine("A key is generated and now placed in key.txt");

            return binaryKey;
        }

        /// <summary>
        /// Task 4. Vername encryption
        /// </summary>
        /// <param name="binaryKey">Binary key</param>
        /// <param name="binaryFile">Binary file</param>
        /// <returns>retirns encrypted message</returns>
        private static List<short> CypherLogic(List<short> binaryKey, List<short> binaryFile, string filename)
        {
            var encoder = new VernameEncoder();
            var crypted = new List<short>();
            var decrypted = new List<short>();

            var sw_crypted = new StreamWriter("crypted.txt",true);
            // encryption
            for (int i = 0; i < binaryFile.Count; i++)
            {
                var cryptedCharacter = encoder.Execute(binaryKey[i%binaryKey.Count], binaryFile[i]);
                crypted.Add((short)cryptedCharacter);
                sw_crypted.Write(cryptedCharacter);
            }
            sw_crypted.Close();
            Console.WriteLine();
            Console.WriteLine("Encryption is done. Encrypted message saved to crypted.txt");

            // decryption
            for (int i = 0; i < crypted.Count; i++)
            {
                var decryptedCharacter = encoder.Execute(binaryKey[i%binaryKey.Count], crypted[i]);
                decrypted.Add((short)decryptedCharacter);
            }

            FileWorker.GetNEWFile(decrypted,"decrypted"+filename);
            Console.WriteLine();
            Console.WriteLine("Decryption is done. Decrypted file is decrypted"+filename);
            Console.WriteLine("-------------------------------------------------------------");

            return crypted;
        }

        /// <summary>
        /// Deletion of old files
        /// </summary>
        private static void DeleteOldFiles()
        {
            File.Delete("key.txt");
            File.Delete("registr.txt");
            File.Delete("crypted.txt");
        }

        /// <summary>
        /// ПУНКТ 2. Сериальный тест
        /// Task 2. frequency test
        /// </summary>
        /// <param name="binaryKey"></param>
        /// <returns>Returns either empty list or key</returns>
        private static List<short> FrequencyTest(List<short> binaryText,int k,string testedName)
        {
            var freqTest = new FrequencyTester(k, binaryText.Count);

            // testing sequence and getting khi value
            var khi = freqTest.Test(binaryText);

            var teorFreq = freqTest._TeorFrequency;
            var practFrequencies = freqTest._PossibleBlocks;

            // represent to user results of executed test
            var isKeyOkay = ConsoleMessager.CheckKhiValue(khi, k, teorFreq, practFrequencies, testedName);

            // TRYING TO GET VALID KEY
            if (testedName == "ключа")
            {
                if (!isKeyOkay)
                {
                    // generating a good key
                    var newKey = ReGenerateKey(isKeyOkay,binaryText.Count,k,testedName);
                    return newKey;
                }
                else
                {
                    return binaryText;
                }
            }

            return new List<short>();
        }

        /// <summary>
        /// Regeneration a key if it has not passed frequency test
        /// </summary>
        /// <param name="isKeyOkay"></param>
        /// <param name="binaryTextLength"></param>
        /// <param name="k"></param>
        /// <param name="testedName"></param>
        /// <returns></returns>
        private static List<short> ReGenerateKey(bool isKeyOkay,int binaryTextLength, int k, string testedName)
        {
            var newKey = new List<short>();
            while (!isKeyOkay)
            {
                Console.WriteLine("Regenerating key because it hasn't passed frequency test.");
                newKey = GenerateKey(binaryTextLength);
                var freqTest = new FrequencyTester(k, binaryTextLength);
                var khi = freqTest.Test(newKey);

                var teorFreq = freqTest._TeorFrequency;
                var practFrequencies = freqTest._PossibleBlocks;

                // represent to user results of executed test
                isKeyOkay = ConsoleMessager.CheckKhiValue(khi, k, teorFreq, practFrequencies, testedName);
            }

            return newKey;
        }

        /// <summary>
        /// Task 3. Corelation test
        /// </summary>
        /// <param name="binaryKey"></param>
        private static void CorelationTest(List<short> binaryKey, string testedName)
        {
            var corTest = new CorelationTester(binaryKey, 3);
            corTest.Test();
            ConsoleMessager.ShowCorelationTestResults(corTest._Rk,corTest._Rkr, testedName);
        }
    }
}
