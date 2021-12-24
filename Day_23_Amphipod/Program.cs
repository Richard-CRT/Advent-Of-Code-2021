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

            State originalStateP1 = new State();

            for (int y = inputList.Count - 1; y >= 0; y--)
            {
                for (int x = 0; x < inputList[y].Length; x++)
                {
                    if (y >= 2 && y <= 3)
                    {
                        if (x == 3)
                            originalStateP1.ARoom.Add(inputList[y][x]);
                        else if (x == 5)
                            originalStateP1.BRoom.Add(inputList[y][x]);
                        else if (x == 7)
                            originalStateP1.CRoom.Add(inputList[y][x]);
                        else if (x == 9)
                            originalStateP1.DRoom.Add(inputList[y][x]);
                    }
                }
            }
            State originalStateP2 = new State(originalStateP1);
            originalStateP2.P2 = true;
            State.cache[originalStateP1.UniqueStringRep()] = originalStateP1;

            P1(originalStateP1);

            originalStateP2.ARoom.Insert(1, 'D');
            originalStateP2.ARoom.Insert(1, 'D');
            originalStateP2.BRoom.Insert(1, 'C');
            originalStateP2.BRoom.Insert(1, 'B');
            originalStateP2.CRoom.Insert(1, 'B');
            originalStateP2.CRoom.Insert(1, 'A');
            originalStateP2.DRoom.Insert(1, 'A');
            originalStateP2.DRoom.Insert(1, 'C');
            State.cache = new Dictionary<string, State>();
            State.cache[originalStateP2.UniqueStringRep()] = originalStateP2;
            P2(originalStateP2);
        }

        static void P1(State originalState)
        {
            //originalState.Print();
            originalState.GenerateAdjacentStates();


            originalState.DijsktraTotalDistance = 0;
            List<State> inProgressNodes = new List<State>() { originalState };

            while (inProgressNodes.Count > 0)
            {
                int minTotalDistanceStateI = -1;
                for (int i = 0; i < inProgressNodes.Count; i++)
                {
                    State state = inProgressNodes[i];
                    if (minTotalDistanceStateI == -1 || state.DijsktraTotalDistance < inProgressNodes[minTotalDistanceStateI].DijsktraTotalDistance)
                        minTotalDistanceStateI = i;
                }

                if (minTotalDistanceStateI == -1)
                    throw new Exception();

                State minTotalDistanceState = inProgressNodes[minTotalDistanceStateI];
                inProgressNodes.RemoveAt(minTotalDistanceStateI);
                minTotalDistanceState.DijsktraInProgress = false;
                minTotalDistanceState.DijsktraExplored = true;

                foreach (var (energyToReachStateAdjacent, stateAdjacentToMinTotalDistanceState) in minTotalDistanceState.AdjacentStates)
                {
                    if (!stateAdjacentToMinTotalDistanceState.DijsktraExplored)
                    {
                        if (!stateAdjacentToMinTotalDistanceState.DijsktraInProgress)
                        {
                            inProgressNodes.Add(stateAdjacentToMinTotalDistanceState);
                            stateAdjacentToMinTotalDistanceState.DijsktraInProgress = true;
                        }

                        int trialDistance = minTotalDistanceState.DijsktraTotalDistance + energyToReachStateAdjacent;

                        if (trialDistance < stateAdjacentToMinTotalDistanceState.DijsktraTotalDistance)
                            stateAdjacentToMinTotalDistanceState.DijsktraTotalDistance = trialDistance;
                    }
                }
            }
            State finalState = State.cache["..AA.BB.CC.DD.."];
            Console.WriteLine(finalState.DijsktraTotalDistance);
            Console.ReadLine();
        }

        static void P2(State originalState)
        {
            //originalState.Print();
            originalState.GenerateAdjacentStates();
            //Console.WriteLine(State.cacheHits);
            //Console.WriteLine(State.cache.Count);


            originalState.DijsktraTotalDistance = 0;
            List<State> inProgressNodes = new List<State>() { originalState };
            //Console.WriteLine("Finished map generation");

            while (inProgressNodes.Count > 0)
            {
                int minTotalDistanceStateI = -1;
                for (int i = 0; i < inProgressNodes.Count; i++)
                {
                    State state = inProgressNodes[i];
                    if (minTotalDistanceStateI == -1 || state.DijsktraTotalDistance < inProgressNodes[minTotalDistanceStateI].DijsktraTotalDistance)
                        minTotalDistanceStateI = i;
                }

                if (minTotalDistanceStateI == -1)
                    throw new Exception();

                State minTotalDistanceState = inProgressNodes[minTotalDistanceStateI];
                inProgressNodes.RemoveAt(minTotalDistanceStateI);
                minTotalDistanceState.DijsktraInProgress = false;
                minTotalDistanceState.DijsktraExplored = true;

                foreach (var (energyToReachStateAdjacent, stateAdjacentToMinTotalDistanceState) in minTotalDistanceState.AdjacentStates)
                {
                    if (!stateAdjacentToMinTotalDistanceState.DijsktraExplored)
                    {
                        if (!stateAdjacentToMinTotalDistanceState.DijsktraInProgress)
                        {
                            inProgressNodes.Add(stateAdjacentToMinTotalDistanceState);
                            stateAdjacentToMinTotalDistanceState.DijsktraInProgress = true;
                        }

                        int trialDistance = minTotalDistanceState.DijsktraTotalDistance + energyToReachStateAdjacent;

                        if (trialDistance < stateAdjacentToMinTotalDistanceState.DijsktraTotalDistance)
                            stateAdjacentToMinTotalDistanceState.DijsktraTotalDistance = trialDistance;
                    }
                }
            }
            State finalState = State.cache["..AAAA.BBBB.CCCC.DDDD.."];
            Console.WriteLine(finalState.DijsktraTotalDistance);
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
                //Console.WriteLine($"CACHE HIT FOR {cacheKey}");
                cacheHits++;
            }
            else
                cache[cacheKey] = state;
            return state;
        }

        public bool P2 = false;

        public bool AdjacentStatesGenerated = false;
        public List<(int, State)> AdjacentStates = new List<(int, State)>();

        public char Hallway1 = '.';
        public char Hallway2 = '.';
        public char Hallway3 = '.';
        public char Hallway4 = '.';
        public char Hallway5 = '.';
        public char Hallway6 = '.';
        public char Hallway7 = '.';

        public List<char> ARoom = new List<char>();
        public List<char> BRoom = new List<char>();
        public List<char> CRoom = new List<char>();
        public List<char> DRoom = new List<char>();

        public bool DijsktraExplored = false;
        public bool DijsktraInProgress = false;
        public int DijsktraTotalDistance = int.MaxValue;

        public State()
        {

        }

        public State(State otherState)
        {
            Hallway1 = otherState.Hallway1;
            Hallway2 = otherState.Hallway2;
            Hallway3 = otherState.Hallway3;
            Hallway4 = otherState.Hallway4;
            Hallway5 = otherState.Hallway5;
            Hallway6 = otherState.Hallway6;
            Hallway7 = otherState.Hallway7;

            ARoom = new List<char>(otherState.ARoom);
            BRoom = new List<char>(otherState.BRoom);
            CRoom = new List<char>(otherState.CRoom);
            DRoom = new List<char>(otherState.DRoom);

            P2 = otherState.P2;
        }

        public void GenerateAdjacentStates()
        {
            int sizeOfRoom = P2 ? 4 : 2;

            int roomIndex;
            char amphipod;
            int energyPerStep;
            int steps;
            int energy;
            State newState;

            bool ARoomAllCorrect = true;
            foreach (char a in ARoom)
            {
                if (a != 'A')
                {
                    ARoomAllCorrect = false;
                    break;
                }
            }
            bool BRoomAllCorrect = true;
            foreach (char a in BRoom)
            {
                if (a != 'B')
                {
                    BRoomAllCorrect = false;
                    break;
                }
            }
            bool CRoomAllCorrect = true;
            foreach (char a in CRoom)
            {
                if (a != 'C')
                {
                    CRoomAllCorrect = false;
                    break;
                }
            }
            bool DRoomAllCorrect = true;
            foreach (char a in DRoom)
            {
                if (a != 'D')
                {
                    DRoomAllCorrect = false;
                    break;
                }
            }

            // This should generate all Amphipods moving out of room

            if (ARoom.Count > 0)
            {
                if (!ARoomAllCorrect)
                {
                    roomIndex = ARoom.Count - 1;
                    amphipod = ARoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway2 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - ARoom.Count) + 1 + 1;
                        newState.Hallway2 = amphipod;
                        newState.ARoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway1 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - ARoom.Count) + 1 + 2;
                            newState.Hallway1 = amphipod;
                            newState.ARoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }
                    }

                    if (Hallway3 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - ARoom.Count) + 1 + 1;
                        newState.Hallway3 = amphipod;
                        newState.ARoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway4 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - ARoom.Count) + 1 + 3;
                            newState.Hallway4 = amphipod;
                            newState.ARoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway5 == '.')
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - ARoom.Count) + 1 + 5;
                                newState.Hallway5 = amphipod;
                                newState.ARoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));

                                if (Hallway6 == '.')
                                {
                                    newState = new State(this);

                                    steps = (sizeOfRoom - ARoom.Count) + 1 + 6 + 1;
                                    newState.Hallway6 = amphipod;
                                    newState.ARoom.RemoveAt(roomIndex);

                                    energy = steps * energyPerStep;

                                    newState = cacheCheck(newState);
                                    AdjacentStates.Add((energy, newState));

                                    if (Hallway7 == '.')
                                    {
                                        newState = new State(this);

                                        steps = (sizeOfRoom - ARoom.Count) + 1 + 6 + 2;
                                        newState.Hallway7 = amphipod;
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
            }

            if (BRoom.Count > 0)
            {
                if (!BRoomAllCorrect)
                {
                    roomIndex = BRoom.Count - 1;
                    amphipod = BRoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway3 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - BRoom.Count) + 1 + 1;
                        newState.Hallway3 = amphipod;
                        newState.BRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway2 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - BRoom.Count) + 1 + 2 + 1;
                            newState.Hallway2 = amphipod;
                            newState.BRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway1 == '.')
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - BRoom.Count) + 1 + 2 + 2;
                                newState.Hallway1 = amphipod;
                                newState.BRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }

                    if (Hallway4 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - BRoom.Count) + 1 + 1;
                        newState.Hallway4 = amphipod;
                        newState.BRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;
                        AdjacentStates.Add((energy, newState));

                        if (Hallway5 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - BRoom.Count) + 1 + 3;
                            newState.Hallway5 = amphipod;
                            newState.BRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway6 == '.')
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - BRoom.Count) + 1 + 4 + 1;
                                newState.Hallway6 = amphipod;
                                newState.BRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));

                                if (Hallway7 == '.')
                                {
                                    newState = new State(this);

                                    steps = (sizeOfRoom - BRoom.Count) + 1 + 4 + 2;
                                    newState.Hallway7 = amphipod;
                                    newState.BRoom.RemoveAt(roomIndex);

                                    energy = steps * energyPerStep;

                                    newState = cacheCheck(newState);
                                    AdjacentStates.Add((energy, newState));
                                }
                            }
                        }
                    }
                }
            }

            if (CRoom.Count > 0)
            {
                if (!CRoomAllCorrect)
                {
                    roomIndex = CRoom.Count - 1;
                    amphipod = CRoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway4 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - CRoom.Count) + 1 + 1;
                        newState.Hallway4 = amphipod;
                        newState.CRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway3 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - CRoom.Count) + 1 + 3;
                            newState.Hallway3 = amphipod;
                            newState.CRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway2 == '.')
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - CRoom.Count) + 1 + 4 + 1;
                                newState.Hallway2 = amphipod;
                                newState.CRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));

                                if (Hallway1 == '.')
                                {
                                    newState = new State(this);

                                    steps = (sizeOfRoom - CRoom.Count) + 1 + 4 + 2;
                                    newState.Hallway1 = amphipod;
                                    newState.CRoom.RemoveAt(roomIndex);

                                    energy = steps * energyPerStep;

                                    newState = cacheCheck(newState);
                                    AdjacentStates.Add((energy, newState));
                                }
                            }
                        }
                    }

                    if (Hallway5 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - CRoom.Count) + 1 + 1;
                        newState.Hallway5 = amphipod;
                        newState.CRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway6 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - CRoom.Count) + 1 + 2 + 1;
                            newState.Hallway6 = amphipod;
                            newState.CRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway7 == '.')
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - CRoom.Count) + 1 + 2 + 2;
                                newState.Hallway7 = amphipod;
                                newState.CRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }
                }
            }

            if (DRoom.Count > 0)
            {
                if (!DRoomAllCorrect)
                {
                    roomIndex = DRoom.Count - 1;
                    amphipod = DRoom[roomIndex];
                    energyPerStep = AmphipodToEnergyPerStep(amphipod);

                    if (Hallway5 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - DRoom.Count) + 1 + 1;
                        newState.Hallway5 = amphipod;
                        newState.DRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway4 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - DRoom.Count) + 1 + 3;
                            newState.Hallway4 = amphipod;
                            newState.DRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));

                            if (Hallway3 == '.')
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - DRoom.Count) + 1 + 5;
                                newState.Hallway3 = amphipod;
                                newState.DRoom.RemoveAt(roomIndex);

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));

                                if (Hallway2 == '.')
                                {
                                    newState = new State(this);

                                    steps = (sizeOfRoom - DRoom.Count) + 1 + 6 + 1;
                                    newState.Hallway2 = amphipod;
                                    newState.DRoom.RemoveAt(roomIndex);

                                    energy = steps * energyPerStep;

                                    newState = cacheCheck(newState);
                                    AdjacentStates.Add((energy, newState));

                                    if (Hallway1 == '.')
                                    {
                                        newState = new State(this);

                                        steps = (sizeOfRoom - DRoom.Count) + 1 + 6 + 2;
                                        newState.Hallway1 = amphipod;
                                        newState.DRoom.RemoveAt(roomIndex);

                                        energy = steps * energyPerStep;

                                        newState = cacheCheck(newState);
                                        AdjacentStates.Add((energy, newState));
                                    }
                                }
                            }
                        }
                    }

                    if (Hallway6 == '.')
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - DRoom.Count) + 1 + 1;
                        newState.Hallway6 = amphipod;
                        newState.DRoom.RemoveAt(roomIndex);

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));

                        if (Hallway7 == '.')
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - DRoom.Count) + 1 + 2;
                            newState.Hallway7 = amphipod;
                            newState.DRoom.RemoveAt(roomIndex);

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }
                    }
                }
            }

            // Still need to generate moving of Amphipods into hallway


            if (Hallway1 != '.' && Hallway2 == '.')
            {
                amphipod = Hallway1;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - ARoom.Count) + 1 + 1;
                    newState.ARoom.Add(amphipod);
                    newState.Hallway1 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }

                if (Hallway3 == '.')
                {
                    if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - BRoom.Count) + 1 + 2 + 1;
                        newState.BRoom.Add(amphipod);
                        newState.Hallway1 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway4 == '.')
                    {
                        if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - CRoom.Count) + 1 + 4 + 1;
                            newState.CRoom.Add(amphipod);
                            newState.Hallway1 = '.';

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }

                        if (Hallway5 == '.')
                        {
                            if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - DRoom.Count) + 1 + 6 + 1;
                                newState.DRoom.Add(amphipod);
                                newState.Hallway1 = '.';

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }
                }
            }

            if (Hallway2 != '.')
            {
                amphipod = Hallway2;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - ARoom.Count) + 1;
                    newState.ARoom.Add(amphipod);
                    newState.Hallway2 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }

                if (Hallway3 == '.')
                {
                    if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - BRoom.Count) + 1 + 2;
                        newState.BRoom.Add(amphipod);
                        newState.Hallway2 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway4 == '.')
                    {
                        if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - CRoom.Count) + 1 + 4;
                            newState.CRoom.Add(amphipod);
                            newState.Hallway2 = '.';

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }

                        if (Hallway5 == '.')
                        {
                            if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - DRoom.Count) + 1 + 6;
                                newState.DRoom.Add(amphipod);
                                newState.Hallway2 = '.';

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }
                }
            }

            if (Hallway3 != '.')
            {
                amphipod = Hallway3;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - ARoom.Count) + 1;
                    newState.ARoom.Add(amphipod);
                    newState.Hallway3 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }
                if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - BRoom.Count) + 1;
                    newState.BRoom.Add(amphipod);
                    newState.Hallway3 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }

                if (Hallway4 == '.')
                {
                    if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - CRoom.Count) + 1 + 2;
                        newState.CRoom.Add(amphipod);
                        newState.Hallway3 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway5 == '.')
                    {
                        if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - DRoom.Count) + 1 + 4;
                            newState.DRoom.Add(amphipod);
                            newState.Hallway3 = '.';

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }
                    }
                }
            }

            if (Hallway4 != '.')
            {
                amphipod = Hallway4;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (Hallway3 == '.')
                {
                    if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - ARoom.Count) + 1 + 2;
                        newState.ARoom.Add(amphipod);
                        newState.Hallway4 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }
                }

                if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - BRoom.Count) + 1;
                    newState.BRoom.Add(amphipod);
                    newState.Hallway4 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }
                if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - CRoom.Count) + 1;
                    newState.CRoom.Add(amphipod);
                    newState.Hallway4 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }

                if (Hallway5 == '.')
                {
                    if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - DRoom.Count) + 1 + 2;
                        newState.DRoom.Add(amphipod);
                        newState.Hallway4 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }
                }
            }

            if (Hallway5 != '.')
            {
                amphipod = Hallway5;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (Hallway4 == '.')
                {
                    if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - BRoom.Count) + 1 + 2;
                        newState.BRoom.Add(amphipod);
                        newState.Hallway5 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway3 == '.')
                    {
                        if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - ARoom.Count) + 1 + 4;
                            newState.ARoom.Add(amphipod);
                            newState.Hallway5 = '.';

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }
                    }
                }

                if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - CRoom.Count) + 1;
                    newState.CRoom.Add(amphipod);
                    newState.Hallway5 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }

                if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - DRoom.Count) + 1;
                    newState.DRoom.Add(amphipod);
                    newState.Hallway5 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }
            }

            if (Hallway6 != '.')
            {
                amphipod = Hallway6;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (Hallway5 == '.')
                {
                    if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - CRoom.Count) + 1 + 2;
                        newState.CRoom.Add(amphipod);
                        newState.Hallway6 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway4 == '.')
                    {
                        if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - BRoom.Count) + 1 + 4;
                            newState.BRoom.Add(amphipod);
                            newState.Hallway6 = '.';

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }

                        if (Hallway3 == '.')
                        {
                            if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - ARoom.Count) + 1 + 6;
                                newState.ARoom.Add(amphipod);
                                newState.Hallway6 = '.';

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }
                }

                if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - DRoom.Count) + 1;
                    newState.DRoom.Add(amphipod);
                    newState.Hallway6 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }
            }

            if (Hallway7 != '.' && Hallway6 == '.')
            {
                amphipod = Hallway7;
                energyPerStep = AmphipodToEnergyPerStep(amphipod);

                if (Hallway5 == '.')
                {
                    if (CRoom.Count < sizeOfRoom && amphipod == 'C' && CRoomAllCorrect)
                    {
                        newState = new State(this);

                        steps = (sizeOfRoom - CRoom.Count) + 1 + 2 + 1;
                        newState.CRoom.Add(amphipod);
                        newState.Hallway7 = '.';

                        energy = steps * energyPerStep;

                        newState = cacheCheck(newState);
                        AdjacentStates.Add((energy, newState));
                    }

                    if (Hallway4 == '.')
                    {
                        if (BRoom.Count < sizeOfRoom && amphipod == 'B' && BRoomAllCorrect)
                        {
                            newState = new State(this);

                            steps = (sizeOfRoom - BRoom.Count) + 1 + 4 + 1;
                            newState.BRoom.Add(amphipod);
                            newState.Hallway7 = '.';

                            energy = steps * energyPerStep;

                            newState = cacheCheck(newState);
                            AdjacentStates.Add((energy, newState));
                        }

                        if (Hallway3 == '.')
                        {
                            if (ARoom.Count < sizeOfRoom && amphipod == 'A' && ARoomAllCorrect)
                            {
                                newState = new State(this);

                                steps = (sizeOfRoom - ARoom.Count) + 1 + 6 + 1;
                                newState.ARoom.Add(amphipod);
                                newState.Hallway7 = '.';

                                energy = steps * energyPerStep;

                                newState = cacheCheck(newState);
                                AdjacentStates.Add((energy, newState));
                            }
                        }
                    }
                }

                if (DRoom.Count < sizeOfRoom && amphipod == 'D' && DRoomAllCorrect)
                {
                    newState = new State(this);

                    steps = (sizeOfRoom - DRoom.Count) + 1 + 1;
                    newState.DRoom.Add(amphipod);
                    newState.Hallway7 = '.';

                    energy = steps * energyPerStep;

                    newState = cacheCheck(newState);
                    AdjacentStates.Add((energy, newState));
                }
            }

            /*
            foreach (var (e, s) in AdjacentStates)
            {
                Console.WriteLine($"Energy required for following state: {e}");
                s.Print();
            }
            */

            AdjacentStatesGenerated = true;
            foreach (var (e, s) in AdjacentStates)
            {
                if (!s.AdjacentStatesGenerated)
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

            strRep += $"{Hallway1}{Hallway2}";

            for (int i = 0; i < (P2 ? 4 : 2); i++)
            {
                if (ARoom.Count > i)
                    strRep += ARoom[i];
                else
                    strRep += ".";
            }

            strRep += Hallway3;

            for (int i = 0; i < (P2 ? 4 : 2); i++)
            {
                if (BRoom.Count > i)
                    strRep += BRoom[i];
                else
                    strRep += ".";
            }

            strRep += Hallway4;

            for (int i = 0; i < (P2 ? 4 : 2); i++)
            {
                if (CRoom.Count > i)
                    strRep += CRoom[i];
                else
                    strRep += ".";
            }

            strRep += Hallway5;

            for (int i = 0; i < (P2 ? 4 : 2); i++)
            {
                if (DRoom.Count > i)
                    strRep += DRoom[i];
                else
                    strRep += ".";
            }

            strRep += $"{Hallway6}{Hallway7}";

            return strRep;
        }

        public void Print()
        {
            Console.WriteLine($"Cache Key:");
            Console.WriteLine(UniqueStringRep());

            Console.WriteLine("State:");

            Console.Write($"{Hallway1}{Hallway2}+{Hallway3}+{Hallway4}+{Hallway5}+{Hallway6}{Hallway7}");

            Console.WriteLine();

            for (int i = (P2 ? 3 : 1); i >= 0; i--)
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

        public override string ToString()
        {
            return this.UniqueStringRep();
        }
    }
}
