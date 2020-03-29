using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StreamCypher
{
    /// <summary>
    /// A class for generating M-sequence
    /// </summary>
    public class KeyWorker
    {
        private Random _Random;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="random"></param>
        public KeyWorker( Random random)
        {
            _Random = random;
        }

        /// <summary>
        /// Сгенерировать регистр
        /// Generating a registr
        /// </summary>
        /// <param name="length">Registr length</param>
        /// <param name="sw">stream writer</param>
        /// <returns>list int</returns>
        public List<short> GenerateRegistr(int length, StreamWriter sw)
        {
            var toReturn = new List<short>();

            while (toReturn.Count != length)
            {
                var value = _Random.Next(0, 2);
                toReturn.Add((short)value);
                sw.Write(value);
            }

            return toReturn;
        }

        /// <summary>
        /// Generating m-sequence of given length with given registr and polynomial numbers
        /// </summary>
        /// <param name="registr"></param>
        /// <param name="M"></param>
        /// <param name="polynomFirstTerm"></param>
        /// <param name="polynomSecondTerm"></param>
        public List<short> GenerateKey(List<short> registr, int M, int polynomFirstTerm, int polynomSecondTerm, StreamWriter sw)
        {
            var regLength = registr.Count;
            if (Math.Pow(2,regLength) <= M)
            {
                throw new Exception("Registr length is not enough to provide necessary key length (equals to message length)");
            }
            var toReturn = new List<short>();

            // idling
            registr = Idle(registr, polynomFirstTerm, polynomSecondTerm);

            for (int i = 0; i < M; i++)
            {
                int y = registr[polynomFirstTerm] ^ registr[polynomSecondTerm];
                int x1 = registr[0];
                for (int j = 0; j < registr.Count - 1;j++)
                {
                    registr[j] = registr[j+1];
                }

                registr[registr.Count - 1] = (short)y;

                toReturn.Add((short)x1);
                sw.Write(x1);
                
            }

            return toReturn;
        }

        /// <summary>
        /// Idling for registr
        /// </summary>
        private List<short> Idle(List<short> registr, int polynomFirstTerm, int polynomSecondTerm)
        {
            // idling
            for (int n = 0; n < 2; n++)
            {
                for (int i = 0; i < registr.Count; i++)
                {
                    int y = registr[polynomFirstTerm] ^ registr[polynomSecondTerm];
                    int x1 = registr[0];
                    for (int j = 0; j < registr.Count - 1; j++)
                    {
                        registr[j] = registr[j + 1];
                    }

                    registr[registr.Count - 1] = (short)y;
                }
            }

            return registr;
        }
    }
}
