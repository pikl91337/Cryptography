using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace AsymmetricCypher
{
    /// <summary>
    /// Class for getting public and private exponents
    /// </summary>
    public class ExponentHelper
    {
        /// <summary>
        /// Getting public exp by phi and depending on key length
        /// </summary>Euler function</param>
        /// <param name="keyLength">Key length</param>
        /// <returns>Big integer</returns>
        public BigInteger GetPublicExp(BigInteger phi, int keyLength,PrimeNumbersGenerator generator)
        {
            int eLength = keyLength / 3; // e length
            var e = MaskGenerator.GetMask(eLength);
            BigInteger eNumeric = MathHelper.GetNumberFromList(e); ;

            var isValid = false; // is number e valid?

            // Check if GCD(e, phi) equals one. If so, we found public exp
            isValid = MathHelper.EvklidAlgorithm(eNumeric, phi) == 1 ? true : false;

            //If number e in binary format has one in most fignificant bit and one in low bit and it is not valid then generate prime number
            while (!isValid)
            {
                e = generator.Generate(e);
                eNumeric = MathHelper.GetNumberFromList(e);

                // Check if GCD(e, phi) equals one. If so, we found public exp
                isValid = MathHelper.EvklidAlgorithm(eNumeric, phi) == 1 ? true : false;
            }

            return eNumeric;
        }

        /// <summary>
        /// Getting private exp
        /// </summary>
        /// <param name="e">Public exp</param>
        /// <param name="phi">Euler function</param>
        /// <returns>Big integer</returns>
        public BigInteger GetPrivateExp(BigInteger e, BigInteger phi)
        {
            BigInteger privateExp = 0;

            // Obtaining private exp by extended euklid's algorithm
            privateExp = MathHelper.ExtendedEvklidAlgorithm(e, phi);

            if (privateExp < 0)
            {
                privateExp += phi;
            }

            return privateExp;
        }
    }
}
