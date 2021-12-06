using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_6_Laternfish
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();
            UInt64[] data = inputList[0].Split(',').Select(i => UInt64.Parse(i)).ToArray();

            UInt64[] howManyFishAtEachDay;

            howManyFishAtEachDay = new UInt64[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (int i in data)
                howManyFishAtEachDay[i]++;
            P1(howManyFishAtEachDay);

            howManyFishAtEachDay = new UInt64[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (int i in data)
                howManyFishAtEachDay[i]++;
            P2(howManyFishAtEachDay);
        }

        static void P1(UInt64[] howManyFishAtEachDay)
        {
            for (int j = 0; j < 80; j++)
            {
                howManyFishAtEachDay = Tick(howManyFishAtEachDay);
            }

            UInt64 sum = 0;
            for (int i = 0; i < howManyFishAtEachDay.Length; i++)
                sum += howManyFishAtEachDay[i];
            Console.WriteLine(sum);
            Console.ReadLine();
        }

        static void P2(UInt64[] howManyFishAtEachDay)
        {
            for (int j = 0; j < 256; j++)
            {
                howManyFishAtEachDay = Tick(howManyFishAtEachDay);
            }

            UInt64 sum = 0;
            for (int i = 0; i < howManyFishAtEachDay.Length; i++)
                sum += howManyFishAtEachDay[i];
            Console.WriteLine(sum);
            Console.ReadLine();
        }

        static UInt64[] Tick(UInt64[] howManyFishAtEachDay)
        {
            UInt64 fishAboutToSpawn = howManyFishAtEachDay[0];
            for (int i = 0; i <= 7; i++)
            {
                howManyFishAtEachDay[i] = howManyFishAtEachDay[i + 1];
            }
            howManyFishAtEachDay[6] += fishAboutToSpawn;
            howManyFishAtEachDay[8] = fishAboutToSpawn;

            return howManyFishAtEachDay;
        }
    }
}
