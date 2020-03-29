using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace StreamCypher
{
    /// <summary>
    /// Frequency tester
    /// </summary>
    public class FrequencyTester
    {
        /// <summary>
        /// One block length
        /// </summary>
        private int _K;

        /// <summary>
        /// Actual frequencies
        /// </summary>
        public Dictionary<int, int> _PossibleBlocks;

        /// <summary>
        /// Theoretical frequency
        /// </summary>
        public double _TeorFrequency;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="k">block length</param>
        /// <param name="mLength">message length</param>
        public FrequencyTester(int k, int mLength)
        {
            if (k < mLength)
            {
                _K = k;
            }
            else
            {
                throw new Exception("The length of on block fore frequency tes is greater than length of the whole message");
            }

            _PossibleBlocks = GetBlocks();  
        }

        public double Test(List<short> m)
        {
            // calculating actual frequencies
            CalculateFrequency(m);

            var khiKvadrat = CalculateKhi(m.Count/_K);

            return khiKvadrat;
        }

        /// <summary>
        /// Возвращает словарь с возможными значениями одного блока
        /// Getting a dictionary with all possible blocks, values filled with zeros
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, int> GetBlocks()
        {
            Dictionary<int, int> toReturn = new Dictionary<int, int>();
            //a number of possible blocks
            var numberOfBlocks = Math.Pow(2, _K);

            for (int i = 0; i < numberOfBlocks; i++)
            {
                toReturn.Add(i, 0);
            }

            return toReturn;
        }

        /// <summary>
        /// getting a decimal from binary
        /// </summary>
        /// <param name="list">binary number placed in list int</param>
        /// <returns>int</returns>
        public int GetNumberFromList(List<short> list)
        {
            double toReturn = 0;
            for (int i = list.Count - 1, j = 0; i >= 0 && j < list.Count; i--, j++)
            {
                toReturn += Math.Pow(2, j) * list[i];
            }

            return (int)toReturn;
        }

        /// <summary>
        /// Calculating actual frequencies
        /// </summary>
        /// <param name="m"></param>
        private void CalculateFrequency(List<short> m)
        {
            for (int i = 0; i < m.Count - _K;)
            {
                var oneBlock = new List<short>();
                for (int j = 0; j < _K; j++)
                {
                    oneBlock.Add(m[i + j]);
                }

                var oneBlockNumber = GetNumberFromList(oneBlock);

                var newValue = _PossibleBlocks[oneBlockNumber] + 1;
                _PossibleBlocks[oneBlockNumber] = newValue;
                i = i + _K;
            }
        }

        /// <summary>
        /// Calculate khi
        /// </summary>
        /// <param name="n">number of nonoverlapping series</param>
        /// <returns></returns>
        private double CalculateKhi(int n)
        {
            double toReturn = 0;
            var teorFreq = n / Math.Pow(2, _K);
            _TeorFrequency = teorFreq;

            for (int i = 0; i < _PossibleBlocks.Count - 1; i++)
            {
                toReturn += (Math.Pow(_PossibleBlocks[i] - teorFreq, 2)) / teorFreq;
            }

            return toReturn;
        }
    }
}
