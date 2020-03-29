using System;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;

namespace AsymmetricCypher
{
    /// <summary>
    /// Prime numbers generator
    /// </summary>
    public class PrimeNumbersGenerator
    {
        /// <summary>
        /// Random
        /// </summary>
        private Random _Random;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public PrimeNumbersGenerator()
        {
            _Random = new Random(); 
        }

        /// <summary>
        /// Генерирует простое число, представленное в двоичном виде внутри списка (0й элемент - 0й бит)
        /// Generating prime binary number placed in int list
        /// </summary>
        /// <param name="mask">a mask, where prime number will be generated</param>
        /// <returns>list int</returns>
        public List<short> Generate(List<short> mask)
        {
            var isPrime = false;

            // while number is not prime - generate new number
            while (!isPrime)
            {
                // not touching first and last bits
                for (int i = 1; i < mask.Count - 1; i++)
                {
                    mask[i] = (short)_Random.Next(0, 2);
                }
                isPrime = CheckOnPrimary(mask);
            }
            return mask;
        }
        
        /// <summary>
        /// Generate composite number placed in list int (0 element - lowest bit)
        /// </summary>
        /// <param name="mask">a mask where generated number will be placed</param>
        /// <returns>list int</returns>
        public List<short> GenerateNotPrime(List<short> mask)
        {
            var isPrime = true;

            // while number is prime - generate new number
            while (isPrime)
            {
                // not touching first and last bits
                for (int i = 1; i < mask.Count - 1; i++)
                {
                    mask[i] = (short)_Random.Next(0, 2);
                }
                isPrime = CheckOnPrimary(mask);
            }
            return mask;
        }

        /// <summary>
        /// Checking if binary number is prime.
        /// </summary>
        /// <param name="mask">Number to check</param>
        /// <returns>bool</returns>
        private bool CheckOnPrimary(List<short> mask)
        {
            var toReturn = false;

            // getting decimal from binary
            BigInteger maskNumeric = MathHelper.GetNumberFromList(mask);

            // checking if its primary
            var k = 10; 
            toReturn = MathHelper.FermatTheorem(maskNumeric, k,_Random);

            return toReturn;
        }
    }
}