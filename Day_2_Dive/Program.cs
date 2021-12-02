using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_2_Dive
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
            int startH = 0;
            int startD = 0;

            foreach (string inputLine in inputList)
            {
                string[] split = inputLine.Split(' ');
                int val = Int32.Parse(split[split.Length - 1]);

                switch (inputLine[0])
                {
                    case 'f':
                        {
                            startH += val;
                            break;
                        }
                    case 'd':
                        {
                            startD += val;
                            break;
                        }
                    case 'u':
                        {
                            startD -= val;
                            break;
                        }
                }
            }

            int result = startH * startD;
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static void P2(List<string> inputList)
        {
            int startH = 0;
            int startD = 0;
            int startA = 0;

            foreach (string inputLine in inputList)
            {
                string[] split = inputLine.Split(' ');
                int val = Int32.Parse(split[split.Length - 1]);

                switch (inputLine[0])
                {
                    case 'f':
                        {
                            startH += val;
                            startD += startA * val;
                            break;
                        }
                    case 'd':
                        {
                            startA += val;
                            break;
                        }
                    case 'u':
                        {
                            startA -= val;
                            break;
                        }
                }
            }

            int result = startH * startD;
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
