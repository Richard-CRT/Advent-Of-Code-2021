using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_10_Syntax_Scoring
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            P1and2(inputList);
        }

        static void P1and2(List<string> inputList)
        {
            int p1Score = 0;
            List<Int64> p2LineScores = new List<Int64>();

            foreach (string line in inputList)
            {
                Int64 p2LineScore = 0;
                Stack<char> stack = new Stack<char>();

                char errorChar = ' ';
                foreach (char c in line)
                {
                    if (c == '(')
                        stack.Push(c);
                    else if (c == '[')
                        stack.Push(c);
                    else if (c == '{')
                        stack.Push(c);
                    else if (c == '<')
                        stack.Push(c);

                    else if (c == ')')
                    {
                        if (stack.Pop() != '(')
                        {
                            errorChar = c;
                            break;
                        }
                    }
                    else if (c == ']')
                    {
                        if (stack.Pop() != '[')
                        {
                            errorChar = c;
                            break;
                        }
                    }
                    else if (c == '}')
                    {
                        if (stack.Pop() != '{')
                        {
                            errorChar = c;
                            break;
                        }
                    }
                    else if (c == '>')
                    {
                        if (stack.Pop() != '<')
                        {
                            errorChar = c;
                            break;
                        }
                    }
                }
                if (errorChar != ' ')
                {
                    // corrupt
                    switch (errorChar)
                    {
                        case ')':
                            p1Score += 3;
                            break;
                        case ']':
                            p1Score += 57;
                            break;
                        case '}':
                            p1Score += 1197;
                            break;
                        case '>':
                            p1Score += 25137;
                            break;
                    }
                }
                else
                {
                    // incomplete but not corrupt
                    while (stack.Count > 0)
                    {
                        p2LineScore *= 5;
                        switch (stack.Pop())
                        {
                            case '(':
                                p2LineScore += 1;
                                break;
                            case '[':
                                p2LineScore += 2;
                                break;
                            case '{':
                                p2LineScore += 3;
                                break;
                            case '<':
                                p2LineScore += 4;
                                break;
                        }
                    }
                    p2LineScores.Add(p2LineScore);
                }
            }
            p2LineScores.Sort();

            Console.WriteLine(p1Score);
            Console.ReadLine();
            Console.WriteLine(p2LineScores[(p2LineScores.Count - 1) / 2]);
            Console.ReadLine();
        }
    }
}
