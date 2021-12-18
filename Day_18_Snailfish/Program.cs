using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_18_Snailfish
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<SnailfishNumber> snailfishNumbers = inputList.Select(x => new SnailfishNumber(x)).ToList();

            P1(snailfishNumbers);
            P2(snailfishNumbers);
        }

        static void P1(List<SnailfishNumber> snailfishNumbers)
        {            
            SnailfishNumber sum = snailfishNumbers[0] + snailfishNumbers[1];
            for (int i = 2; i < snailfishNumbers.Count; i++)
            {
                sum += snailfishNumbers[i];
            }

            Console.WriteLine(sum.Magnitude);
            Console.ReadLine();
        }

        static void P2(List<SnailfishNumber> snailfishNumbers)
        {
            int maxMag = int.MinValue;
            for (int lefti = 0; lefti < snailfishNumbers.Count; lefti++)
            {
                for (int righti = 0; righti < snailfishNumbers.Count; righti++)
                {
                    if (lefti != righti)
                    {
                        SnailfishNumber sum = snailfishNumbers[lefti] + snailfishNumbers[righti];
                        int sumMag = sum.Magnitude;
                        if (sumMag > maxMag)
                            maxMag = sumMag;
                    }
                }
            }

            Console.WriteLine(maxMag);
            Console.ReadLine();
        }
    }

    public class SnailfishNumber
    {
        public SnailfishNumber LeftSnailFishNumber;
        public SnailfishNumber RightSnailFishNumber;

        public int Value = -1;

        public int Magnitude
        {
            get
            {
                if (Value != -1)
                    return Value;
                else 
                    return 3 * LeftSnailFishNumber.Magnitude + 2 * RightSnailFishNumber.Magnitude;
            }
        }

        public SnailfishNumber(string snailfishNumberStr)
        {
            if (snailfishNumberStr[0] >= '0' && snailfishNumberStr[0] <= '9')
                Value = snailfishNumberStr[0] - '0';
            else
            {
                int sqBrcktCnt = 0;
                int leftEndPos = 1;
                int rightStartPos = 1;
                int rightEndPos = 1;
                bool left = true;
                for (int i = 0; i < snailfishNumberStr.Length; i++)
                {
                    char c = snailfishNumberStr[i];
                    if (c == ',' && sqBrcktCnt == 1 && left)
                    {
                        leftEndPos = i;
                        rightStartPos = i + 1;
                    }
                    else if (c == ']' && sqBrcktCnt == 1)
                    {
                        rightEndPos = i;
                    }
                    else if (c == '[')
                        sqBrcktCnt++;
                    else if (c == ']')
                        sqBrcktCnt--;

                }

                string leftSnailfishNumberStr = snailfishNumberStr.Substring(1, leftEndPos - 1);
                string rightSnailfishNumberStr = snailfishNumberStr.Substring(rightStartPos, rightEndPos - rightStartPos);

                LeftSnailFishNumber = new SnailfishNumber(leftSnailfishNumberStr);
                RightSnailFishNumber = new SnailfishNumber(rightSnailfishNumberStr);
            }
        }

        public SnailfishNumber(SnailfishNumber leftSnailfishNumber, SnailfishNumber rightSnailfishNumber)
        {
            LeftSnailFishNumber = leftSnailfishNumber;
            RightSnailFishNumber = rightSnailfishNumber;
        }

        public SnailfishNumber(int val)
        {
            Value = val;
        }

        public (int, int, bool) CheckForExplode(int depth)
        {
            bool exploded = false;
            if (Value == -1)
            {
                if (depth == 4)
                {
                    int leftVal = LeftSnailFishNumber.Value;
                    int rightVal = RightSnailFishNumber.Value;
                    if (leftVal == -1 || rightVal == -1)
                        throw new Exception();

                    LeftSnailFishNumber = null;
                    RightSnailFishNumber = null;
                    Value = 0;
                    return (leftVal, rightVal, true);
                }
                else
                {
                    (int, int, bool) leftRes = LeftSnailFishNumber.CheckForExplode(depth + 1);
                    if (leftRes.Item2 != -1)
                    {
                        RightSnailFishNumber.AddRight(leftRes.Item2);
                    }
                    if (leftRes.Item1 != -1)
                    {
                        return (leftRes.Item1, -1, true);
                    }

                    exploded = leftRes.Item3;

                    if (!exploded)
                    {
                        (int, int, bool) rightRes = RightSnailFishNumber.CheckForExplode(depth + 1);
                        if (rightRes.Item1 != -1)
                        {
                            LeftSnailFishNumber.AddLeft(rightRes.Item1);
                        }
                        if (rightRes.Item2 != -1)
                        {
                            return (-1, rightRes.Item2, true);
                        }

                        exploded = rightRes.Item3;
                    }

                }
            }
            return (-1, -1, exploded);
        }

        public bool AddLeft(int add)
        {
            if (Value != -1)
            {
                Value += add;
                return true;
            }
            else
            {
                if (RightSnailFishNumber.AddLeft(add))
                    return true;
                if (LeftSnailFishNumber.AddLeft(add))
                    return true;
            }
            return false;
        }

        public bool AddRight(int add)
        {
            if (Value != -1)
            {
                Value += add;
                return true;
            }
            else
            {
                if (LeftSnailFishNumber.AddRight(add))
                    return true;
                if (RightSnailFishNumber.AddRight(add))
                    return true;
            }
            return false;
        }

        public bool CheckForSplit()
        {
            if (Value == -1)
            {
                if (LeftSnailFishNumber.CheckForSplit())
                    return true;
                if (RightSnailFishNumber.CheckForSplit())
                    return true;
            }
            else
            {
                if (Value > 9)
                {
                    double div = Value / (double)2;
                    int newLeft = (int)Math.Floor(div);
                    LeftSnailFishNumber = new SnailfishNumber(newLeft);
                    int newRight = (int)Math.Ceiling(div);
                    RightSnailFishNumber = new SnailfishNumber(newRight);
                    Value = -1;
                    return true;
                }
            }
            return false;
        }

        public void Reduce()
        {
            //Console.WriteLine(this);
            bool actionChanged = true;
            while (actionChanged)
            {
                actionChanged = false;
                var temp = this.CheckForExplode(0);
                actionChanged = temp.Item3;
                if (actionChanged)
                {
                    //Console.WriteLine("explode");
                }

                if (!actionChanged)
                {
                    actionChanged = this.CheckForSplit();
                    if (actionChanged)
                    {
                        //Console.WriteLine("split");
                    }
                }
                //Console.WriteLine(this);
            }
        }

        public SnailfishNumber Copy()
        {
            if (Value != -1)
            {
                return new SnailfishNumber(Value);
            }
            else
            {
                return new SnailfishNumber(LeftSnailFishNumber.Copy(), RightSnailFishNumber.Copy());
            }
        }

        public override string ToString()
        {
            if (Value != -1)
                return Value.ToString();
            else
                return $"[{LeftSnailFishNumber},{RightSnailFishNumber}]";
        }

        public static SnailfishNumber operator +(SnailfishNumber left, SnailfishNumber right)
        {
            SnailfishNumber leftCopy = left.Copy();
            SnailfishNumber rightCopy = right.Copy();
            SnailfishNumber joinedSnailfishNumber = new SnailfishNumber(leftCopy, rightCopy);
            joinedSnailfishNumber.Reduce();
            return joinedSnailfishNumber;
        }
    }
}
