using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_1_Sonar_Sweep
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
            int lastVal = Int32.MaxValue;
            int increaseCount = 0;
            foreach (string inputLine in inputList)
            {
                int newVal = Int32.Parse(inputLine);
                if (newVal > lastVal)
                    increaseCount++;
                lastVal = newVal;
            }

            Console.WriteLine("P1 {0}", increaseCount);
            Console.ReadLine();
        }

        static void P2(List<string> inputList)
        {
            int increaseCount = 0;
            int[] vals = new int[] { 0, 0, 0 };
            int loc = 0;
            int lastRunningTotal = 0;
            foreach (string inputLine in inputList)
            {
                int newVal = Int32.Parse(inputLine);

                if (loc < 3)
                {
                    vals[loc] = newVal;
                    loc++;
                }
                else
                {
                    lastRunningTotal = vals.Sum();

                    vals[0] = vals[1];
                    vals[1] = vals[2];
                    vals[2] = newVal;

                    int newRunningTotal = vals.Sum();

                    if (newRunningTotal > lastRunningTotal)
                        increaseCount++;

                    lastRunningTotal = newRunningTotal;
                }
            }

            Console.WriteLine("P2 {0}", increaseCount);
            Console.ReadLine();
        }
    }
}
