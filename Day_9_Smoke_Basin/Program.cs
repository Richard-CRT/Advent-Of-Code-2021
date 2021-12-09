using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_9_Smoke_Basin
{
    internal class Program
    {

        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<List<Location>> map = new List<List<Location>>();
            int y = 0;
            foreach (string mapLine in inputList)
            {
                int x = 0;
                List<Location> l = new List<Location>();
                foreach (char c in mapLine)
                {
                    l.Add(new Location(int.Parse(c.ToString()), x, y));
                    x++;
                }
                map.Add(l);
                y++;
            }

            P1(map);
            P2(map);
        }

        static void P1(List<List<Location>> map)
        {
            int riskLevel = 0;
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    bool lowest = true;
                    int val = map[y][x].val;
                    if (val == 9)
                        lowest = false;
                    else if (x > 0 && map[y][x - 1].val < val)
                        lowest = false;
                    else if (x < map[y].Count - 1 && map[y][x + 1].val < val)
                        lowest = false;
                    else if (y > 0 && map[y - 1][x].val < val)
                        lowest = false;
                    else if (y < map.Count - 1 && map[y + 1][x].val < val)
                        lowest = false;

                    if (lowest)
                        riskLevel += val + 1;
                }
            }

            Console.WriteLine(riskLevel);
            Console.ReadLine();
        }

        static void P2(List<List<Location>> map)
        {
            List<List<Location>> basins = new List<List<Location>>();

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x].Search(map, basins) == -1 && map[y][x].val != 9)
                    {
                        int basinId = basins.Count;
                        List<Location> newBasin = new List<Location>();
                        newBasin.Add(map[y][x]);
                        map[y][x].basinId = basinId;
                        basins.Add(newBasin);
                    }
                }
            }

            var basinsBySize = basins.OrderByDescending(basin => basin.Count()).ToList();
            int product = basinsBySize[0].Count * basinsBySize[1].Count * basinsBySize[2].Count;
            Console.WriteLine(product);
            Console.ReadLine();
        }
    }

    public class Location
    {
        public int val;
        public int basinId = -1;


        public int x;
        public int y;

        public Location(int _val, int _x, int _y)
        {
            this.val = _val;
            x = _x;
            y = _y;
        }

        public int Search(List<List<Location>> map, List<List<Location>> basins, List<Location> visitedLocations = null)
        {
            if (visitedLocations == null)
                visitedLocations = new List<Location> { this };
            else
                visitedLocations.Add(this);

            if (val != 9 && basinId == -1)
            {
                if (x > 0 && !visitedLocations.Contains(map[y][x - 1]) && map[y][x - 1].val != 9)
                {
                    int tmp = map[y][x - 1].Search(map, basins, visitedLocations);
                    if (tmp != -1)
                        basinId = tmp;
                }
                if (x < map[y].Count - 1 && !visitedLocations.Contains(map[y][x + 1]) && map[y][x + 1].val != 9)
                {
                    int tmp = map[y][x + 1].Search(map, basins, visitedLocations);
                    if (tmp != -1)
                        basinId = tmp;
                }
                if (y > 0 && !visitedLocations.Contains(map[y - 1][x]) && map[y - 1][x].val != 9)
                {
                    int tmp = map[y - 1][x].Search(map, basins, visitedLocations);
                    if (tmp != -1)
                        basinId = tmp;
                }
                if (y < map.Count - 1 && !visitedLocations.Contains(map[y + 1][x]) && map[y + 1][x].val != 9)
                {
                    int tmp = map[y + 1][x].Search(map, basins, visitedLocations);
                    if (tmp != -1)
                        basinId = tmp;
                }
            }

            if (basinId != -1 && !basins[basinId].Contains(this))
            {
                basins[basinId].Add(this);
            }

            return basinId;
        }

        public override string ToString()
        {
            return $"[{x},{y}]";
        }
    }
}
