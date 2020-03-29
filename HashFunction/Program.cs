using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Common;

namespace HashFunction
{
    class Program
    {
        static void Main(string[] args)
        {
            // getting a string to calculate a hash function
            var inputString = ConsoleMessager.AskString();

            Console.WriteLine("Task 1.");
            // Task 1. Getting hash from initial string value
            var mdFourInstance = GetHashFromString(inputString);

            // printing results to console
            ConsoleMessager.DemonstrateHashResult(inputString, mdFourInstance.ResultHashString);

            // Task 2. "The avalanche effect"
            Console.WriteLine("Task 2.");
            var inputStringWithOneBitChanged = ChangeOneBitInString(inputString);
            var mdFourInstanceWithOneBitChanged = GetHashFromString(inputStringWithOneBitChanged);

            // printing results to console
            ConsoleMessager.DemonstrateHashResult(inputStringWithOneBitChanged, mdFourInstanceWithOneBitChanged.ResultHashString);

            var numberOfDifferentBits = CalculateDifferentBits(mdFourInstance.ResultHashBytes, mdFourInstanceWithOneBitChanged.ResultHashBytes);

            // printing results to console
            ConsoleMessager.ShowDifferentBits(inputString, inputStringWithOneBitChanged, numberOfDifferentBits);

            // Task 3. finding a collision.
            Console.WriteLine("Task 3.");
            FindAndPrintCollision();

            // Task 4. Finding prototype
            Console.WriteLine("Task 4.");
            FindAndPrintPrototype();
        }

        /// <summary>
        /// Task 4. Prototype
        /// </summary>
        private static void FindAndPrintPrototype()
        {
            Console.WriteLine("Enter a password:");

            var inputPassword = ConsoleMessager.AskString();

            // asking user how many bits (counting from the beginning of initial message) he wants to be the same
            var k = ConsoleMessager.AskHashLength();

            // asking length for future string value generating
            var L = ConsoleMessager.AskStringLength();

            // initial hash
            var mdFourInitialInstance = GetHashFromString(inputPassword);


            var firstBitsList = GetBitsList(mdFourInitialInstance.ResultHashBytes, k);
            var firstBitsString = Helper.PrintList(firstBitsList);

            var prototypeString = "";

            int N = 0;
            while (prototypeString == "")
            {
                N++;
                var randomString = GetRandomString(L);
                var mdFourRandomStringInstance = GetHashFromString(randomString);
                var randomHashFirstBitsList = GetBitsList(mdFourRandomStringInstance.ResultHashBytes, k);
                var randomHashFirstBitsString = Helper.PrintList(randomHashFirstBitsList);

                if (randomHashFirstBitsString == firstBitsString)
                {
                    Console.WriteLine(String.Format("We found a string with the same {0} first bits in hash for your password '{1}'",k,inputPassword));
                    prototypeString = randomString;
                    ConsoleMessager.PrintCollisionResults(mdFourInitialInstance.ResultHashString, inputPassword,
                        mdFourRandomStringInstance.ResultHashString, randomString, randomHashFirstBitsString, N);
                }
            }


        }

        /// <summary>
        /// Task 3. Collision
        /// </summary>
        private static void FindAndPrintCollision()
        {
            // asking user how many bits (counting from the beginning of initial message) he wants to be the same
            var k = ConsoleMessager.AskHashLength();

            // asking length for future string value generating
            var L = ConsoleMessager.AskStringLength();

            // a dictionary where key is a bits piece and value is a random generated string
            var kBitsStringHashDict = new Dictionary<string,string>();

            while (true)
            {
                var randString = GetRandomString(L); 
                var mdFourInstance = GetHashFromString(randString);
                var firstBitsList = GetBitsList(mdFourInstance.ResultHashBytes,k);

                var firstBitsString = Helper.PrintList(firstBitsList);

                // when we try to add the same key an exception is going to be raised.
                // that means a collision has been found.
                try
                {
                    kBitsStringHashDict.Add(firstBitsString, randString);
                }
                catch (ArgumentException e)
                {
                    var firstHash = GetHashFromString(kBitsStringHashDict[firstBitsString]).ResultHashString;
                    var secondHash = mdFourInstance.ResultHashString;

                    var firstRandomString = kBitsStringHashDict[firstBitsString];
                    var secondRandomString = randString;
                    var N = kBitsStringHashDict.Count;

                    ConsoleMessager.PrintCollisionResults(firstHash, firstRandomString, secondHash, secondRandomString,
                        firstBitsString,N);
                    break;
                }
            }

        }

