using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_14_Extended_Polymerisation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            Dictionary<string, ReplacementPair> replacementPairs = new Dictionary<string, ReplacementPair>();
            Dictionary<char, Int64> howManyOfEachLetter = new Dictionary<char, Int64>();
            string template = inputList[0];
            int i = 2;
            while (i < inputList.Count && inputList[i] != "")
            {
                string substitution = inputList[i];
                string pair = substitution.Substring(0, 2);
                string newLetter = substitution.Substring(6, 1);
                ReplacementPair rP = new ReplacementPair(pair, newLetter);
                replacementPairs[pair] = rP;

                i++;
            }

            foreach (char c in template)
            {
                if (howManyOfEachLetter.ContainsKey(c))
                    howManyOfEachLetter[c]++;
                else
                    howManyOfEachLetter[c] = 1;
            }

            for (i = 0; i < template.Length - 1; i++)
            {
                char[] chars = { template[i], template[i + 1] };
                string pair = new string(chars);
                if (replacementPairs.ContainsKey(pair))
                {
                    replacementPairs[pair].NewCount++;
                }
            }
            foreach (var kVP in replacementPairs)
            {
                var rP = kVP.Value;
                rP.Count = rP.NewCount;
            }

            P1(replacementPairs, howManyOfEachLetter);
            P2(replacementPairs, howManyOfEachLetter);
        }

        static void P1(Dictionary<string, ReplacementPair> replacementPairs, Dictionary<char, Int64> howManyOfEachLetter)
        {
            for (int cycle = 0; cycle < 10; cycle++)
            {
                Cycle(replacementPairs, howManyOfEachLetter);
            }

            Int64 minElementCount = Int64.MaxValue;
            Int64 maxElementCount = Int64.MinValue;
            foreach (var kVP in howManyOfEachLetter)
            {
                if (kVP.Value < minElementCount)
                    minElementCount = kVP.Value;
                if (kVP.Value > maxElementCount)
                    maxElementCount = kVP.Value;
            }

            Int64 result = maxElementCount - minElementCount;
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static void P2(Dictionary<string, ReplacementPair> replacementPairs, Dictionary<char, Int64> howManyOfEachLetter)
        {
            for (int cycle = 10; cycle < 40; cycle++)
            {
                Cycle(replacementPairs, howManyOfEachLetter);
            }

            Int64 minElementCount = Int64.MaxValue;
            Int64 maxElementCount = Int64.MinValue;
            foreach (var kVP in howManyOfEachLetter)
            {
                if (kVP.Value < minElementCount)
                    minElementCount = kVP.Value;
                if (kVP.Value > maxElementCount)
                    maxElementCount = kVP.Value;
            }

            Int64 result = maxElementCount - minElementCount;
            Console.WriteLine(result);
            Console.ReadLine();
        }

        static void Cycle(Dictionary<string, ReplacementPair> replacementPairs, Dictionary<char, Int64> howManyOfEachLetter)
        {
            foreach (var kVP in replacementPairs)
            {
                var rP = kVP.Value;
                if (rP.Count > 0)
                {
                    if (howManyOfEachLetter.ContainsKey(rP.NewLetter))
                        howManyOfEachLetter[rP.NewLetter] += rP.Count;
                    else
                        howManyOfEachLetter[rP.NewLetter] = rP.Count;
                    replacementPairs[rP.NewPair1].NewCount += rP.Count;
                    replacementPairs[rP.NewPair2].NewCount += rP.Count;
                    rP.NewCount -= rP.Count;
                }
            }
            foreach (var kVP in replacementPairs)
            {
                var rP = kVP.Value;
                rP.Count = rP.NewCount;
            }
        }
    }

    public class ReplacementPair
    {
        public string Pair;
        public char NewLetter;
        public Int64 Count = 0;
        public Int64 NewCount = 0;
        public string NewPair1 = "";
        public string NewPair2 = "";

        public ReplacementPair(string pair, string newLetter)
        {
            Pair = pair;
            NewPair1 = pair[0] + newLetter;
            NewPair2 = newLetter + pair[1];
            NewLetter = newLetter[0];
        }

        public override string ToString()
        {
            return $"{Pair} -> {NewLetter}, {Count}";
        }
    }
}
