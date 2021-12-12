using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_12_Passage_Pathing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Node> nodeByName = new Dictionary<string, Node>();

            List<string> inputList = AoCUtilities.GetInputLines();

            foreach (string line in inputList)
            {
                string[] split = line.Split('-');
                if (!nodeByName.ContainsKey(split[0]))
                    nodeByName[split[0]] = new Node(split[0]);
                if (!nodeByName.ContainsKey(split[1]))
                    nodeByName[split[1]] = new Node(split[1]);

                nodeByName[split[0]].ConnectedNodes.Add(nodeByName[split[1]]);
                nodeByName[split[1]].ConnectedNodes.Add(nodeByName[split[0]]);
            }

            P1(nodeByName);
            P2(nodeByName);
        }

        static void P1(Dictionary<string, Node> nodeByName)
        {
            List<List<Node>> paths = nodeByName["start"].TraverseP1(new List<Node>());

            Console.WriteLine(paths.Count);
            Console.ReadLine();
        }

        static void P2(Dictionary<string, Node> nodeByName)
        {
            List<List<Node>> paths = nodeByName["start"].TraverseP2(new List<Node>());

            /*
            foreach (List<Node> p in paths)
            {
                PrintPath(p);
            }
            */

            Console.WriteLine(paths.Count);
            Console.ReadLine();
        }

        static void PrintPath(List<Node> path)
        {
            foreach (Node node in path)
                Console.Write($"{node.Name},");
            Console.WriteLine();
        }
    }

    public class Node
    {
        public string Name = "";
        public bool Big = false;
        public List<Node> ConnectedNodes = new List<Node>();

        public Node(string name)
        {
            Name = name;
            Big = (name[0] >= 0x41 && name[0] <= 0x5A);
        }

        public List<List<Node>> TraverseP1(List<Node> path)
        {
            List<List<Node>> pathsFromThisNode = new List<List<Node>>();

            if (!path.Contains(this) || this.Big)
            {
                // path is allowed
                path.Add(this);
                if (this.Name == "end")
                {
                    // end of path
                    pathsFromThisNode.Add(new List<Node> { this });
                }
                else
                {
                    // traverse connnected nodes and look for true result
                    foreach (Node node in this.ConnectedNodes)
                    {
                        List<Node> subPath = new List<Node>(path);
                        List<List<Node>> pathsFromConnectedNode = node.TraverseP1(subPath);
                        foreach (List<Node> pathFromConnectedNode in pathsFromConnectedNode)
                        {
                            pathFromConnectedNode.Insert(0, this);
                            pathsFromThisNode.Add(pathFromConnectedNode);
                        }
                    }
                }
            }
            else
            {
                // small and already in path
            }

            return pathsFromThisNode;
        }

        public List<List<Node>> TraverseP2(List<Node> path)
        {
            List<List<Node>> pathsFromThisNode = new List<List<Node>>();

            // this is very inefficient
            // better would be to have the path store a bool indicating that a small cave has already been duplicated
            List<Node> duplicateNodes = path.GroupBy(x => x).Where(g => g.Count() > 1).Select(y => y.Key).ToList();
            bool smallCaveAlreadyDuplicated = false;
            foreach (Node duplicateNode in duplicateNodes)
            {
                if (!duplicateNode.Big)
                    smallCaveAlreadyDuplicated = true;
            }

            if ((smallCaveAlreadyDuplicated == false && this.Name != "start") || !path.Contains(this) || this.Big)
            {
                // path is allowed
                path.Add(this);
                if (this.Name == "end")
                {
                    // end of path
                    pathsFromThisNode.Add(new List<Node> { this });
                }
                else
                {
                    // traverse connnected nodes and look for true result
                    foreach (Node node in this.ConnectedNodes)
                    {
                        List<Node> subPath = new List<Node>(path);
                        List<List<Node>> pathsFromConnectedNode = node.TraverseP2(subPath);
                        foreach (List<Node> pathFromConnectedNode in pathsFromConnectedNode)
                        {
                            pathFromConnectedNode.Insert(0, this);
                            pathsFromThisNode.Add(pathFromConnectedNode);
                        }
                    }
                }
            }
            else
            {
                // small and already in path
            }

            return pathsFromThisNode;
        }

        public override string ToString()
        {
            return $"[Node - {Name} - {ConnectedNodes.Count} connected nodes]";
        }
    }
}
