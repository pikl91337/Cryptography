using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Common
{
    /// <summary>
    /// Class for console-user communication
    /// </summary>
    public static class ConsoleMessager
    {
        /// <summary>
        /// Ask user if he wants to generate new alphabet (he has no choice :D)
        /// </summary>
        /// <returns></returns>
        public static bool WillGenerateAlphabet()
        {
            Console.WriteLine("Do you want to generate new alphabet? (Y/y)");
            var toReturn = false;
            var answer = Console.ReadLine();

            while (!toReturn)
            {
                switch (answer)
                {
                    case "Y":
                        toReturn = true;
                        break;
                    case "y":
                        toReturn = true;
                        break;
                    default:
                        Console.WriteLine("Type either Y or y");
                        answer = Console.ReadLine();
                        break;
                }
            }
            Console.WriteLine("Alright! a key is going to be generated and saved to key.txt");
            Console.WriteLine();
            return toReturn;
        }

        public static void ShowDifferentBits(string inputString, string inputStringWithOneBitChanged, int numberOfDifferentBits)
        {
            Console.WriteLine(String.Format("Hash functions of two string values ('{0}' '{1}') have been compared." +
                                            " Total amount of different bits is {2}", inputString, inputStringWithOneBitChanged, numberOfDifferentBits));
            Console.WriteLine("-------------------------------------------------------------");
        }

        public static int AskStringLength()
        {
            Console.WriteLine("Please enter a length for future string generating (L).");
            var answer = Console.ReadLine();
            var L = 0;
            var isInt = int.TryParse(answer, out L);

            while (!isInt)
            {
                Console.WriteLine("Enter in value");
                answer = Console.ReadLine();

                isInt = int.TryParse(answer, out L);

            }
            Console.WriteLine("-------------------------------------------------------------");

            return L;
        }

        public static int AskHashLength()
        {
            Console.WriteLine("Please enter a bit hash length (from 1 to 32) to find a collision (k).");
            var answer = Console.ReadLine();
            var k = 0;
            var isInt = int.TryParse(answer, out k);

            while (k > 64 | k < 1)
            {
                Console.WriteLine("Enter a valid length (1..32)");
                answer = Console.ReadLine();

                isInt = int.TryParse(answer, out k);

            }
            Console.WriteLine("-------------------------------------------------------------");

            return k;
        }

        public static void PrintCollisionResults(string firstHash, string firstRandomString, string secondHash, string secondRandomString, string firstBitsString, int N)
        {
            Console.WriteLine("Results:");
            Console.WriteLine(String.Format("First string: {0}", firstRandomString));
            Console.WriteLine(String.Format("First hash: {0}", firstHash));
            
            Console.WriteLine(String.Format("Second string: {0}", secondRandomString));
            Console.WriteLine(String.Format("Second hash: {0}", secondHash));

            Console.WriteLine(String.Format("Bits: {0}", firstBitsString));
            Console.WriteLine(String.Format("Number of strings generated: {0}", N));

            Console.WriteLine("-------------------------------------------------------------");

        }

        /// <summary>
        /// Asks user to input a string value and returns this value
        /// </summary>
        /// <returns>string value</returns>
        public static string AskString()
        {
            Console.WriteLine("Hello! Please, input any string value.");
            var answer = Console.ReadLine();
            Console.WriteLine(String.Format("Alright! You entered value '{0}'. Its all good.",answer));
            Console.WriteLine("-------------------------------------------------------------");

            return answer;

        }

        /// <summary>
        /// Ask user block length
        /// </summary>
        /// <returns>int</returns>
        public static int ReadBlockLength()
        {
            Console.WriteLine("Input block size. Block size - amout of symbols which will be encrypted at a time");

            int blockLength;
            var answer = Console.ReadLine();
            bool parsedSuccessfully = int.TryParse(answer, out blockLength);

            while (!parsedSuccessfully)
            {
                Console.WriteLine("Your value is not integer, input integer value.");
                answer = Console.ReadLine();
                parsedSuccessfully = int.TryParse(answer, out blockLength);
            }

            return Convert.ToInt16(blockLength);
        }

        /// <summary>
        /// Asking file name
        /// </summary>
        /// <returns></returns>
        public static string AskFilename()
        {
            Console.WriteLine("Hello. Input file name which you wish to encrypt and decrypt: ");
            var answer = Console.ReadLine();

            while (!File.Exists(answer))
            {
                Console.WriteLine("There is no such file. Input existed file name: ");
                answer = Console.ReadLine();
            }
            Console.WriteLine("Ok! We found it.");
            Console.WriteLine("-------------------------------------------------------------");


            return answer;
        }

        /// <summary>
        /// Asking polinom numbers
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<int> AskPolinomNumbers()
        {
            Console.WriteLine("");
            Console.WriteLine("Now enter two numbers that will be used in the polynomial. A polynomial is needed to generate an M-sequence using a register. " +
                              "Possible numbers: 30 and 2, 30 and 5, 30 and 6. BE CAREFUL, IF YOU ENTER NUMBERS NOT FROM THE LIST, THE ALGORITHM WILL WORK INCORRECTLY");

            Console.Write("First number: ");
            var answer1 = Console.ReadLine();
            
            Console.Write("Second number: ");
            var answer2 = Console.ReadLine();

            var number1 = 0;
            var number2 = 0;

            var isValid = int.TryParse(answer1, out number1) && int.TryParse(answer2, out number2);

            while (!isValid)
            {
                Console.WriteLine("Nope. Not numbers.");

                Console.Write("First number: ");
                answer1 = Console.ReadLine();

                Console.Write("Second number: ");
                answer2 = Console.ReadLine();

                isValid = int.TryParse(answer1, out number1) && int.TryParse(answer2, out number2);
            }

            var toReturn = new List<int>() { number1, number2};
            Console.WriteLine("-------------------------------------------------------------");

            return toReturn;
        }

        /// <summary>
        /// Asking series length
        /// </summary>
        /// <param name="mLength"></param>
        /// <returns></returns>
        public static int AskBlockLength(int mLength)
        {
            Console.WriteLine();
            Console.WriteLine("Now let's check the key, the file before encryption, and the encrypted files with serial and correlation tests." +
                              " Enter the size of one so-called series: ");

            var answer = Console.ReadLine();
            var k = 0;
            var isInt = int.TryParse(answer, out k);

            while (k > mLength)
            {
                Console.WriteLine("The length of a single series is longer than the length of the entire message. Let's fix this. Enter a smaller number " + mLength);
                answer = Console.ReadLine();

                isInt = int.TryParse(answer, out k);

            }
            Console.WriteLine("-------------------------------------------------------------");

            return k;
        }

        /// <summary>
        /// Asking user for a number to divide the key length by
        /// </summary>
        /// <returns></returns>
        public static int AskKeyDivideOnNumber()
        {
            Console.WriteLine("Enter the number to divide the key length by. For example, the degree of two (2 4 8 16 32 64 128 256 512 ... 16384 32768)");
            var answer = Console.ReadLine();
            var number = 0;
            var isInt = int.TryParse(answer, out number);

            while (!isInt)
            {
                Console.WriteLine("Number is not an integer. Input integer");
                answer = Console.ReadLine();
                isInt = int.TryParse(answer, out number);
            }

            Console.WriteLine("-------------------------------------------------------------");

            return number;

        }

        /// <summary>
        /// Ask the user for a coefficient
        /// </summary>
        /// <returns></returns>
        public static double AskRate()
        {
            Console.WriteLine("Input number");
            var answer = Console.ReadLine();
            double number = 0;
            var isNumber = double.TryParse(answer, out number);

            while (!isNumber)
            {
                Console.WriteLine("Not a number. Input a number: ");
                answer = Console.ReadLine();
                isNumber = double.TryParse(answer, out number);
            }

            Console.WriteLine("-------------------------------------------------------------");

            return number;

        }

        /// <summary>
        /// Displays the results of the serial test to the user
        /// </summary>
        /// <param name="khi">khi value</param>
        /// <param name="k">series length</param>
        /// <param name="teorFrequency">theoretical frequency</param>
        /// <param name="practFrequencies">practical frequencies of each series</param>
        public static bool CheckKhiValue(double khi, int k, double teorFrequency, Dictionary<int, int> practFrequencies,string testedName)
        {
            var toReturn = false;
            Console.WriteLine();
            Console.WriteLine("FREQUENCY TEST");
            Console.WriteLine(String.Format("Testing {0}",testedName));
            if (k == 2)
            {
                if (khi>0.584 && khi<6.251)
                {
                    var one = 0.584;
                    var two = 6.251;
                    WriteInfoGraphicAboutSeries(khi, teorFrequency, practFrequencies,one,two);
                    toReturn = true;
                }
                else
                {
                    Console.WriteLine(String.Format("Test failed. The khi value was equal to {0} and did not fit into the frame ({1}; {2})", khi, 0.584, 6.251));
                }
            }
            if (k == 3)
            {
                if (khi>2.833 && khi<12.017)
                {
                    var one = 2.833;
                    var two = 12.017;
                    WriteInfoGraphicAboutSeries(khi, teorFrequency, practFrequencies, one, two);
                    toReturn = true;

                }
                else
                {
                    Console.WriteLine(String.Format("Test failed. The khi value was equal to {0} and did not fit into the frame ({1}; {2})", khi, 2.833, 12.017));
                }
            }
            if (k == 4)
            {
                if (khi>8.547 && khi<22.307)
                {
                    var one = 8.547;
                    var two = 22.307;
                    WriteInfoGraphicAboutSeries(khi, teorFrequency, practFrequencies, one, two);
                    toReturn = true;

                }
                else
                {
                    Console.WriteLine(String.Format("Test failed. The khi value was equal to {0} and did not fit into the frame ({1}; {2})", khi, 8.547, 22.307));
                }
            }

            if (k > 4 || k == 1)
            {
                Console.WriteLine("There is no information in the manual. Enter a number less than or equal to 4 to pass the test");
            }
            Console.WriteLine("-------------------------------------------------------------");

            return toReturn;

        }

        public static void WriteInfoGraphicAboutSeries(double khi, double teorFrequency, Dictionary<int, int> practFrequencies,double one,double two)
        {
            Console.WriteLine("Тест пройден! значение хи квадрат: " + khi);
            Console.WriteLine(String.Format("It was necessary to keep within the limits ({0}; {1})", one,two));
            Console.WriteLine("Infographics:");

            Console.WriteLine(String.Format("theoretical frequency: {0}", teorFrequency));
            Console.WriteLine("practical frequencies:");
            Console.WriteLine(String.Format("Series (decimal) - frequency"));
            foreach (var oneFreq in practFrequencies)
            {
                Console.WriteLine(String.Format("{0} - {1}", oneFreq.Key, oneFreq.Value));
            }
        }

        /// <summary>
        /// Present the results of a correlation test to the user
        /// </summary>
        /// <param name="rk"></param>
        /// <param name="rkr"></param>
        public static void ShowCorelationTestResults(double rk, double rkr,string testedName)
        {
            Console.WriteLine();
            Console.WriteLine("CORRELATION TEST");
            Console.WriteLine(String.Format("Test {0}.",testedName));
            Console.WriteLine("Results:");

            if (Math.Abs(rk) < rkr)
            {
                Console.WriteLine(String.Format("The correlation test was passed successfully. Value Rk = {0}; Rкр = {1}", Math.Abs(rk), rkr));
            }
            else
            {
                Console.WriteLine(String.Format("The correlation test was passed successfully. Value Rk = {0}; Rкр = {1}", Math.Abs(rk), rkr));
            }
            Console.WriteLine("-------------------------------------------------------------");
        }

        /// <summary>
        /// Asking key length
        /// </summary>
        /// <returns>Возвращает инт</returns>
        public static int AskKeyLength()
        {
            Console.WriteLine();
            Console.WriteLine("Input key length ");

            var answer = Console.ReadLine();
            var k = 0;
            var isInt = int.TryParse(answer, out k);

            while (!isInt)
            {
                Console.WriteLine("This is not an integer. Enter an integer: ");
                answer = Console.ReadLine();

                isInt = int.TryParse(answer, out k);

            }
            Console.WriteLine("-------------------------------------------------------------");

            return k;
        }


        public static void DemonstrateHashResult(string inputString, string resultHash)
        {
            Console.WriteLine(String.Format("Program calculated hash-function for input value '{0}'. Hash-function is '{1}'",inputString,resultHash));

            Console.WriteLine("-------------------------------------------------------------");
        }
    }
}