        /// <summary>
        /// Get first L bits from a byte list
        /// </summary>
        /// <param name="resultHashBytes">byte list</param>
        /// <param name="L">how many bits we need</param>
        /// <returns>list of ints</returns>
        private static List<short> GetBitsList(List<byte> resultHashBytes, int k)
        {
            var bitsList = new List<short>();
            for (int i = 0; i < 8; i++)
            {
                var bits = new BitArray(new byte[] { resultHashBytes[i] });
                foreach (var oneBit in bits)
                {
                    bitsList.Add((bool)oneBit ? (short)1 : (short)0);
                }
            }

            return bitsList.GetRange(0, k);
        }

        /// <summary>
        /// Getting random string
        /// </summary>
        /// <param name="L">string length</param>
        /// <returns>string value</returns>
        private static string GetRandomString(int L)
        {
            // generator for randomizing
            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[L];

            // random bytes
            generator.GetBytes(bytes);
            for (int i = 0; i < bytes.Length; i++)
            {
                // that is because "normal ascii symbols are only placed in range (32, 128)"
                bytes[i] = (byte)((bytes[i] % 96) + 32);
            }

            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Searching of different bits in two given byte lists.
        /// </summary>
        /// <param name="resultHash">first byte list</param>
        /// <param name="resultHashWithOneBitChanged">second byte list</param>
        /// <returns>total amount of different bits</returns>
        private static int CalculateDifferentBits(List<byte> resultHash, List<byte> resultHashWithOneBitChanged)
        {
            int totalDifferentBits = 0;
            var moduloTwo = new List<byte>();

            for (int i = 0; i < resultHash.Count; i++)
            {
                moduloTwo.Add((byte)(resultHash[i] ^ resultHashWithOneBitChanged[i]));
            }

            foreach (var oneByte in moduloTwo)
            {
                // total amount of trues in bits list shows total amount of different bits
                var bits = new BitArray(new byte[] { oneByte });
                foreach (var oneBit in bits)
                {
                    if ((bool)oneBit) totalDifferentBits++;
                }
            }
            return totalDifferentBits;
        }

        /// <summary>
        /// Changing one bit in given string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns>string with one bite change</returns>
        private static string ChangeOneBitInString(string inputString)
        {
            Console.WriteLine("Now lets change one bit in message to observe 'The avalanche effect'.");
            var inputBytes = Helper.GetBytesFromString(inputString);
            var firstByteDecimal = (int)inputBytes[0];
            var firstByteBinary = Helper.GetBinaryFromNumber(firstByteDecimal);

            var oldValue = firstByteBinary[0];
            firstByteBinary[0] = firstByteBinary[0] == 1 ? (short) 0 : (short) 1;

            Console.WriteLine(String.Format("We changed most significant bit in the first byte of initial message from {0} to {1}.", oldValue, firstByteBinary[0]));

            var firstByteChanged = Helper.GetNumberFromList(firstByteBinary);
            inputBytes[0] = (byte) firstByteChanged;

            var changedInputString = Helper.GetStringFromBytes(inputBytes);

            return changedInputString;
        }

        /// <summary>
        /// Task 1. Get and print to console hash function from a random string entered by the user
        /// </summary>
        private static MdFourAlgorithm GetHashFromString(string inputString)
        {
            var mdFourHelper = new MdFourAlgorithm();

            // calculating hash from a string value
            mdFourHelper.Execute(inputString);

            return mdFourHelper;
        }
    }
}
