using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubstitutionCipher
{
    /// <summary>
    /// Substitution cypher
    /// </summary>
    public class Encoder
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="text">message</param>
        /// <param name="alphabet">A dictionary where key is a russian letter, value - letter from new alphabet</param>
        /// <returns>char array</returns>
        public char[] Crypt(char[] text, Dictionary<char,char> alphabet)
        {
            var cryptedText = "";

            foreach (var oneCharacter in text)
            {
                try
                {
                    cryptedText += alphabet[char.ToLower(oneCharacter)];
                }
                // if symbol which is not in dictionary detected, leave an empty space
                catch (KeyNotFoundException e)
                {
                    continue;
                }
            }
            return cryptedText.ToCharArray();
        }

        /// <summary>
        /// Дешифрует текст
        /// Decryption
        /// </summary>
        /// <param name="cryptedText">encrypted message</param>
        /// <param name="alphabet">key dictionary</param>
        /// <returns>char array</returns>
        public char[] DeCrypt(char[] cryptedText, Dictionary<char, char> alphabet)
        {
            var text = "";

            foreach (var oneCharacter in cryptedText)
            {
                text += alphabet.FirstOrDefault(x => x.Value == char.ToLower(oneCharacter)).Key;
            }

            return text.ToCharArray();
        }
    }
}
