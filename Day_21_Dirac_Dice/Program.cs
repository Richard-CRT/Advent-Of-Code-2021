using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_21_Dirac_Dice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            string[] str0Split = inputList[0].Split(' ');
            string[] str1Split = inputList[1].Split(' ');
            int p1StartPosition = int.Parse(str0Split[str0Split.Length - 1]) - 1;
            int p2StartPosition = int.Parse(str1Split[str1Split.Length - 1]) - 1;

            P1(p1StartPosition, p2StartPosition);
            P2(p1StartPosition, p2StartPosition);
        }

        static void P1(int p1StartPosition, int p2StartPosition)
        {
            DeterministicDice deterministicDice = new DeterministicDice();

            int p1Position = p1StartPosition;
            int p2Position = p2StartPosition;
            int p1Score = 0;
            int p2Score = 0;

            while (p1Score < 1000 && p2Score < 1000)
            {
                int move;

                move = deterministicDice.Roll() + deterministicDice.Roll() + deterministicDice.Roll();
                p1Position = (p1Position + move) % 10;
                p1Score += p1Position + 1;

                if (p1Score < 1000)
                {
                    move = deterministicDice.Roll() + deterministicDice.Roll() + deterministicDice.Roll();
                    p2Position = (p2Position + move) % 10;
                    p2Score += p2Position + 1;
                }
            }

            int lowerScore = p1Score < p2Score ? p1Score : p2Score;
            Console.WriteLine(deterministicDice.RollCount * lowerScore);
            Console.ReadLine();
        }

        static void P2(int p1Position, int p2Position)
        {
            Int64 p1UniverseWinCount = 0;
            Int64 p2UniverseWinCount = 0;

            (Int64, Int64) temp1 = Turn(true, 3, 1, p1Position, p2Position, 0, 0);
            (Int64, Int64) temp2 = Turn(true, 4, 3, p1Position, p2Position, 0, 0);
            (Int64, Int64) temp3 = Turn(true, 5, 6, p1Position, p2Position, 0, 0);
            (Int64, Int64) temp4 = Turn(true, 6, 7, p1Position, p2Position, 0, 0);
            (Int64, Int64) temp5 = Turn(true, 7, 6, p1Position, p2Position, 0, 0);
            (Int64, Int64) temp6 = Turn(true, 8, 3, p1Position, p2Position, 0, 0);
            (Int64, Int64) temp7 = Turn(true, 9, 1, p1Position, p2Position, 0, 0);

            p1UniverseWinCount += temp1.Item1 + temp2.Item1 + temp3.Item1 + temp4.Item1 + temp5.Item1 + temp6.Item1 + temp7.Item1;
            p2UniverseWinCount += temp1.Item2 + temp2.Item2 + temp3.Item2 + temp4.Item2 + temp5.Item2 + temp6.Item2 + temp7.Item2;

            Int64 result = p1UniverseWinCount > p2UniverseWinCount ? p1UniverseWinCount : p2UniverseWinCount;
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static Dictionary<(bool, int, int, int, int), (Int64, Int64)> cache = new Dictionary<(bool, int, int, int, int), (long, long)>();

        static (Int64, Int64) Turn(bool p1Turn, int sumRoll, Int64 numberRolls, int p1Position, int p2Position, int p1Score, int p2Score)
        {
            Int64 p1UniverseWinCount = 0;
            Int64 p2UniverseWinCount = 0;

            if (p1Turn)
            {
                p1Position = (p1Position + sumRoll) % 10;
                p1Score += p1Position + 1;
            }
            else
            {
                p2Position = (p2Position + sumRoll) % 10;
                p2Score += p2Position + 1;
            }

            if (p1Score >= 21)
            {
                p1UniverseWinCount += numberRolls;
                return (p1UniverseWinCount, p2UniverseWinCount);
            }
            else if (p2Score >= 21)
            {
                p2UniverseWinCount += numberRolls;
                return (p1UniverseWinCount, p2UniverseWinCount);
            }

            var cacheKey = (p1Turn, p1Position, p2Position, p1Score, p2Score);

            Int64 p1UniverseWinInc;
            Int64 p2UniverseWinInc;

            if (cache.ContainsKey(cacheKey))
            {
                var (cacheP1UniverseWinCount, cacheP2UniverseWinCount) = cache[cacheKey];
                p1UniverseWinInc = cacheP1UniverseWinCount;
                p2UniverseWinInc = cacheP2UniverseWinCount;
            }
            else
            {
                (Int64, Int64) temp1 = Turn(!p1Turn, 3,  1, p1Position, p2Position, p1Score, p2Score);
                (Int64, Int64) temp2 = Turn(!p1Turn, 4,  3, p1Position, p2Position, p1Score, p2Score);
                (Int64, Int64) temp3 = Turn(!p1Turn, 5,  6, p1Position, p2Position, p1Score, p2Score);
                (Int64, Int64) temp4 = Turn(!p1Turn, 6,  7, p1Position, p2Position, p1Score, p2Score);
                (Int64, Int64) temp5 = Turn(!p1Turn, 7,  6, p1Position, p2Position, p1Score, p2Score);
                (Int64, Int64) temp6 = Turn(!p1Turn, 8,  3, p1Position, p2Position, p1Score, p2Score);
                (Int64, Int64) temp7 = Turn(!p1Turn, 9,  1, p1Position, p2Position, p1Score, p2Score);

                p1UniverseWinInc = temp1.Item1 + temp2.Item1 + temp3.Item1 + temp4.Item1 + temp5.Item1 + temp6.Item1 + temp7.Item1;
                p2UniverseWinInc = temp1.Item2 + temp2.Item2 + temp3.Item2 + temp4.Item2 + temp5.Item2 + temp6.Item2 + temp7.Item2;

                var cacheValue = (p1UniverseWinInc, p2UniverseWinInc);
                cache[cacheKey] = cacheValue;
            }

            p1UniverseWinCount += numberRolls * p1UniverseWinInc;
            p2UniverseWinCount += numberRolls * p2UniverseWinInc;

            return (p1UniverseWinCount, p2UniverseWinCount);
        }
    }

    public class DeterministicDice
    {
        public int RollCount = 0;

        private int NextVal = 1;
        public int Roll()
        {
            int result = NextVal;
            NextVal++;
            if (NextVal > 100)
                NextVal = 1;
            RollCount++;
            return result;
        }
    }
}
