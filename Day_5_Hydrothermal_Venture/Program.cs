using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_5_Hydrothermal_Venture
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<HydrothermalVent> hydrothermalVents = inputList.Select(line => new HydrothermalVent(line)).ToList();

            P1(hydrothermalVents);
            P2(hydrothermalVents);
        }

        static void P1(List<HydrothermalVent> hydrothermalVents)
        {
            Dictionary<Tuple<int, int>, int> map = new Dictionary<Tuple<int, int>, int>();

            foreach (HydrothermalVent hydrothermalVent in hydrothermalVents)
            {
                List<Tuple<int, int>> coveredCoordinates = hydrothermalVent.GetCoveredCoordinates(false);
                foreach (Tuple<int, int> coveredCoordinate in coveredCoordinates)
                {
                    if (map.ContainsKey(coveredCoordinate))
                        map[coveredCoordinate]++;
                    else
                        map[coveredCoordinate] = 1;
                }
            }

            int numberOfMultiOverlaps = 0;
            foreach (var kVP in map)
            {
                if (kVP.Value > 1)
                    numberOfMultiOverlaps++;
            }

            Console.WriteLine(numberOfMultiOverlaps);
            Console.ReadLine();
        }

        static void P2(List<HydrothermalVent> hydrothermalVents)
        {
            Dictionary<Tuple<int, int>, int> map = new Dictionary<Tuple<int, int>, int>();

            foreach (HydrothermalVent hydrothermalVent in hydrothermalVents)
            {
                List<Tuple<int, int>> coveredCoordinates = hydrothermalVent.GetCoveredCoordinates(true);
                foreach (Tuple<int, int> coveredCoordinate in coveredCoordinates)
                {
                    if (map.ContainsKey(coveredCoordinate))
                        map[coveredCoordinate]++;
                    else
                        map[coveredCoordinate] = 1;
                }
            }

            int numberOfMultiOverlaps = 0;
            foreach (var kVP in map)
            {
                if (kVP.Value > 1)
                    numberOfMultiOverlaps++;
            }

            Console.WriteLine(numberOfMultiOverlaps);
            Console.ReadLine();
        }
    }

    public class HydrothermalVent
    {
        public int X1 = 0;
        public int Y1 = 0;
        public int X2 = 0;
        public int Y2 = 0;

        public HydrothermalVent(string inputLine)
        {
            string[] split1 = inputLine.Split(' ');
            string[] split2 = split1[0].Split(',');
            string[] split3 = split1[2].Split(',');
            X1 = Int32.Parse(split2[0]);
            Y1 = Int32.Parse(split2[1]);
            X2 = Int32.Parse(split3[0]);
            Y2 = Int32.Parse(split3[1]);
        }

        public List<Tuple<int, int>> GetCoveredCoordinates(bool includeDiagonals)
        {
            List<Tuple<int, int>> coveredCoordinates = new List<Tuple<int, int>>();

            if (X1 == X2)
            {
                int lowerY = Y1;
                int higherY = Y2;
                if (Y2 < lowerY)
                {
                    lowerY = Y2;
                    higherY = Y1;
                }

                for (int i = lowerY; i <= higherY; i++)
                    coveredCoordinates.Add(new Tuple<int, int>(X1, i));
            }
            else if (Y1 == Y2)
            {
                int lowerX = X1;
                int higherX = X2;
                if (X2 < lowerX)
                {
                    lowerX = X2;
                    higherX = X1;
                }

                for (int i = lowerX; i <= higherX; i++)
                    coveredCoordinates.Add(new Tuple<int, int>(i, Y1));
            }
            else if (includeDiagonals)
            {
                bool swap = false;
                int lowerX = X1;
                int higherX = X2;
                if (X2 < lowerX)
                {
                    swap = true;
                    lowerX = X2;
                    higherX = X1;
                }

                for (int i = 0; i <= higherX - lowerX; i++)
                    coveredCoordinates.Add(new Tuple<int, int>(
                        lowerX + i,
                        swap ?
                            Y2 > Y1 ? Y2 - i : Y2 + i
                            :
                            Y2 > Y1 ? Y1 + i : Y1 - i
                        ));
            }
            return coveredCoordinates;
        }
    }
}
