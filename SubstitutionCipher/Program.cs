using System;
using System.Collections.Generic;
using System.IO;
using Common;

namespace SubstitutionCipher
{
    public class Program
    {
        static void Main(string[] args)
        {
            var willGenerate = ConsoleMessager.WillGenerateAlphabet();

            // zeroize key to generate new one
            File.WriteAllText("key.txt", "");

            // deleting old files
            File.Delete("cryptedtext.txt");
            File.Delete("decryptedtext.txt");
            Console.WriteLine("Старые файлы удалены!");
            Console.WriteLine();

            // generating a key
            var forKeyGenerating = new StreamWriter("key.txt", true);
            KeyWorker.GenerateKey(forKeyGenerating);
            forKeyGenerating.Close();

            // Reading a key from file
            // A dictionary where key is a letter from russian alphabet, value - a letter from new alphabet
            var forKeyReading = new StreamReader("key.txt");
            Dictionary<char, char> alphabet = KeyWorker.GetKey(forKeyReading);
            forKeyReading.Close();

            var encoder = new Encoder();
            var blockLength = ConsoleMessager.ReadBlockLength();

            // encrypting
            Func<char[], Dictionary<char, char>, char[]> crypt = new Func<char[], Dictionary<char, char>, char[]>(encoder.Crypt);
            CryptOrDecrypt(crypt, blockLength, alphabet, "text.txt", "Encryption", "cryptedtext.txt");

            // decrypting
            Func<char[], Dictionary<char, char>, char[]> decrypt = new Func<char[], Dictionary<char, char>, char[]>(encoder.DeCrypt);
            CryptOrDecrypt(decrypt, blockLength, alphabet, "cryptedtext.txt", "Decryption", "decryptedtext.txt");

            // calculating entropy
            var entropyWorker = new EntropyWorker();
            var textReader = new StreamReader("шифрограмма.txt");
            var text = textReader.ReadToEnd();
            var entropy = entropyWorker.CalculateEntropy(text);

            Console.WriteLine();
            Console.WriteLine("Entropy value: " +entropy);
        }

        /// <summary>
        /// Calling either encryption or decryption depending on action in input parameters
        /// </summary>
        /// <param name="func">Method to call</param>
        /// <param name="blockLength">Block length</param>
        /// <param name="alphabet">Key</param>
        /// <param name="fromFile">file from where a text will be read</param>
        /// <param name="whereFile">File where en(-de)crypted text will be saved</param>
        private static void CryptOrDecrypt(Func<char[],Dictionary<char,char>,char[]> func, int blockLength, Dictionary<char, char> alphabet, string fromFile,string actionName, string whereFile)
        {
            using (StreamReader sr = new StreamReader(fromFile))
            {
                while (!sr.EndOfStream)
                {
                    var buf = new char[blockLength];
                    var n = sr.ReadBlock(buf, 0, blockLength);

                    var result = new char[n];

                    Array.Copy(buf, result, n);

                    var transformedText = func(result, alphabet);

                    using (StreamWriter sr1 = new StreamWriter(whereFile, true))
                    {
                        sr1.Write(new string(transformedText));
                    }

                }
                Console.WriteLine(String.Format("{0} is done. Check {1}",actionName,whereFile));
                Console.WriteLine();
            }
        }
    }
}
