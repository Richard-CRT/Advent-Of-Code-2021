using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_11_Dumbo_Octopus
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Octopus[][] cavern = new Octopus[10][];
            for (int i = 0; i < cavern.Length; i++)
            {
                cavern[i] = new Octopus[10];
            }

            List<string> inputList = AoCUtilities.GetInputLines();

            for (int y = 0; y < cavern.Length; y++)
            {
                for (int x = 0; x < cavern[y].Length; x++)
                {
                    cavern[y][x] = new Octopus(int.Parse(inputList[y][x].ToString()),x,y);
                }
            }

            P1(cavern);
            P2(cavern);
        }

        static void P1(Octopus[][] cavern)
        {
            int flashCount = 0;
            for (int i = 0;i < 100;i++)
            {
                for (int y = 0; y < cavern.Length; y++)
                {
                    for (int x = 0; x < cavern[y].Length; x++)
                    {
                        cavern[y][x].Energy++;
                    }
                }

                for (int y = 0; y < cavern.Length; y++)
                {
                    for (int x = 0; x < cavern[y].Length; x++)
                    {
                        cavern[y][x].CheckFlash(cavern);
                    }
                }

                for (int y = 0; y < cavern.Length; y++)
                {
                    for (int x = 0; x < cavern[y].Length; x++)
                    {
                        if (cavern[y][x].Flashed)
                        {
                            cavern[y][x].Flashed = false;
                            cavern[y][x].Energy = 0;
                            flashCount++;
                        }
                    }
                }
            }

            Console.WriteLine(flashCount);
            Console.ReadLine();
        }

        static void P2(Octopus[][] cavern)
        {
            int cycleCount = 100;
            int flashCount = 0;
            while (flashCount != 100)
            {
                flashCount = 0;
                for (int y = 0; y < cavern.Length; y++)
                {
                    for (int x = 0; x < cavern[y].Length; x++)
                    {
                        cavern[y][x].Energy++;
                    }
                }

                for (int y = 0; y < cavern.Length; y++)
                {
                    for (int x = 0; x < cavern[y].Length; x++)
                    {
                        cavern[y][x].CheckFlash(cavern);
                    }
                }

                for (int y = 0; y < cavern.Length; y++)
                {
                    for (int x = 0; x < cavern[y].Length; x++)
                    {
                        if (cavern[y][x].Flashed)
                        {
                            cavern[y][x].Flashed = false;
                            cavern[y][x].Energy = 0;
                            flashCount++;
                        }
                    }
                }
                cycleCount++;
            }

            Console.WriteLine(cycleCount);
            Console.ReadLine();
        }

        static void PrintCavern(Octopus[][] cavern)
        {
            for (int y = 0; y < cavern.Length; y++)
            {
                for (int x = 0; x < cavern[y].Length; x++)
                {
                    Console.Write(cavern[y][x]);
                }
                Console.WriteLine();
            }
        }
    }

    public class Octopus
    {
        public int Energy;
        public int X;
        public int Y;
        public bool Flashed = false;
        public Octopus(int energy, int x, int y)
        {
            Energy = energy;
            X = x;
            Y = y;
        }

        public void CheckFlash(Octopus[][] cavern)
        {
            if (!Flashed && Energy > 9)
            {
                Flashed = true;
                if (X > 0)
                {
                    cavern[Y][X - 1].Energy++;
                    cavern[Y][X - 1].CheckFlash(cavern);

                    if (Y > 0)
                    {
                        cavern[Y - 1][X - 1].Energy++;
                        cavern[Y - 1][X - 1].CheckFlash(cavern);
                    }
                    if (Y < cavern.Length - 1)
                    {
                        cavern[Y + 1][X - 1].Energy++;
                        cavern[Y + 1][X - 1].CheckFlash(cavern);
                    }
                }
                if (X < cavern[Y].Length - 1)
                {
                    cavern[Y][X + 1].Energy++;
                    cavern[Y][X + 1].CheckFlash(cavern);

                    if (Y > 0)
                    {
                        cavern[Y - 1][X + 1].Energy++;
                        cavern[Y - 1][X + 1].CheckFlash(cavern);
                    }
                    if (Y < cavern.Length - 1)
                    {
                        cavern[Y + 1][X + 1].Energy++;
                        cavern[Y + 1][X + 1].CheckFlash(cavern);
                    }
                }
                if (Y > 0)
                {
                    cavern[Y - 1][X].Energy++;
                    cavern[Y - 1][X].CheckFlash(cavern);
                }
                if (Y < cavern.Length - 1)
                {
                    cavern[Y + 1][X].Energy++;
                    cavern[Y + 1][X].CheckFlash(cavern);
                }
            }
        }

        public override string ToString()
        {
            return $"[{Energy}]";
        }
    }
}
