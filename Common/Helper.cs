using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// Helper for all projects
    /// </summary>
    public class Helper
    {

        /// <summary>
        /// Getting byte array from string
        /// </summary>
        /// <param name="s">string value</param>
        /// <returns>byte array</returns>
        public static List<byte> GetBytesFromString(String s)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < s.Length; i++)
            {
                bytes.Add(Convert.ToByte(s[i]));
            }
            return bytes;
        }
        
        /// <summary>
        /// Getting byte array from string
        /// </summary>
        /// <param name="s">string value</param>
        /// <returns>byte array</returns>
        public static string GetStringFromBytes(List<byte> bytes)
        {
            string str = "";
            for (int i = 0; i < bytes.Count; i++)
            {
                str += Convert.ToString((char)bytes[i]);
            }
            return str;
        }

        /// <summary>
        /// Получаем двоичную последовательность из числа
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static List<short> GetBinaryFromNumber(int number)
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

        /// <summary>
        /// Приведение к нужному двоичному формату (добавление незначащих нулей)
        /// </summary>
        /// <param name="block">Блок для приведения</param>
        /// <param name="L">Длина выходного блока</param>
        /// <returns>Возвращает список целых чисел</returns>
        public static List<short> BringingToNeededBinaryFormat(List<short> block, int L)
        {
            var toReturn = new List<short>();

            // добавляем незначащие нули в начало списка (в старшие биты)
            for (int i = block.Count; i < L; i++)
            {
                toReturn.Add(0);
            }

            // добавляем к незначашим нулям в конец само двоичное число
            toReturn.AddRange(block);
            return toReturn;
        }

        /// Получаем число из двоичной последовательность
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static int GetNumberFromList(List<short> list)
        {
            double toReturn = 0;
            for (int i = list.Count - 1, j = 0; i >= 0 && j < list.Count; i--, j++)
            {
                toReturn += Math.Pow(2, j) * list[i];
            }

            return (int)toReturn;
        }


        public static string PrintList(List<short> list)
        {
            var toReturn = "";
            foreach (var charact in list)
            {
                toReturn += charact;
            }

            return toReturn;
        }
    }
}
