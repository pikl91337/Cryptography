using System;
using System.Collections.Generic;
using System.Text;

namespace SubstitutionCipher
{
    /// <summary>
    /// Entropy calculator
    /// </summary>
    public class EntropyWorker
    {
        /// <summary>
        /// Entropy calculation
        /// </summary>
        /// <param name="text">message</param>
        /// <returns>double number</returns>
        public double CalculateEntropy(string text)
        {
            Dictionary<char, int> characterDict = new Dictionary<char, int>();

            // filling a dictionary with all possible keys
            foreach (var character in text)
            {
                if (!characterDict.ContainsKey(character))
                {
                    characterDict.Add(character,0);
                }
            }

            // calculating an amount of every character in message
            foreach (var character in text)
            {
                var newValue = characterDict[character] + 1;
                characterDict[character] = newValue;
            }

            double entropy = 0;

            // calculating entropy value
            foreach (var onePair in characterDict)
            {
                float frequency = (float)onePair.Value / text.Length;
                entropy += frequency * Math.Log2(frequency);
            }

            return -entropy;
        }
    }
}
