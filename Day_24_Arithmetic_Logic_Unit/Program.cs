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

            Console.WriteLine(System.Runtime.InteropServices.Marshal.SizeOf(typeof(State)));

            Dictionary<(int, int, int, int), State> states = new Dictionary<(int, int, int, int), State>();
            states[(0, 0, 0, 0)] = new State(0, 0, 0, 0, 0);



            foreach (string inst in inputList)
            {
                if (inst[1] == 'n') // input
                {
                    Dictionary<(int, int, int, int), State> newStates = new Dictionary<(int, int, int, int), State>();
                    foreach (var kVP in states)
                    {
                        for (int i = 9; i >= 1; i--)
                        {
                            int newInputNumber = (kVP.Value.inputNumber * 10) + i;
                            var stateKey = (i, kVP.Value.values[1], kVP.Value.values[2], kVP.Value.values[3]);
                            if (newStates.ContainsKey(stateKey))
                            {
                                if (newInputNumber > newStates[stateKey].inputNumber)
                                {
                                    newStates[stateKey] = new State(i, kVP.Value.values[1], kVP.Value.values[2], kVP.Value.values[3], newInputNumber);
                                }
                            }
                            else
                                newStates[stateKey] = new State(i, kVP.Value.values[1], kVP.Value.values[2], kVP.Value.values[3], newInputNumber);
                        }
                    }
                    states = newStates;
                    Console.WriteLine($"Processing {states.Count} states");
                }
                else
                {
                    string[] split = inst.Split(' ');
                    string op = split[0];
                    char op1 = split[1][0];
                    string op2 = split[2];
                    int op2Literal = -1;

                    int srcPos = -1;
                    if (op2[0] < 'w' || op2[0] > 'z')
                        op2Literal = int.Parse(op2);
                    else
                    {
                        switch (op2[0])
                        {
                            case 'w':
                                srcPos = 0;
                                break;
                            case 'x':
                                srcPos = 1;
                                break;
                            case 'y':
                                srcPos = 2;
                                break;
                            case 'z':
                                srcPos = 3;
                                break;
                        }
                    }

                    int destPos = 0;
                    switch (op1)
                    {
                        case 'w':
                            destPos = 0;
                            break;
                        case 'x':
                            destPos = 1;
                            break;
                        case 'y':
                            destPos = 2;
                            break;
                        case 'z':
                            destPos = 3;
                            break;
                    }

                    switch (op)
                    {
                        case "add":
                            foreach (var kVP in states)
                            {
                                kVP.Value.values[destPos] = kVP.Value.values[destPos] + (op2Literal != -1 ? op2Literal : kVP.Value.values[srcPos]);
                            }
                            break;
                        case "mul":
                            foreach (var kVP in states)
                            {
                                kVP.Value.values[destPos] = kVP.Value.values[destPos] * (op2Literal != -1 ? op2Literal : kVP.Value.values[srcPos]);
                            }
                            break;
                        case "div":
                            foreach (var kVP in states)
                            {
                                kVP.Value.values[destPos] = kVP.Value.values[destPos] / (op2Literal != -1 ? op2Literal : kVP.Value.values[srcPos]);
                            }
                            break;
                        case "mod":
                            foreach (var kVP in states)
                            {
                                kVP.Value.values[destPos] = kVP.Value.values[destPos] % (op2Literal != -1 ? op2Literal : kVP.Value.values[srcPos]);
                            }
                            break;
                        case "eql":
                            foreach (var kVP in states)
                            {
                                kVP.Value.values[destPos] = (kVP.Value.values[destPos] == (op2Literal != -1 ? op2Literal : kVP.Value.values[srcPos])) ? 1 : 0;
                            }
                            break;
                    }
                }
            }
        }
    }
    
    public struct State
    {
        public int[] values;

        public int inputNumber;

        public State(int _w, int _x, int _y, int _z, int _inputNumber)
        {
            values = new int[4];
            values[0] = _w;
            values[1] = _x;
            values[2] = _y;
            values[3] = _z;
            this.inputNumber = _inputNumber;
        }

        public override string ToString()
        {
            return $"{values[0]},{values[1]},{values[2]},{values[3]},{inputNumber}";
        }
    }
}
