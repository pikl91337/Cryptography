using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Common
{
    /// <summary>
    /// Класс для преобразования в обе стороны файл в битовую последовательность
    /// </summary>
    public static class FileWorker
    {
        /// <summary>
        /// Этот метод преобразовывает файл любого (указанного в параметрах) формата в байтовую последовательность 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static List<Int16> GetBinaryFile(string filename)
        {
            byte[] bytes;
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                bytes = new byte[file.Length];
                file.Read(bytes, 0, (Int16)file.Length);
            }
            BitArray bits = new BitArray(bytes);//заводим битовый массив для дальнейшей его кодировки (содержание - последовательность true и false)
            BitArray bitsNEW = new BitArray(bytes);//аналогичный битовый массив на всякий пожарный

            List<Int16> iInt16 = new List<Int16>();//массив целых чисел для получения единичнонулевой последовательности
            for (Int16 i = 0; i < bits.Length; i++)//заполняем массив бинарной последоавтельностью
                iInt16.Add(Convert.ToInt16(bits[i]));
            return iInt16;
        }

        /// <summary>
        /// Этот метод преобразовывает битовый лист в файл указанного вручную формата 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void GetNEWFile(List<Int16> iInt16, string filename)
        {
            BitArray bitsNEW = new BitArray(iInt16.Count);
            for (Int16 i = 0; i < iInt16.Count; i++)//делаем обратное преобразование в последовательность тру и фолсов
                bitsNEW[i] = Convert.ToBoolean(iInt16[i]);


            byte[] file_final = new byte[bitsNEW.Length / 8 + 1];//делаем финальный байтовый массив
            bitsNEW.CopyTo(file_final, 0);//копируем битовый массив в байтовый
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                file.Write(file_final, 0, (Int16)file_final.Length);
            }
        }

        /// <summary>
        /// Записывает большое целое число в файл
        /// </summary>
        /// <param name="number">Большое целое число</param>
        /// <param name="filename">Название файла</param>
        public static void CreateNEWFile(BigInteger number, string filename)
        {
            var sw = new StreamWriter(filename, false);
            sw.Write(number);
            sw.Close();
        }
        
        /// <summary>
        /// Записывает строку в текст
        /// </summary>
        /// <param name="number">Большое целое число</param>
        /// <param name="filename">Название файла</param>
        public static void CreateNEWFile(string text, string filename)
        {
            var sw = new StreamWriter(filename, false);
            sw.Write(text);
            sw.Close();
        }

        /// <summary>
        /// Запись двоичного файла из листа интов в файл
        /// </summary>
        /// <param name="text">Двоичное число внутри списка интов</param>
        /// <param name="filename">Название файла</param>
        public static void CreateTextFileForBinaryNumber(List<Int16> text, string filename)
        {
            var sw = new StreamWriter(filename, true);
            foreach (var litera in text)
            {
                sw.Write(litera);
            }
            sw.Close();
        }
    }
}
