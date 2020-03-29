using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Common;

namespace AsymmetricCypher
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Ask user key length
            var keyLength = ConsoleMessager.AskKeyLength();

            Console.WriteLine("Now input number (number p will be increased on  this number, q will be decreased on this number):");
            var keyChange = (int)ConsoleMessager.AskRate();

            var keysGenerationTime = new Stopwatch();
            keysGenerationTime.Start();

            // generating of public, private key and cypher module
            var keys = GenerateKeys(keyLength, keyChange);
            keysGenerationTime.Stop();
            Console.WriteLine(String.Format("Key generation time: {0} sec", (double)keysGenerationTime.ElapsedMilliseconds / 1000));
            Console.WriteLine("-------------------------------------------------------------");

            // ask file name which will be encrypted
            var fileName = ConsoleMessager.AskFilename();

            // Rsa encrypting
            RsaCrypt(keys, fileName, keyLength);

            //var a = BigInteger.Parse(Console.ReadLine());
            //RsaBreaking(a);

            // trying to break cypher module
            RsaBreaking(keys.cypherModule);

        }

        /// <summary>
        /// Task 4,5. Breaking of rsa key. Complexity ~ factorization of N.
        /// Finding one of the N's multiplier
        /// </summary>
        /// <param name="cypherModule">N number</param>
        private static void RsaBreaking(BigInteger cypherModule)
        {
            Console.WriteLine(String.Format("Timer has started. Trying to break key (factorization of N = {0})", cypherModule));
            var watch = new Stopwatch();
            watch.Start();

            var p = MathHelper.PollardRhoAlgorithm(cypherModule);

            watch.Stop();
            var time = watch.ElapsedMilliseconds;
            Console.WriteLine(String.Format("A key is broken!One of multipliers = {0}. Execution time: {1} sec", p, (double) time / 1000));
            Console.WriteLine("-------------------------------------------------------------");
        }

        /// <summary>
        /// Task 1,5. Generating of public and private keys.
        /// </summary>
        /// <param name="keyLength">Key length</param>
        /// <param name="keyChange">For p and q length cnanging</param>
        /// <returns>KeysHolder</returns>
        private static KeysHolder GenerateKeys(int keyLength, int keyChange)
        {
            var generator = new PrimeNumbersGenerator();

            // generating of prime numbers
            var pNumeric = GeneratePrimeNumber(keyLength - keyChange, generator);
            var qNumeric = GeneratePrimeNumber(keyLength + keyChange, generator);
            Console.WriteLine(String.Format("Prime numbers p = {0} and q = {1} are generated.", pNumeric, qNumeric));

            //// generating of composite numbers (on purpose)
            //var pNumeric = GenerateNotPrimeNumber(keyLength, generator);
            //var qNumeric = GenerateNotPrimeNumber(keyLength, generator);
            //Console.WriteLine(String.Format("Составные числа p = {0} и q = {1} сгенерированы.", pNumeric, qNumeric));


            // Calculating public exp, private exp and cypher module
            var keysToReturn = CalculateKeysValues(keyLength, pNumeric, qNumeric, generator);

            FileWorker.CreateNEWFile(keysToReturn.publicKey,"public.txt");
            FileWorker.CreateNEWFile(keysToReturn.privateKey,"private.txt");
            FileWorker.CreateNEWFile(keysToReturn.cypherModule,"cyphermodule.txt");

            Console.WriteLine("The keys are generated and placed in public.txt, private.txt, cyphermodule.txt");

            return keysToReturn;
        }

        /// <summary>
        /// Calculating public exp, private exp and cypher module
        /// </summary>
        /// <param name="keyLength">Key length</param>
        /// <param name="pNumeric">First decimal prime number</param>
        /// <param name="qNumeric">Second decimal prime number</param>
        /// <param name="generator">prime numbers generator</param>
        /// <returns>Возвращает структуру, хранящую ключи и модуль</returns>
        private static KeysHolder CalculateKeysValues(int keyLength, BigInteger pNumeric, BigInteger qNumeric, PrimeNumbersGenerator generator)
        {
            var keysToReturn = new KeysHolder();

            // Cypher module
            var N = pNumeric * qNumeric;
            keysToReturn.cypherModule = N;

            // Euler function
            var phi = (pNumeric - 1) * (qNumeric - 1);

            var exponentHelper = new ExponentHelper();

            // public exp
            var e = exponentHelper.GetPublicExp(phi, keyLength, generator);
            keysToReturn.publicKey = e;

            // private exp
            var d = exponentHelper.GetPrivateExp(e, phi);
            keysToReturn.privateKey = d;

            return keysToReturn;
        }

        /// <summary>
        /// Generating decimal prime number
        /// </summary>
        /// <param name="keyLength">Key length </param>
        /// <returns>Big integer</returns>
        private static BigInteger GeneratePrimeNumber(int keyLength,PrimeNumbersGenerator generator)
        {
            List<short> mask = MaskGenerator.GetMask(keyLength / 2);

            // generating prime number
            var primeNumber = generator.Generate(mask);

            return MathHelper.GetNumberFromList(primeNumber);
        }
        
        /// <summary>
        /// ПУНКТ 3. Генерация составного числа (проверка работоспособности шифра RSA с составными числами вместо простых
        /// </summary>
        /// <param name="keyLength">Длина ключа</param>
        /// <returns>Возвращает большое целое число</returns>
        private static BigInteger GenerateNotPrimeNumber(int keyLength,PrimeNumbersGenerator generator)
        {
            // Подготаваливаем затравку для последующей генерации в ней простого числа. 
            List<short> zatravkaMaska = MaskGenerator.GetMask(keyLength / 2);

            // простое число
            var primeNumber = generator.GenerateNotPrime(zatravkaMaska);

            return MathHelper.GetNumberFromList(primeNumber);
        }

        /// <summary>
        /// Task 2. RSA Encryption and decryption
        /// </summary>
        /// <param name="keys">keys</param>
        /// <param name="fileName"></param>
        /// <param name="keyLength">Key length</param>
        private static void RsaCrypt(KeysHolder keys, string fileName,int keyLength)
        {

            var plainText = FileWorker.GetBinaryFile(fileName);
            var encodedText = new List<short>();
            var decryptedText = new List<short>();
            var rsaEncoder = new RsaEncoder();

            // encrypting
            encodedText = Encode(keys.publicKey,keys.cypherModule,plainText, rsaEncoder, keyLength / 4, keyLength,false);
            // saving encrypted message
            FileWorker.CreateTextFileForBinaryNumber(encodedText, "crypted.txt");
            Console.WriteLine(String.Format("Encryption has finished. Check crypted.txt"));

            // decrypting
            decryptedText = Encode(keys.privateKey,keys.cypherModule,encodedText, rsaEncoder, keyLength, keyLength / 4,true);
            // saving decrypted message
            FileWorker.GetNEWFile(decryptedText,"decrypted_" + fileName);
            Console.WriteLine(String.Format("DEcryption has finished. Check decrypted{0}", fileName));
            Console.WriteLine("-------------------------------------------------------------");

        }

        /// <summary>
        /// RSA Encryption and decryption. Initial message will be separated on blocks with length toTakeLength
        /// Encrypted (decrypted message) are written as blocks with length toPutLength
        /// </summary>
        /// <param name="key">key (either public or private)</param>
        /// <param name="cypherModule">cypher module</param>
        /// <param name="text">message</param>
        /// <param name="rsaEncoder">RSA instance</param>
        /// <param name="toTakeLength"></param>
        /// <param name="toPutLength"></param>
        /// <returns>binary number placed in list int</returns>
        private static List<short> Encode(BigInteger key, BigInteger cypherModule, List<short> text, RsaEncoder rsaEncoder, int toTakeLength, int toPutLength, bool isDecryption)
        {
            var toReturn = new List<short>();
            while (text.Count != 0)
            {
                // separate message on blocks
                var oneBlock = text.Take(toTakeLength);
                var oneBlockNumeric = MathHelper.GetNumberFromList(oneBlock.ToList());

                // encrypt(decrypt) one block
                var encodedNumber = rsaEncoder.Encode(oneBlockNumeric, key, cypherModule);
                var encodedBlockBinary = MathHelper.GetBinaryFromNumber(encodedNumber);
                if (encodedBlockBinary[0] == 0)
                {
                    encodedBlockBinary.RemoveAt(0); // delete first bit responsible for number sign
                }

                // in case decryption there is no need to fill last block with insignificant zeros
                if (!(isDecryption && text.Count == toTakeLength))
                {
                    encodedBlockBinary = Helper.BringingToNeededBinaryFormat(encodedBlockBinary, toPutLength);
                }

                toReturn.AddRange(encodedBlockBinary);

                // remove block which has already been en(-de)crypted
                text.RemoveRange(0, oneBlock.Count());
            }

            return toReturn;
        }

    }
}
