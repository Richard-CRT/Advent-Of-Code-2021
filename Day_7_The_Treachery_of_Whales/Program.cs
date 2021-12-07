using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_7_The_Treachery_of_Whales
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            List<int> horizontalPositions = inputList[0].Split(',').Select(i => int.Parse(i)).ToList();

            P1(horizontalPositions);
            P2(horizontalPositions);
        }

        static void P1(List<int> horizontalPositions)
        {
            int minFuelUsage = int.MaxValue;
            int min = horizontalPositions.Min();
            int max = horizontalPositions.Max();
            for (int targetHorizontalPosition = min; targetHorizontalPosition <= max; targetHorizontalPosition++)
            {
                int fuelUsage = 0;
                foreach (int horizontalPosition in horizontalPositions)
                {
                    fuelUsage += Math.Abs(horizontalPosition - targetHorizontalPosition);
                }
                if (fuelUsage < minFuelUsage)
                    minFuelUsage = fuelUsage;
            }

            Console.WriteLine(minFuelUsage);
            Console.ReadLine();
        }

        static void P2(List<int> horizontalPositions)
        {
            int minFuelUsage = int.MaxValue;
            int min = horizontalPositions.Min();
            int max = horizontalPositions.Max();
            for (int targetHorizontalPosition = min; targetHorizontalPosition <= max; targetHorizontalPosition++)
            {
                int fuelUsage = 0;
                foreach (int horizontalPosition in horizontalPositions)
                {
                    int dif = Math.Abs(horizontalPosition - targetHorizontalPosition);
                    fuelUsage += TriangleNumber(dif);
                }
                if (fuelUsage < minFuelUsage)
                    minFuelUsage = fuelUsage;
            }

            Console.WriteLine(minFuelUsage);
            Console.ReadLine();
        }

        static int TriangleNumber(int n)
        {
            return (n*(n + 1)) / 2;
        }
    }
}
