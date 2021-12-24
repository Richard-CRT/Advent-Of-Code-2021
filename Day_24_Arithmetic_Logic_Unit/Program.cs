using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Day_24_Arithmetic_Logic_Unit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();


            List<List<string>> blocks = new List<List<string>>();
            for (int blockStart = 0; blockStart < inputList.Count; blockStart += 18)
            {
                List<string> block = new List<string>();
                for (int i = blockStart; i < blockStart + 18; i++)
                {
                    block.Add(inputList[i]);
                }
                blocks.Add(block);
            }

            P1(blocks);
            P2(blocks);
        }

        static void P1(List<List<string>> blocks)
        {
            Dictionary<Int64, Int64> states = new Dictionary<Int64, Int64>();
            states[0] = 0;

            states = Execute(blocks, states, false);

            Console.WriteLine(states[0]);
            Console.ReadLine();
        }

        static void P2(List<List<string>> blocks)
        {
            Dictionary<Int64, Int64> states = new Dictionary<Int64, Int64>();
            states[0] = 0;

            states = Execute(blocks, states, true);

            Console.WriteLine(states[0]);
            Console.ReadLine();
        }

        static Dictionary<Int64, Int64> Execute(List<List<string>> blocks, Dictionary<Int64, Int64> states, bool P2)
        {
            int start = P2 ? 1 : 9;
            int target = P2 ? 9 : 1;
            int increment = P2 ? 1 : -1;
            foreach (List<string> block in blocks)
            {
                Dictionary<Int64, Int64> newStates = new Dictionary<Int64, Int64>();
                foreach (var kVP in states)
                {
                    for (int inVal = start; (P2 && inVal <= target) || (!P2 && inVal >= target); inVal += increment)
                    {
                        Int64 zVal = kVP.Key;
                        Int64 inputNumberSoFar = kVP.Value;

                        // Decompiled
                        int const1 = int.Parse(block[4].Split(' ')[2]);
                        int const2 = int.Parse(block[5].Split(' ')[2]);
                        int const3 = int.Parse(block[15].Split(' ')[2]);

                        Int64 tempVar = (zVal % 26) + const2;
                        zVal /= const1;
                        if (tempVar != inVal)
                        {
                            zVal = (zVal * 26) + inVal + const3;
                        }
                        inputNumberSoFar = inputNumberSoFar * 10 + inVal;

                        if (newStates.ContainsKey(zVal))
                        {
                            if ((!P2 && inputNumberSoFar > newStates[zVal]) || (P2 && inputNumberSoFar < newStates[zVal]))
                                newStates[zVal] = inputNumberSoFar;
                        }
                        else
                        {
                            newStates[zVal] = inputNumberSoFar;
                        }
                    }
                }
                states = newStates;

                //Console.WriteLine(states.Count);
            }

            return states;
        }
    }
}
