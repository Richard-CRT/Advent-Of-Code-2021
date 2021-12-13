using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_13_Transparent_Origami
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            Dictionary<Tuple<int,int>, Dot> dotsByCoord = new Dictionary<Tuple<int,int>,Dot>();
            List<Fold> folds = new List<Fold>();

            int i = 0;
            while (inputList[i] != "")
            {
                Dot newDot = new Dot(inputList[i]);
                dotsByCoord[new Tuple<int,int>(newDot.X, newDot.Y)] = newDot;

                i++;
            }
            i++;
            while (i < inputList.Count && inputList[i] != "")
            {
                folds.Add(new Fold(inputList[i]));

                i++;
            }

            P1(dotsByCoord, folds[0]);
            P2(dotsByCoord, folds);
        }

        static void P1(Dictionary<Tuple<int, int>, Dot> dotsByCoord, Fold fold)
        {
            PerformFold(dotsByCoord, fold);

            Console.WriteLine(dotsByCoord.Count);
            Console.ReadLine();
        }

        static void P2(Dictionary<Tuple<int, int>, Dot> dotsByCoord, List<Fold> folds)
        {
            for (int i = 1; i < folds.Count; i++)
            {
                PerformFold(dotsByCoord, folds[i]);
            }

            int maxX = 0;
            int maxY = 0;
            foreach (var kVP in dotsByCoord)
            {
                if (kVP.Value.X > maxX)
                    maxX = kVP.Value.X;
                if (kVP.Value.Y > maxY)
                    maxY = kVP.Value.Y;
            }

            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    Tuple<int, int> coord = new Tuple<int, int>(x, y);
                    if (dotsByCoord.ContainsKey(coord))
                        Console.Write("██");
                    else
                        Console.Write("  ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();
        }

        static void PerformFold(Dictionary<Tuple<int, int>, Dot> dotsByCoord, Fold fold)
        {
            List<Dot> tempDots = dotsByCoord.Values.ToList();
            foreach (Dot dot in tempDots)
            {
                dot.PerformFold(dotsByCoord, fold);
            }
        }
    }

    public class Fold
    {
        public bool Vertical;
        public int Coord;

        public Fold(string fold)
        {
            if (fold[11] == 'y')
                Vertical = false;
            else
                Vertical = true;

            Coord = int.Parse(fold.Substring(13));
        }

        public override string ToString()
        {
            if (this.Vertical)
            {
                return $"[Vert x={Coord}]";
            }
            else
            {
                return $"[Hori y={Coord}]";
            }
        }
    }

    public class Dot
    {
        public int X;
        public int Y;

        public Dot(string coord)
        {
            string[] split = coord.Split(',');
            X = int.Parse(split[0]);
            Y = int.Parse(split[1]);
        }

        public void PerformFold(Dictionary<Tuple<int, int>, Dot> dotsByCoord, Fold fold)
        {
            dotsByCoord.Remove(new Tuple<int, int>(X, Y));
            if (fold.Vertical)
            {
                if (X > fold.Coord)
                    X = fold.Coord - (X - fold.Coord);
            }
            else
            {
                if (Y > fold.Coord)
                    Y = fold.Coord - (Y - fold.Coord);
            }
            Tuple<int, int> newCoord = new Tuple<int, int>(X, Y);
            if (!dotsByCoord.ContainsKey(newCoord))
                dotsByCoord[newCoord] = this;
        }

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }
    }
}
