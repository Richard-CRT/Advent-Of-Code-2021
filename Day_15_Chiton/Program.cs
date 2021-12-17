using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_15_Chiton
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            int width = inputList[0].Length;
            int height = inputList.Count;

            Node[][] pathRiskMapP1 = new Node[height][];
            for (int y = 0; y < pathRiskMapP1.Length; y++)
            {
                pathRiskMapP1[y] = new Node[width];
                for (int x = 0; x < pathRiskMapP1[y].Length; x++)
                {
                    pathRiskMapP1[y][x] = new Node(x, y, int.Parse(inputList[y][x].ToString()));
                }
            }

            Node[][] pathRiskMapP2 = new Node[height * 5][];
            for (int y = 0; y < pathRiskMapP2.Length; y++)
            {
                pathRiskMapP2[y] = new Node[width * 5];
                for (int x = 0; x < pathRiskMapP2[y].Length; x++)
                {
                    int val = int.Parse(inputList[y % pathRiskMapP1.Length][x % pathRiskMapP1[y % pathRiskMapP1.Length].Length].ToString()) +
                        (y / pathRiskMapP1.Length) +
                        (x / pathRiskMapP1[y % pathRiskMapP1.Length].Length);
                    while (val > 9)
                        val -= 9;

                    pathRiskMapP2[y][x] = new Node(x, y, val);
                }
            }

            P1(pathRiskMapP1);
            P2(pathRiskMapP2);
        }

        static void P1(Node[][] pathRiskMap)
        {
            PathFind(pathRiskMap);
            Console.WriteLine(pathRiskMap[pathRiskMap.Length - 1][pathRiskMap[pathRiskMap.Length - 1].Length - 1].PathRisk);
            Console.ReadLine();
        }

        static void P2(Node[][] pathRiskMap)
        {
            PathFind(pathRiskMap);
            Console.WriteLine(pathRiskMap[pathRiskMap.Length - 1][pathRiskMap[pathRiskMap.Length - 1].Length - 1].PathRisk);
            Console.ReadLine();
        }

        static void PathFind(Node[][] pathRiskMap)
        {
            List<Node> inProgressNodes = new List<Node>();
            inProgressNodes.Add(pathRiskMap[0][0]);
            pathRiskMap[0][0].PathRisk = 0;

            while (inProgressNodes.Count > 0)
            {
                Node minPathRiskNode = null;
                foreach (Node inProgressNode in inProgressNodes)
                {
                    if (minPathRiskNode == null || inProgressNode.PathRisk < minPathRiskNode.PathRisk)
                        minPathRiskNode = inProgressNode;
                }

                minPathRiskNode.Finalised = true;
                inProgressNodes.Remove(minPathRiskNode);

                if (minPathRiskNode.X > 0 && !pathRiskMap[minPathRiskNode.Y][minPathRiskNode.X - 1].Finalised)
                {
                    Node adjacentNode = pathRiskMap[minPathRiskNode.Y][minPathRiskNode.X - 1];
                    if (adjacentNode.PathRisk == int.MaxValue)
                        inProgressNodes.Add(adjacentNode);
                    int newPathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                    if (newPathRisk < adjacentNode.PathRisk)
                        adjacentNode.PathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                }
                if (minPathRiskNode.X < pathRiskMap[minPathRiskNode.Y].Length - 1 && !pathRiskMap[minPathRiskNode.Y][minPathRiskNode.X + 1].Finalised)
                {
                    Node adjacentNode = pathRiskMap[minPathRiskNode.Y][minPathRiskNode.X + 1];
                    if (adjacentNode.PathRisk == int.MaxValue)
                        inProgressNodes.Add(adjacentNode);
                    int newPathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                    if (newPathRisk < adjacentNode.PathRisk)
                        adjacentNode.PathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                }
                if (minPathRiskNode.Y > 0 && !pathRiskMap[minPathRiskNode.Y - 1][minPathRiskNode.X].Finalised)
                {
                    Node adjacentNode = pathRiskMap[minPathRiskNode.Y - 1][minPathRiskNode.X];
                    if (adjacentNode.PathRisk == int.MaxValue)
                        inProgressNodes.Add(adjacentNode);
                    int newPathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                    if (newPathRisk < adjacentNode.PathRisk)
                        adjacentNode.PathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                }
                if (minPathRiskNode.Y < pathRiskMap.Length - 1 && !pathRiskMap[minPathRiskNode.Y + 1][minPathRiskNode.X].Finalised)
                {
                    Node adjacentNode = pathRiskMap[minPathRiskNode.Y + 1][minPathRiskNode.X];
                    if (adjacentNode.PathRisk == int.MaxValue)
                        inProgressNodes.Add(adjacentNode);
                    int newPathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                    if (newPathRisk < adjacentNode.PathRisk)
                        adjacentNode.PathRisk = minPathRiskNode.PathRisk + adjacentNode.Risk;
                }
            }
        }
    }

    public class Node
    {
        public int X;
        public int Y;
        public int Risk;
        public int PathRisk = int.MaxValue;
        public bool Finalised = false;

        public Node(int x, int y, int risk)
        {
            X = x;
            Y = y;
            Risk = risk;
        }

        public override string ToString()
        {
            return $"{X},{Y}";
        }
    }
}
