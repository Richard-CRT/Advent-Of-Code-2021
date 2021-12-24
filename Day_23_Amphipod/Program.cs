using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_23_Amphipod
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            State originalState = new State();

            for (int y = inputList.Count - 1; y >= 0; y--)
            {
                for (int x = 0; x < inputList[y].Length; x++)
                {
                    if (y >= 2 && y <= 3)
                    {
                        if (x == 3)
                            originalState.ARoom.Add(inputList[y][x]);
                        else if (x == 5)
                            originalState.BRoom.Add(inputList[y][x]);
                        else if (x == 7)
                            originalState.CRoom.Add(inputList[y][x]);
                        else if (x == 9)
                            originalState.DRoom.Add(inputList[y][x]);
                    }
                }
            }
            State.cache[originalState.UniqueStringRep()] = originalState;

            originalState.Print();
            originalState.GenerateAdjacentStates();
            Console.WriteLine(State.cacheHits);
            Console.ReadLine();
        }
    }

    public class State
    {
        public static Dictionary<string, State> cache = new Dictionary<string, State>();
        public static int cacheHits = 0;
        public static State cacheCheck(State state)
        {
            string cacheKey = state.UniqueStringRep();
            if (cache.ContainsKey(cacheKey))
            {
                state = cache[cacheKey];
                Console.WriteLine($"CACHE HIT FOR {cacheKey}");
                cacheHits++;
            }
            else
                cache[cacheKey] = state;
            return state;
        }

        public List<(int, State)> AdjacentStates = new List<(int, State)>();

        public List<char> Hallway1 = new List<char>();
        public char Hallway2 = '.';
        public char Hallway3 = '.';
        public char Hallway4 = '.';
        public List<char> Hallway5 = new List<char>();

        public List<char> ARoom = new List<char>();
        public List<char> BRoom = new List<char>();
        public List<char> CRoom = new List<char>();
        public List<char> DRoom = new List<char>();

        public State()
        {

        }

        public State(State otherState)
        {
            Hallway1 = new List<char>(otherState.Hallway1);
            Hallway2 = otherState.Hallway2;
            Hallway3 = otherState.Hallway3;
            Hallway4 = otherState.Hallway4;
            Hallway5 = new List<char>(otherState.Hallway5);

            ARoom = new List<char>(otherState.ARoom);
            BRoom = new List<char>(otherState.BRoom);
            CRoom = new List<char>(otherState.CRoom);
            DRoom = new List<char>(otherState.DRoom);
        }

        public void GenerateAdjacentStates()
        {
            int roomIndex;
            char amphipod;
            int energyPerStep;
            int steps;
            int energy;
            State newState;

            // This should generate all Amphipods moving out of room

            if (ARoom.Count > 0)
            {
                bool allCorrectAmphipod = true;
                foreach (char a in ARoom)
                {
                    if (a != 'A')
                    {
                        allCorrectAmphipod = false;
                        break;
                    }
                }

                if (!allCorrectAmphipod)
                {
                    roomIndex = ARoom.Count - 1;
                    amphipod = ARoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway1.Count < 2)
                    {
                        newState = new State(this);

                        steps = (2 - ARoom.Count) + 1 + (2 - Hallway1.Count);
                        newState.Hallway1.Add(amphipod);
                        newState.ARoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway2 == '.')
                    {
                        newState = new State(this);

                        steps = (2 - ARoom.Count) + 1 + 1;
                        newState.Hallway2 = amphipod;
                        newState.ARoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway3 == '.')
                        {
                            newState = new State(this);

                            steps = (2 - ARoom.Count) + 1 + 3;
                            newState.Hallway3 = amphipod;
                            newState.ARoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway4 == '.')
                            {
                                newState = new State(this);

                                steps = (2 - ARoom.Count) + 1 + 5;
                                newState.Hallway4 = amphipod;
                                newState.ARoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));

                                if (Hallway5.Count < 2)
                                {
                                    newState = new State(this);

                                    steps = (2 - ARoom.Count) + 1 + 6 + (2 - Hallway5.Count);
                                    newState.Hallway5.Add(amphipod);
                                    newState.ARoom.RemoveAt(roomIndex);

                                    energy = steps * energyPerStep;

                                    newState = cacheCheck(newState);
                                    AdjacentStates.Add((energy, newState));
                                }
                            }
                        }
                    }
                }
            }

            if (BRoom.Count > 0)
            {
                bool allCorrectAmphipod = true;
                foreach (char a in BRoom)
                {
                    if (a != 'B')
                    {
                        allCorrectAmphipod = false;
                        break;
                    }
                }

                if (!allCorrectAmphipod)
                {
                    roomIndex = BRoom.Count - 1;
                    amphipod = BRoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway2 == '.')
                    {
                        newState = new State(this);

                        steps = (2 - BRoom.Count) + 1 + 1;
                        newState.Hallway2 = amphipod;
                        newState.BRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway1.Count < 2)
                        {
                            newState = new State(this);

                            steps = (2 - BRoom.Count) + 1 + 2 + (2 - Hallway1.Count);
                            newState.Hallway1.Add(amphipod);
                            newState.BRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }
                    }

                    if (Hallway3 == '.')
                    {
                        newState = new State(this);

                        steps = (2 - BRoom.Count) + 1 + 1;
                        newState.Hallway3 = amphipod;
                        newState.BRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;
                        AdjacentStates.Add((energy, newState));

                        if (Hallway4 == '.')
                        {
                            newState = new State(this);

                            steps = (2 - BRoom.Count) + 1 + 3;
                            newState.Hallway4 = amphipod;
                            newState.BRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway5.Count < 2)
                            {
                                newState = new State(this);

                                steps = (2 - BRoom.Count) + 1 + 4 + (2 - Hallway5.Count);
                                newState.Hallway5.Add(amphipod);
                                newState.BRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }
                }
            }

            if (CRoom.Count > 0)
            {
                bool allCorrectAmphipod = true;
                foreach (char a in CRoom)
                {
                    if (a != 'C')
                    {
                        allCorrectAmphipod = false;
                        break;
                    }
                }

                if (!allCorrectAmphipod)
                {
                    roomIndex = CRoom.Count - 1;
                    amphipod = CRoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway3 == '.')
                    {
                        newState = new State(this);

                        steps = (2 - CRoom.Count) + 1 + 1;
                        newState.Hallway3 = amphipod;
                        newState.CRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway2 == '.')
                        {
                            newState = new State(this);

                            steps = (2 - CRoom.Count) + 1 + 3;
                            newState.Hallway2 = amphipod;
                            newState.CRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway1.Count < 2)
                            {
                                newState = new State(this);

                                steps = (2 - CRoom.Count) + 1 + 4 + (2 - Hallway1.Count);
                                newState.Hallway1.Add(amphipod);
                                newState.CRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }

                    if (Hallway4 == '.')
                    {
                        newState = new State(this);

                        steps = (2 - CRoom.Count) + 1 + 1;
                        newState.Hallway4 = amphipod;
                        newState.CRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway5.Count < 2)
                        {
                            newState = new State(this);

                            steps = (2 - CRoom.Count) + 1 + 2 + (2 - Hallway5.Count);
                            newState.Hallway5.Add(amphipod);
                            newState.CRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }
                    }
                }
            }

            if (DRoom.Count > 0)
            {
                bool allCorrectAmphipod = true;
                foreach (char a in DRoom)
                {
                    if (a != 'D')
                    {
                        allCorrectAmphipod = false;
                        break;
                    }
                }

                if (!allCorrectAmphipod)
                {
                    roomIndex = DRoom.Count - 1;
                    amphipod = DRoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway4 == '.')
                    {
                        newState = new State(this);

                        steps = (2 - DRoom.Count) + 1 + 1;
                        newState.Hallway4 = amphipod;
                        newState.DRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway3 == '.')
                        {
                            newState = new State(this);

                            steps = (2 - DRoom.Count) + 1 + 3;
                            newState.Hallway3 = amphipod;
                            newState.DRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway2 == '.')
                            {
                                newState = new State(this);

                                steps = (2 - DRoom.Count) + 1 + 5;
                                newState.Hallway2 = amphipod;
                                newState.DRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));

                                if (Hallway1.Count < 2)
                                {
                                    newState = new State(this);

                                    steps = (2 - DRoom.Count) + 1 + 6 + (2 - Hallway1.Count);
                                    newState.Hallway1.Add(amphipod);
                                    newState.DRoom.RemoveAt(roomIndex);

                                    energy = steps * energyPerStep;

                                    newState = cacheCheck(newState);
                                    AdjacentStates.Add((energy, newState));
                                }
                            }
                        }
                    }

                    if (Hallway5.Count < 2)
                    {
                        newState = new State(this);

                        steps = (2 - DRoom.Count) + 1 + (2 - Hallway5.Count);
                        newState.Hallway5.Add(amphipod);
                        newState.DRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }
                }
            }

            // Still to generate moving of Amphipods into hallway

            foreach (var (e, s) in AdjacentStates)
            {
                Console.WriteLine($"Energy required for following state: {e}");
                s.Print();
            }

            foreach (var (e, s) in AdjacentStates)
            {
                s.GenerateAdjacentStates();
            }
        }

        public static int AmphipodToEnergyPerStep(char amphipod)
        {
            switch (amphipod)
            {
                case 'A':
                    return 1;
                case 'B':
                    return 10;
                case 'C':
                    return 100;
                case 'D':
                    return 1000;
            }
            throw new Exception();
        }

        public string UniqueStringRep()
        {
            string strRep = "";

            if (Hallway1.Count == 0)
                strRep += "..";
            else if (Hallway1.Count == 1)
                strRep += $"{Hallway1[0]}.";
            else if (Hallway1.Count == 2)
                strRep += $"{Hallway1[0]}{Hallway1[1]}";
            else throw new Exception();

            for (int i = 0; i < 2; i++)
            {
                if (ARoom.Count > i)
                    strRep += ARoom[i];
                else
                    strRep += ".";
            }

            strRep += Hallway2;

            for (int i = 0; i < 2; i++)
            {
                if (BRoom.Count > i)
                    strRep += BRoom[i];
                else
                    strRep += ".";
            }

            strRep += Hallway3;

            for (int i = 0; i < 2; i++)
            {
                if (CRoom.Count > i)
                    strRep += CRoom[i];
                else
                    strRep += ".";
            }

            strRep += Hallway4;

            for (int i = 0; i < 2; i++)
            {
                if (DRoom.Count > i)
                    strRep += DRoom[i];
                else
                    strRep += ".";
            }

            if (Hallway5.Count == 0)
                strRep += "..";
            else if (Hallway5.Count == 1)
                strRep += $".{Hallway5[0]}.";
            else if (Hallway5.Count == 2)
                strRep += $"{Hallway5[1]}{Hallway5[0]}";
            else throw new Exception();

            return strRep;
        }

        public void Print()
        {
            Console.WriteLine($"Cache Key:");
            Console.WriteLine(UniqueStringRep());

            Console.WriteLine("State:");

            if (Hallway1.Count == 0)
                Console.Write("..");
            else if (Hallway1.Count == 1)
                Console.Write($"{Hallway1[0]}.");
            else if (Hallway1.Count == 2)
                Console.Write($"{Hallway1[0]}{Hallway1[1]}");

            Console.Write($"+{Hallway2}+{Hallway3}+{Hallway4}+");

            if (Hallway5.Count == 0)
                Console.Write("..");
            else if (Hallway5.Count == 1)
                Console.Write($".{Hallway5[0]}");
            else if (Hallway5.Count == 2)
                Console.Write($"{Hallway5[1]}{Hallway5[0]}");

            Console.WriteLine();

            for (int i = 1; i >= 0; i--)
            {
                Console.Write("  ");
                if (ARoom.Count > i)
                    Console.Write(ARoom[i]);
                else
                    Console.Write('.');
                Console.Write(" ");
                if (BRoom.Count > i)
                    Console.Write(BRoom[i]);
                else
                    Console.Write('.');
                Console.Write(" ");
                if (CRoom.Count > i)
                    Console.Write(CRoom[i]);
                else
                    Console.Write('.');
                Console.Write(" ");
                if (DRoom.Count > i)
                    Console.Write(DRoom[i]);
                else
                    Console.Write('.');

                Console.WriteLine();
            }
        }
    }
}
