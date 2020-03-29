using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace HashFunction
{
    /// <summary>
    /// Class for MD4 Algorithm (Hash-function)
    /// </summary>
    public class MdFourAlgorithm
    {
        public uint _A;
        public uint _B;
        public uint _C;
        public uint _D;

        public List<byte> ResultHashBytes;
        public String ResultHashString;

        /// <summary>
        /// Constructor
        /// </summary>
        public MdFourAlgorithm()
        {
            _A = 0x67452301;
            _B = 0xefcdab89;
            _C = 0x98badcfe;
            _D = 0x10325476;
        }
        /// <summary>
        /// Entrance point to the class logic.
        /// </summary>
        /// <param name="inputString">Initial string to calculate hash code</param>
        /// <returns>String value (Hash code)</returns>
        public void Execute(string inputString)
        {
            // getting byte array from string
            var inputBytes = Helper.GetBytesFromString(inputString);

            // extending initial list (making it binary length to be a multiple of 512)
            ExtendMessage(inputBytes);

            // calculating actual hash from a List<int16>
            var resultHash = GetHash(inputBytes);

            ResultHashBytes = resultHash;
        }

        /// <summary>
        /// Calculate hash of binary number
        /// </summary>
        /// <param name="extendedInputBin"></param>
        /// <returns></returns>
        private List<byte> GetHash(List<byte> extendedInputBytes)
        {

            // running through all message
            for (int i = 0; i < extendedInputBytes.Count;)
            {
                var wordsList = new List<UInt32>();

                // getting block with 512 bit size
                var oneBlock = extendedInputBytes.GetRange(i, 64);

                // running through one block
                for (int j = 0; j < oneBlock.Count;)
                {
                    // getting one normal "word"
                    var oneWordByte = oneBlock.GetRange(j, 4);

                    // adding one word to the list of words
                    var oneWord = BitConverter.ToUInt32(oneWordByte.ToArray(),0);
                    wordsList.Add(oneWord);

                    j += 4;
                }

                // doing three round for this block with obtained list of words
                DoThreeRounds(wordsList);

                i += 64;
            }

            var aString = GetStringHash(_A);
            var bString = GetStringHash(_B);
            var cString = GetStringHash(_C);
            var dString = GetStringHash(_D);
            ResultHashString = aString + bString + cString + dString;


            var toReturn = BitConverter.GetBytes(_A).ToList();
            toReturn.AddRange(BitConverter.GetBytes(_B).ToList());
            toReturn.AddRange(BitConverter.GetBytes(_C).ToList());
            toReturn.AddRange(BitConverter.GetBytes(_D).ToList());

            return toReturn;
        }

        /// <summary>
        /// getting a 16 string representation of a uint number
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public string GetStringHash(uint a)
        {
            var toReturn = "";

            var aBytes = BitConverter.GetBytes(a);
            foreach (var oneByte in aBytes)
            {
                toReturn += Convert.ToString(oneByte, 16).PadLeft(2,'0');
            }

            return toReturn;
        }

        /// <summary>
        /// All three round of hash algorythm for one 512 bit block
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="wordsList"></param>
        private void DoThreeRounds(List<UInt32> wordsList)
        {
            var AA = _A;
            var BB = _B;
            var CC = _C;
            var DD = _D;

            _A = FirstRound(_A, _B, _C, _D, wordsList[0], 3);
            _D = FirstRound(_D, _A, _B, _C, wordsList[1], 7);
            _C = FirstRound(_C, _D, _A, _B, wordsList[2], 11);
            _B = FirstRound(_B, _C, _D, _A, wordsList[3], 19);
            
            _A = FirstRound(_A, _B, _C, _D, wordsList[4], 3);
            _D = FirstRound(_D, _A, _B, _C, wordsList[5], 7);
            _C = FirstRound(_C, _D, _A, _B, wordsList[6], 11);
            _B = FirstRound(_B, _C, _D, _A, wordsList[7], 19);

            _A = FirstRound(_A, _B, _C, _D, wordsList[8], 3);
            _D = FirstRound(_D, _A, _B, _C, wordsList[9], 7);
            _C = FirstRound(_C, _D, _A, _B, wordsList[10], 11);
            _B = FirstRound(_B, _C, _D, _A, wordsList[11], 19);

            _A = FirstRound(_A, _B, _C, _D, wordsList[12], 3);
            _D = FirstRound(_D, _A, _B, _C, wordsList[13], 7);
            _C = FirstRound(_C, _D, _A, _B, wordsList[14], 11);
            _B = FirstRound(_B, _C, _D, _A, wordsList[15], 19);
            
            _A = SecondRound(_A, _B, _C, _D, wordsList[0], 3);
            _D = SecondRound(_D, _A, _B, _C, wordsList[4], 5);
            _C = SecondRound(_C, _D, _A, _B, wordsList[8], 9);
            _B = SecondRound(_B, _C, _D, _A, wordsList[12], 13);
            
            _A = SecondRound(_A, _B, _C, _D, wordsList[1], 3);
            _D = SecondRound(_D, _A, _B, _C, wordsList[5], 5);
            _C = SecondRound(_C, _D, _A, _B, wordsList[9], 9);
            _B = SecondRound(_B, _C, _D, _A, wordsList[13], 13);
            
            _A = SecondRound(_A, _B, _C, _D, wordsList[2], 3);
            _D = SecondRound(_D, _A, _B, _C, wordsList[6], 5);
            _C = SecondRound(_C, _D, _A, _B, wordsList[10], 9);
            _B = SecondRound(_B, _C, _D, _A, wordsList[14], 13);
            
            _A = SecondRound(_A, _B, _C, _D, wordsList[3], 3);
            _D = SecondRound(_D, _A, _B, _C, wordsList[7], 5);
            _C = SecondRound(_C, _D, _A, _B, wordsList[11], 9);
            _B = SecondRound(_B, _C, _D, _A, wordsList[15], 13);
            
            _A = ThirdRound(_A, _B, _C, _D, wordsList[0], 3);
            _D = ThirdRound(_D, _A, _B, _C, wordsList[8], 9);
            _C = ThirdRound(_C, _D, _A, _B, wordsList[4], 11);
            _B = ThirdRound(_B, _C, _D, _A, wordsList[12], 15);
            
            _A = ThirdRound(_A, _B, _C, _D, wordsList[2], 3);
            _D = ThirdRound(_D, _A, _B, _C, wordsList[10], 9);
            _C = ThirdRound(_C, _D, _A, _B, wordsList[6], 11);
            _B = ThirdRound(_B, _C, _D, _A, wordsList[14], 15);
            
            _A = ThirdRound(_A, _B, _C, _D, wordsList[1], 3);
            _D = ThirdRound(_D, _A, _B, _C, wordsList[9], 9);
            _C = ThirdRound(_C, _D, _A, _B, wordsList[5], 11);
            _B = ThirdRound(_B, _C, _D, _A, wordsList[13], 15);
            
            _A = ThirdRound(_A, _B, _C, _D, wordsList[3], 3);
            _D = ThirdRound(_D, _A, _B, _C, wordsList[11], 9);
            _C = ThirdRound(_C, _D, _A, _B, wordsList[7], 11);
            _B = ThirdRound(_B, _C, _D, _A, wordsList[15], 15);

            _A = _A + AA;
            _B = _B + BB;
            _C = _C + CC;
            _D = _D + DD;

        }

        private uint Ffunction(uint x, uint y, uint z)
        {
            return (x & y) | (~x & z);

        }
        
        private uint Gfunction(uint x, uint y, uint z)
        {
            return (x & y) | (x & z) | (y & z);

        }
        
        private uint Hfunction(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }

        /// <summary>
        /// Left binary cycle shift  
        /// </summary>
        /// <param name="a">Number to be CycleShifted</param>
        /// <param name="s">Shift value</param>
        /// <returns>shifted number</returns>
        private uint CycleShift(uint a, int s)
        {
            return (a << s) | (a >> (32 - s));
        }

        /// <summary>
        /// First round of calculating hash sum
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="word"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private uint FirstRound(uint a, uint b, uint c, uint d, uint word, int s)
        {
            var funcResult = Ffunction(b, c, d);
            var toReturn = a + funcResult + word;
            toReturn = CycleShift(toReturn, s);

            return toReturn;
        }

        /// <summary>
        /// Second round of calculating hash sum
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="word"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private uint SecondRound(uint a, uint b, uint c, uint d, uint word, int s)
        {
            var toReturn = a + Gfunction(b, c, d) + word + 0x5A827999;
            toReturn = CycleShift(toReturn, s);

            return toReturn;
        }

        /// <summary>
        /// Third round of calculating hash sum
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="word"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        private uint ThirdRound(uint a, uint b, uint c, uint d, uint word, int s)
        {
            var toReturn = a + Hfunction(b, c, d) + word + 0x6ED9EBA1;
            toReturn = CycleShift(toReturn, s);

            return toReturn;
        }

        /// <summary>
        /// Extending initial message (making it length to be a multiple of 512) 
        /// </summary>
        /// <param name="inputBin">Initial message</param>
        /// <returns>Extended message</returns>
        private List<byte> ExtendMessage(List<byte> inputBytes)
        {
            var inputBinLength = inputBytes.Count * 8;

            // adding one to the end of message
            inputBytes.Add(128);

            // adding zeros so inputBytes mod 512 = 448
            AddZeros(inputBytes);

            // adding 64 bits representation of number b where b is a bits length of initial message.
            AddMessageBinaryLength(inputBytes, inputBinLength);

            return inputBytes;
        }

        /// <summary>
        /// adding zeros so inputBin mod 512 = 448
        /// </summary>
        /// <param name="inputBin">Initial message</param>
        private void AddZeros(List<byte> inputBytes)
        {
            while (inputBytes.Count * 8 % 512 != 448)
            {
                inputBytes.Add(0);
            }
        }

        /// <summary>
        /// Adding 64 representation of a inputBinLength number to the end of list
        /// </summary>
        /// <param name="inputBytes"></param>
        /// <param name="inputBinLength"></param>
        private void AddMessageBinaryLength(List<byte> inputBytes, int inputBinLength)
        {
            UInt64 b = (UInt64)inputBinLength;
            byte[] bBytes = new byte[64];
            bBytes = BitConverter.GetBytes(b);

            inputBytes.AddRange(bBytes);
        }
    }
}