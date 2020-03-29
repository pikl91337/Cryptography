using System;
using System.Collections.Generic;
using System.Text;

namespace StreamCypher
{
    /// <summary>
    /// vername encoder
    /// </summary>
    public class VernameEncoder
    {
        /// <summary>
        /// vername En(-de)cryption
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="text">message</param>
        /// <returns></returns>
        public int Execute(int key, int text)
        {
            return key ^ text;
        }

        /// <summary>
        /// getting a decimal from binary
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int GetNumberFromList(List<int> list)
        {
            double toReturn = 0;
            for (int i = list.Count - 1,j=0; i >=0 && j<list.Count; i--,j++)
            {
                toReturn += Math.Pow(2, j) * list[i];
            }

            return (int) toReturn;
        }

        /// <summary>
        /// Getting a binary from decimal
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<short> GetBinaryFromNumber(int number)
        {
            var toReturn = new List<short>();
            ulong test = 1;
            var str = Convert.ToString(number, 2);

            for (int i = 0; i < str.Length; i++)
            {
                toReturn.Add((short)Char.GetNumericValue(str[i]));
            }

            return toReturn;
        }
    }
}
