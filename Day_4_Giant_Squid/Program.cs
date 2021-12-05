using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_4_Giant_Squid
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<int> randomNumbers = inputList[0].Split(',').Select(i => Int32.Parse(i)).ToList();

            string input = AoCUtilities.GetInput();
            List<string> boards = input.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            boards.RemoveAt(0);

            List<BingoBoard> bingoBoards = boards.Select(board => new BingoBoard(board)).ToList();

            List<BingoBoard> bingoBoardsP1 = new List<BingoBoard>(bingoBoards);
            P1(randomNumbers, bingoBoardsP1);
            List<BingoBoard> bingoBoardsP2 = new List<BingoBoard>(bingoBoards);
            P2(randomNumbers, bingoBoardsP2);
        }

        static void P1(List<int> randomNumbers, List<BingoBoard> bingoBoards)
        {
            foreach (int randomNumber in randomNumbers)
            {
                foreach (BingoBoard bingoBoard in bingoBoards)
                {
                    if (bingoBoard.Pick(randomNumber))
                    {
                        Console.WriteLine(randomNumber * bingoBoard.SumUnmarked());
                        Console.ReadLine();
                        return;
                    }
                }
            }
        }

        static void P2(List<int> randomNumbers, List<BingoBoard> bingoBoards)
        {
            List<BingoBoard> wonBingoBoards = new List<BingoBoard>();
            foreach (int randomNumber in randomNumbers)
            {
                foreach (BingoBoard bingoBoard in bingoBoards)
                {
                    if (bingoBoard.Pick(randomNumber))
                    {
                        if (!wonBingoBoards.Contains(bingoBoard))
                            wonBingoBoards.Add(bingoBoard);

                        if (wonBingoBoards.Count == bingoBoards.Count)
                        {
                            Console.WriteLine(randomNumber * bingoBoard.SumUnmarked());
                            Console.ReadLine();
                        }
                    }
                }
            }
        }
    }

    public class BingoBoard
    {
        public BingoInt[][] Board = new BingoInt[5][];

        public BingoBoard(string board)
        {
            for (int i = 0; i < Board.Length; i++)
            {
                Board[i] = new BingoInt[5];
            }

            string[] lines = board.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < lines.Length; y++)
            {
                string[] numbers = lines[y].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int x = 0; x < numbers.Length; x++)
                {
                    this.Board[y][x] = new BingoInt(Int32.Parse(numbers[x]));
                }
            }
        }

        public bool Pick(int randomNumber)
        {
            for (int y = 0; y < Board.Length; y++)
            {
                for (int x = 0; x < Board[y].Length; x++)
                {
                    if (Board[y][x].Value == randomNumber)
                    {
                        Board[y][x].Picked = true;

                        bool allColPicked = true;
                        bool allRowPicked = true;

                        for (int yi = 0; yi < Board.Length; yi++)
                        {
                            if (!Board[yi][x].Picked)
                            {
                                allColPicked = false;
                                break;
                            }
                        }

                        if (!allColPicked)
                        {
                            for (int xi = 0; xi < Board[y].Length; xi++)
                            {
                                if (!Board[y][xi].Picked)
                                {
                                    allRowPicked = false;
                                    break;
                                }
                            }
                        }

                        if (allColPicked || allRowPicked)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public int SumUnmarked()
        {
            int sum = 0;
            for (int y = 0; y < Board.Length; y++)
            {
                for (int x = 0; x < Board[y].Length; x++)
                {
                    if (!Board[y][x].Picked)
                        sum += Board[y][x].Value;
                }
            }
            return sum;
        }

        public class BingoInt
        {
            public int Value { get; set; } = 0;
            public bool Picked { get; set; } = false;

            public BingoInt(int val)
            {
                this.Value = val;
            }

            public override string ToString()
            {
                return $"[{Value}, {Picked}]";
            }
        }
    }
}
