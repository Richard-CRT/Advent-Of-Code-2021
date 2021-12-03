using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_3_Binary_Diagnostic
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            P1(inputList);
            P2(inputList);
        }

        static void P1(List<string> inputList)
        {
            uint gamma = 0;
            for (int i = 0; i < inputList[0].Length; i++)
            {
                uint zeroes = 0;
                uint ones = 0;
                foreach (string binaryVal in inputList)
                {
                    if (binaryVal[i] == '0')
                        zeroes++;
                    else
                        ones++;
                }
                gamma = gamma << 1;
                if (ones >= zeroes)
                {
                    gamma |= 1;
                }
            }

            uint result = gamma * (~gamma & ((uint)Math.Pow(2, inputList[0].Length) - 1));
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static void P2(List<string> inputList)
        {
            List<string> inputListO2Copy = new List<string>(inputList);
            List<string> inputListCO2Copy = new List<string>(inputList);
            int bitPosition;
            bitPosition = 0;
            while (inputListO2Copy.Count > 1)
            {
                inputListO2Copy = Cull(inputListO2Copy, bitPosition, false);
                bitPosition++;
            }
            bitPosition = 0;
            while (inputListCO2Copy.Count > 1)
            {
                inputListCO2Copy = Cull(inputListCO2Copy, bitPosition, true);
                bitPosition++;
            }
            Console.WriteLine(Convert.ToUInt32(inputListO2Copy[0], 2) * Convert.ToUInt32(inputListCO2Copy[0], 2));
            Console.ReadLine();
        }

        static List<string> Cull(List<string> list, int bitPosition, bool reverse)
        {
            uint zeroes = 0;
            uint ones = 0;
            foreach (string binaryVal in list)
            {
                if (binaryVal[bitPosition] == '0')
                    zeroes++;
                else
                    ones++;
            }

            bool keepOnes = (ones >= zeroes) ^ reverse;

            for (int i = 0; i < list.Count;)
            {
                string str = list[i];

                if (str[bitPosition] == '0' && keepOnes)
                    list.RemoveAt(i);
                else if (str[bitPosition] == '1' && !keepOnes)
                    list.RemoveAt(i);
                else
                {
                    i++;
                }
            }

            return list;
        }
    }
}
