using System.Collections.Generic;

namespace AsymmetricCypher
{
    /// <summary>
    /// generator of Binary number represented by list 
    /// </summary>
    public static class MaskGenerator
    {
        /// <summary>
        /// Making binary number of a certain length with ones at he beginning and at the end
        /// </summary>
        /// <param name="numberLength">Binary length</param>
        /// <returns>List int</returns>
        public static List<short> GetMask(int numberLength)
        {
            var toReturn = new List<short>();
            toReturn.Add(1); // set lowest bit to 1

            for (int i = 1; i < numberLength -1 ; i++)
            {
                toReturn.Add(0);
            }

            toReturn.Add(1); // set the most significant bit to 1
            return toReturn;
        }
    }
}