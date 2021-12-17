using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_17_Trick_Shot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            string subStr = inputList[0].Substring(15);
            string[] subStrSplit = subStr.Split(',');
            string xRngStr = subStrSplit[0];
            string yRngStr = subStrSplit[1].Substring(3);


            string[] xRngStrSplit = xRngStr.Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
            string[] yRngStrSplit = yRngStr.Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);

            int xLow = Convert.ToInt32(xRngStrSplit[0]);
            int xHigh = Convert.ToInt32(xRngStrSplit[1]);
            int yLow = Convert.ToInt32(yRngStrSplit[0]);
            int yHigh = Convert.ToInt32(yRngStrSplit[1]);

            P1(xLow, xHigh, yLow, yHigh);
            P2(xLow, xHigh, yLow, yHigh);
        }

        static void P1(int xLow, int xHigh, int yLow, int yHigh)
        {
            int minX = 1;
            int maxX = xHigh; // Higher than this will overshoot
            int minY = 1; // Trickshot will only go up
            int maxY = 100; // Guess :)

            int highestY = 0;
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Path newPath = new Path(x, y);
                    var data = newPath.Simulate(xLow, xHigh, yLow, yHigh);
                    if (data.Item1 && data.Item2 > highestY)
                        highestY = data.Item2;
                }
            }

            Console.WriteLine(highestY);
            Console.ReadLine();
        }

        static void P2(int xLow, int xHigh, int yLow, int yHigh)
        {
            int minX = 1;
            int maxX = xHigh; // Higher than this will overshoot
            int minY = yLow; // Lower than this will overshoot
            int maxY = 100; // Guess :)

            int numberOfSolutions = 0;
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    Path newPath = new Path(x, y);
                    var data = newPath.Simulate(xLow, xHigh, yLow, yHigh);
                    if (data.Item1)
                        numberOfSolutions++;
                }
            }

            Console.WriteLine(numberOfSolutions);
            Console.ReadLine();
        }
    }

    public class Path
    {
        public List<Tuple<int, int>> Positions = new List<Tuple<int, int>>();
        public int InitVelX;
        public int InitVelY;

        public Path(int initVelX, int initVelY)
        {
            InitVelX = initVelX;
            InitVelY = initVelY;
        }

        public (bool, int) Simulate(int xLow, int xHigh, int yLow, int yHigh)
        {
            int posX = 0;
            int posY = 0;
            Positions.Add(new Tuple<int, int>(posX, posY));

            int highestY = posY;

            int velX = InitVelX;
            int velY = InitVelY;

            bool inTargetZone = false;
            while (!inTargetZone && posY >= yLow)
            {
                posX += velX;
                posY += velY;
                if (posY > highestY)
                    highestY = posY;
                Positions.Add(new Tuple<int, int>(posX, posY));
                inTargetZone = posX >= xLow && posX <= xHigh && posY >= yLow && posY <= yHigh;

                velX = velX == 0 ? 0 : velX > 0 ? velX - 1 : velX + 1;
                velY--;
            }

            //if (inTargetZone)
            //    this.Display(xLow, xHigh, yLow, yHigh);

            return (inTargetZone, highestY);
        }

        public void Display(int xLow, int xHigh, int yLow, int yHigh)
        {
            int minX = int.MaxValue;
            int maxX = xHigh;
            int minY = yLow;
            int maxY = int.MinValue;

            foreach (Tuple<int, int> pos in Positions)
            {
                if (pos.Item1 < minX)
                    minX = pos.Item1;
                if (pos.Item1 > maxX)
                    maxX = pos.Item1;
                if (pos.Item2 < minY)
                    minY = pos.Item2;
                if (pos.Item2 > maxY)
                    maxY = pos.Item2;
            }

            for (int y = maxY; y >= minY; y--)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    foreach (var pos in Positions)
                    {
                        if (pos.Item1 == x && pos.Item2 == y)
                            Console.Write("#");
                        else if (x >= xLow && x <= xHigh && y >= yLow && y <= yHigh)
                            Console.Write("T");
                        else
                            Console.Write(".");
                    }
                }
                Console.WriteLine();
            }
            Console.ReadLine();
        }
    }
}
