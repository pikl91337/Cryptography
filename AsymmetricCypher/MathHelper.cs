using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using Microsoft.AspNetCore.Routing.Template;

namespace AsymmetricCypher
{
    /// <summary>
    /// Class for math methods
    /// </summary>
    public static class MathHelper
    {
        /// <summary>
        /// Making big prime numer
        /// </summary>
        /// <param name="N">Max number</param>
        /// <param name="rand">Random instance</param>
        /// <returns> BigInteger</returns>
        public static BigInteger RandomBigInteger(BigInteger N, Random rand)
        {
            BigInteger result = 0;
            do
            {
                int length = (int)Math.Ceiling(BigInteger.Log(N, 2));
                int numBytes = (int)Math.Ceiling(length / 8.0);
                byte[] data = new byte[numBytes];
                rand.NextBytes(data);
                result = new BigInteger(data);
            } while (result >= N || result <= 0);
            return result;
        }

        /// <summary>
        /// Pollard Rho Algorithm. factorization of N number.
        /// </summary>
        /// <param name="N">Number N</param>
        /// <returns>Number p. The multiplier of N</returns>
        public static BigInteger PollardRhoAlgorithm(BigInteger N)
        {
            // xi xj
            var xPair = new List<BigInteger>();

            // The first iteration
            xPair.Add(2);
            xPair.Add(3);

            // The multiplier of N
            BigInteger p = 0;
            int j = 1;
            while (EvklidAlgorithm(N, xPair[0]) <= 1 && p == 0)
            {
                // making a room for next pair
                xPair.RemoveAt(0);

                // current value of sequence member xi+1 = (xi^2 - 1)mod N 
                BigInteger sequence = xPair[0];

                // A loop due to wikipedia's formula (advanced algorithm)
                // Calculating GCD only for j =2^k, where k=0,1..3. i takes the values [2^k + 1, 2^(k+1)]
                for (int i = 0; i < j; i++)
                {
                    sequence = BigInteger.ModPow((BigInteger.Pow(sequence, 2) - 1), 1, N);
                    var nod = EvklidAlgorithm(N, BigInteger.Abs(xPair[0] - sequence));
                    if (nod > 1)
                    {
                        p = nod;
                    }
                }
                j *= 2; // the power of two
                xPair.Add(sequence);
                xPair[0] = BigInteger.Abs(xPair[0] - xPair[1]);
            }

            if (p == 0)
                p = EvklidAlgorithm(N, xPair[0]);

            return p;
        }

        /// <summary>
        /// Euklid algorithm for GCD
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second number</param>
        /// <returns>Big integer</returns>
        public static BigInteger EvklidAlgorithm(BigInteger a, BigInteger b)
        {
            BigInteger t = 0;
            while (b != 0)
            {
                t = a % b;
                a = b;
                b = t;  
            }

            return a;
        }

        /// <summary>
        /// Exponentiation by squaring. x^d mod n
        /// </summary>
        /// <param name="x"></param>
        /// <param name="d"></param>
        /// <param name="n"></param>
        /// <returns>BigInteger</returns>
        public static BigInteger FastPow(BigInteger x, BigInteger d, BigInteger n)
        {
            BigInteger y = 1;

            while (d > 0)
            {
                if (d % 2 == 1)
                {
                    y = (y * x) % n;
                }

                d = d / 2;
                x = (x * x) % n;
            }

            return y;
        }

        /// <summary>
        /// Extended Euklidean algorithm
        /// The extended Euclidean algorithm is an extension to the Euclidean algorithm, and computes,
        /// in addition to the greatest common divisor of integers a and b,
        /// also the coefficients of Bézout's identity, which are integers x and y such that. as+bt = GCD(a,b)
        /// </summary>
        /// <param name="m">First number</param>
        /// <param name="n">Second number</param>
        /// <returns>BigInteger</returns>
        public static BigInteger ExtendedEvklidAlgorithm(BigInteger m, BigInteger n)
        {
            // d=НОД(m,n)
            // d = s*m + t*n
            BigInteger d, s, t;
            var a = m;
            var b = n;

            BigInteger u1 = 1;
            BigInteger u2 = 0;
            BigInteger v1 = 0;
            BigInteger v2 = 1;

            while (b != 0)
            {
                var q = a / b;
                var r = a % b;
                a = b;
                b = r;
                r = u2;
                u2 = u1 - q * u2;
                u1 = r;
                r = v2;
                v2 = v1 - q * v2;
                v1 = r;
            }


            d = a;
            s = u1;
            t = v1;

            return s;
        }


        /// <summary>
        /// Calculating big integer number from binary number
        /// </summary>
        /// <param name="list">Binary number</param>
        /// <returns>Big integer</returns>
        public static BigInteger GetNumberFromList(List<short> list)
        {
            BigInteger toReturn = 0;
            for (int i = list.Count - 1, j = 0; i >= 0 && j < list.Count; i--, j++)
            {
                toReturn += BigInteger.Pow(2, j) * list[i];
            }

            return toReturn;
        }

        /// <summary>
        /// Calculating binary number from decimal number and putting it in list<int>
        /// </summary>
        /// <param name="number">Decimal number</param>
        /// <returns>List<int></returns>
        public static List<short> GetBinaryFromNumber(BigInteger number)
        {
            var toReturn = new List<short>();
            var bytes = number.ToByteArray();

            var idx = bytes.Length - 1;
            var base2 = new StringBuilder(bytes.Length * 8);
            var binaryString = Convert.ToString(bytes[idx], 2);

            //// Ensure leading zero exists if value is positive.
            //if (binaryString[0] != '0' && number.Sign == 1)
            //{
            //    base2.Append('0');
            //}

            base2.Append(binaryString);

            for (idx--; idx >= 0; idx--)
            {
                base2.Append(Convert.ToString(bytes[idx], 2).PadLeft(8, '0'));
            }

            var binaryNumber = base2.ToString();
            foreach (var ch in binaryNumber)
            {
                toReturn.Add( (short)Char.GetNumericValue(ch));
            }
            return toReturn;
        }

        /// <summary>
        /// Checking if number is prime by fermat's Theorem
        /// </summary>
        /// <param name="maskNumeric">Number to check</param>
        /// <param name="k">Number k which shows how many a numbers we check</param>
        /// <returns>bool value</returns>
        public static bool FermatTheorem(BigInteger T, int k, Random random)
        {
            // is prime
            var toReturn = false;
            for (int i = 0; i < k; i++)
            {
                // generate a number
                var a = MathHelper.RandomBigInteger(T, random);

                // a^(T-1) mod T
                var resultOfFastPow = MathHelper.FastPow(a, T - 1, T);

                // a^(T-1) mod T must be 1. If not so, return false.
                toReturn = resultOfFastPow == 1 ? true : false;
                if (!toReturn) return false;
            }

            return toReturn;
        }

        /// <summary>
        /// Checking if number is power of two
        /// </summary>
        /// <param name="x">Number to check</param>
        /// <returns>bool</returns>
        private static bool IsPowerOfTwo(BigInteger x)
        {
            return (x & (x - 1)) == 0;
        }
    }
}