using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SubstitutionCipher
{
    /// <summary>
    /// Getting a key from file
    /// </summary>
    public static class KeyWorker
    {
        /// <summary>
        /// Reading alphabet-key from file
        /// </summary>
        /// <param name="sr">stream reader</param>
        /// <returns>dictionary char char</returns>
        public static Dictionary<char, char> GetKey(StreamReader sr)
        {
            var key = new Dictionary<char,char>();
            while (!sr.EndOfStream)
            {
                var oneLine = sr.ReadLine();
                key.Add(oneLine[0], oneLine[2]);
            }

            return key;
        }

        /// <summary>
        /// Generating an alphabet-key and writing it to file
        /// </summary>
        /// <param name="sw">stream writer</param>
        public static void GenerateKey(StreamWriter sw)
        {
            char[] options = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и',
                'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф',
                'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', ' ' };
            var temp = new string(options);


            Random random = new Random();

            foreach (var charachter in options)
            {
                var randPositionInOptions = random.Next(temp.Length);

                // to exclude reoccurrence of letters
                while (options[randPositionInOptions] != charachter)
                {
                    randPositionInOptions = random.Next(temp.Length);
                }
                var oneLine = charachter + " " + options[randPositionInOptions];
                temp = temp.Remove(randPositionInOptions,1);

                options = temp.ToCharArray();

                sw.WriteLine(oneLine);
            }
        }
    }
}