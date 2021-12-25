using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_25_Sea_Cucumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<SeaCucumber> eastHerd = new List<SeaCucumber>();
            List<SeaCucumber> southHerd = new List<SeaCucumber>();

            List<List<SeaCucumber>> map = new List<List<SeaCucumber>>();

            for (int y = 0; y < inputList.Count;y++)
            {
                var newRow = new List<SeaCucumber>();
                for (int x = 0; x < inputList[y].Length;x++)
                {
                    if (inputList[y][x] == 'v')
                    {
                        var newSeaCucumber = new SeaCucumber(x, y, false);
                        southHerd.Add(newSeaCucumber);
                        newRow.Add(newSeaCucumber);
                    }
                    else if (inputList[y][x] == '>')
                    {
                        var newSeaCucumber = new SeaCucumber(x, y,true);
                        eastHerd.Add(newSeaCucumber);
                        newRow.Add(newSeaCucumber);
                    }
                    else
                    {
                        newRow.Add(null);
                    }
                }
                map.Add(newRow);
            }

            P1(map, eastHerd, southHerd);
        }

        static void P1(List<List<SeaCucumber>> map, List<SeaCucumber> eastHerd, List<SeaCucumber> southHerd)
        {
            bool changeMade = true;
            int step = 0;
            while (changeMade)
            {
                changeMade = false;
                //Print(map);

                foreach (SeaCucumber seaCucumber in eastHerd)
                {
                    int newX;
                    if (seaCucumber.X < map[seaCucumber.Y].Count - 1)
                        newX = seaCucumber.X + 1;
                    else
                        newX = 0;
                    seaCucumber.GoingToMove = false;
                    if (map[seaCucumber.Y][newX] == null)
                        seaCucumber.GoingToMove = true;
                }
                foreach (SeaCucumber seaCucumber in eastHerd)
                {
                    if (seaCucumber.GoingToMove)
                    {
                        changeMade = true;
                        int newX;
                        if (seaCucumber.X < map[seaCucumber.Y].Count - 1)
                            newX = seaCucumber.X + 1;
                        else
                            newX = 0;
                        map[seaCucumber.Y][seaCucumber.X] = null;
                        seaCucumber.X = newX;
                        map[seaCucumber.Y][seaCucumber.X] = seaCucumber;
                    }
                }


                foreach (SeaCucumber seaCucumber in southHerd)
                {
                    int newY;
                    if (seaCucumber.Y < map.Count - 1)
                        newY = seaCucumber.Y + 1;
                    else
                        newY = 0;
                    seaCucumber.GoingToMove = false;
                    if (map[newY][seaCucumber.X] == null)
                        seaCucumber.GoingToMove = true;
                }
                foreach (SeaCucumber seaCucumber in southHerd)
                {
                    if (seaCucumber.GoingToMove)
                    {
                        changeMade = true;
                        int newY;
                        if (seaCucumber.Y < map.Count - 1)
                            newY = seaCucumber.Y + 1;
                        else
                            newY = 0;
                        map[seaCucumber.Y][seaCucumber.X] = null;
                        seaCucumber.Y = newY;
                        map[seaCucumber.Y][seaCucumber.X] = seaCucumber;
                    }
                }

                step++;
            }

            Console.WriteLine(step);
            Console.ReadLine();
        }

        static void Print(List<List<SeaCucumber>> map)
        {
            for (int y = 0; y < map.Count;y++)
            {
                for(int x=0;x<map[y].Count;x++)
                {
                    if (map[y][x] != null)
                    {
                        if (map[y][x].East)
                            Console.Write(">");
                        else
                            Console.Write("v");
                    }
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }

    public class SeaCucumber
    {
        public bool GoingToMove = false;
        public int X;
        public int Y;
        public bool East;

        public SeaCucumber(int x, int y, bool east)
        {
            X = x;
            Y = y;
            East = east;
        }
    }
}
