using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_8_Seven_Segment_Search
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<Display> displays = inputList.Select(i => new Display(i)).ToList();

            P1(displays);
            P2(displays);
        }

        static void P1(List<Display> displays)
        {
            int _1478count = 0;
            foreach (Display display in displays)
            {
                foreach (DisplayDigit displayDigit in display.outputDigits)
                {
                    if (
                        displayDigit.segments.Count == 2 || // 1
                        displayDigit.segments.Count == 4 || // 4
                        displayDigit.segments.Count == 3 || // 7
                        displayDigit.segments.Count == 7 // 8
                        )
                        _1478count++;
                }
            }
            Console.WriteLine(_1478count);
            Console.ReadLine();
        }

        static void P2(List<Display> displays)
        {
            int sum = 0;
            foreach (Display display in displays)
            {
                sum += display.val;
            }
            Console.WriteLine(sum);
            Console.ReadLine();
        }
    }

    public class Display
    {
        Dictionary<string, List<DisplaySegment>> segmentLocations = new Dictionary<string, List<DisplaySegment>>();
        Dictionary<string, DisplaySegment> trueSegmentLocations;

        List<DisplayDigit> uniquePatternDigits = new List<DisplayDigit>();
        public List<DisplayDigit> outputDigits = new List<DisplayDigit>();
        public int val = -1;

        public Display(string display)
        {
            segmentLocations["T"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };
            segmentLocations["TL"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };
            segmentLocations["TR"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };
            segmentLocations["M"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };
            segmentLocations["BL"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };
            segmentLocations["BR"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };
            segmentLocations["B"] = new List<DisplaySegment> { DisplaySegment.a, DisplaySegment.b, DisplaySegment.c, DisplaySegment.d, DisplaySegment.e, DisplaySegment.f, DisplaySegment.g };

            string[] split = display.Split('|');
            uniquePatternDigits = split[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(i => new DisplayDigit(i)).ToList();
            outputDigits = split[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(i => new DisplayDigit(i)).ToList();

            // make easy deductions immediately
            foreach (var uniquePatternDigit in uniquePatternDigits)
            {
                uniquePatternDigit.DeduceEasyOnes(this.segmentLocations);
            }

            List<Dictionary<string, DisplaySegment>> possibleSegmentLocations = new List<Dictionary<string, DisplaySegment>>();
            possibleSegmentLocations = new List<Dictionary<string, DisplaySegment>>
            {
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][0] },
                    { "M", segmentLocations["M"][1] },

                    { "TR", segmentLocations["TR"][0] },
                    { "BR", segmentLocations["BR"][1] },

                    { "BL", segmentLocations["BL"][0] },
                    { "B", segmentLocations["B"][1] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][0] },
                    { "M", segmentLocations["M"][1] },

                    { "TR", segmentLocations["TR"][0] },
                    { "BR", segmentLocations["BR"][1] },

                    { "BL", segmentLocations["BL"][1] },
                    { "B", segmentLocations["B"][0] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][0] },
                    { "M", segmentLocations["M"][1] },

                    { "TR", segmentLocations["TR"][1] },
                    { "BR", segmentLocations["BR"][0] },

                    { "BL", segmentLocations["BL"][0] },
                    { "B", segmentLocations["B"][1] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][0] },
                    { "M", segmentLocations["M"][1] },

                    { "TR", segmentLocations["TR"][1] },
                    { "BR", segmentLocations["BR"][0] },

                    { "BL", segmentLocations["BL"][1] },
                    { "B", segmentLocations["B"][0] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][1] },
                    { "M", segmentLocations["M"][0] },

                    { "TR", segmentLocations["TR"][0] },
                    { "BR", segmentLocations["BR"][1] },

                    { "BL", segmentLocations["BL"][0] },
                    { "B", segmentLocations["B"][1] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][1] },
                    { "M", segmentLocations["M"][0] },

                    { "TR", segmentLocations["TR"][0] },
                    { "BR", segmentLocations["BR"][1] },

                    { "BL", segmentLocations["BL"][1] },
                    { "B", segmentLocations["B"][0] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][1] },
                    { "M", segmentLocations["M"][0] },

                    { "TR", segmentLocations["TR"][1] },
                    { "BR", segmentLocations["BR"][0] },

                    { "BL", segmentLocations["BL"][0] },
                    { "B", segmentLocations["B"][1] },
                },
                new Dictionary<string, DisplaySegment>
                {
                    { "T", segmentLocations["T"][0] },

                    { "TL", segmentLocations["TL"][1] },
                    { "M", segmentLocations["M"][0] },

                    { "TR", segmentLocations["TR"][1] },
                    { "BR", segmentLocations["BR"][0] },

                    { "BL", segmentLocations["BL"][1] },
                    { "B", segmentLocations["B"][0] },
                }
            };

            foreach (var possibleSegmentLocation in possibleSegmentLocations)
            {
                bool success = true;
                foreach (var uniquePatternDigit in uniquePatternDigits)
                {
                    if (!uniquePatternDigit.TryHardOnes(possibleSegmentLocation))
                    {
                        success = false;
                        break;
                    }
                }
                if (success)
                {
                    trueSegmentLocations = possibleSegmentLocation;
                    break;
                }
            }

            if (trueSegmentLocations == null)
            {
                throw new Exception();
            }
            else
            {
                outputDigits[0].TryHardOnes(trueSegmentLocations);
                outputDigits[1].TryHardOnes(trueSegmentLocations);
                outputDigits[2].TryHardOnes(trueSegmentLocations);
                outputDigits[3].TryHardOnes(trueSegmentLocations);
            }

            if (outputDigits[0].val != -1 && outputDigits[1].val != -1 && outputDigits[2].val != -1 && outputDigits[3].val != -1)
            {
                val = outputDigits[0].val * 1000 + outputDigits[1].val * 100 + outputDigits[2].val * 10 + outputDigits[3].val * 1;
            }
            else
            {
                throw new Exception();
            }
        }
    }

    public enum DisplaySegment
    {
        a,
        b,
        c,
        d,
        e,
        f,
        g
    }

    public class DisplayDigit
    {
        public static DisplaySegment CharToDisplaySegment(char c)
        {
            switch (c)
            {
                case 'a':
                    return DisplaySegment.a;
                case 'b':
                    return DisplaySegment.b;
                case 'c':
                    return DisplaySegment.c;
                case 'd':
                    return DisplaySegment.d;
                case 'e':
                    return DisplaySegment.e;
                case 'f':
                    return DisplaySegment.f;
                case 'g':
                    return DisplaySegment.g;

                default:
                    return DisplaySegment.a;
            }
        }
        public static char DisplaySegmentToChar(DisplaySegment ds)
        {
            switch (ds)
            {
                case DisplaySegment.a:
                    return 'a';
                case DisplaySegment.b:
                    return 'b';
                case DisplaySegment.c:
                    return 'c';
                case DisplaySegment.d:
                    return 'd';
                case DisplaySegment.e:
                    return 'e';
                case DisplaySegment.f:
                    return 'f';
                case DisplaySegment.g:
                    return 'g';

                default:
                    return 'a';
            }
        }

        public List<DisplaySegment> segments = new List<DisplaySegment>();
        public int val = -1;

        public DisplayDigit(string digit)
        {
            foreach (char c in digit)
            {
                segments.Add(DisplayDigit.CharToDisplaySegment(c));
            }
        }

        public void DeduceEasyOnes(Dictionary<string, List<DisplaySegment>> segmentLocations)
        {
            if (this.segments.Count == 2)
            {
                // digit is a 1
                segmentLocations["TR"] = segmentLocations["TR"].Intersect(this.segments).ToList();
                segmentLocations["BR"] = segmentLocations["BR"].Intersect(this.segments).ToList();

                segmentLocations["T"] = segmentLocations["T"].Except(this.segments).ToList();
                segmentLocations["TL"] = segmentLocations["TL"].Except(this.segments).ToList();
                segmentLocations["M"] = segmentLocations["M"].Except(this.segments).ToList();
                segmentLocations["BL"] = segmentLocations["BL"].Except(this.segments).ToList();
                segmentLocations["B"] = segmentLocations["B"].Except(this.segments).ToList();
            }
            else if (this.segments.Count == 4)
            {
                // digit is a 4
                segmentLocations["TR"] = segmentLocations["TR"].Intersect(this.segments).ToList();
                segmentLocations["BR"] = segmentLocations["BR"].Intersect(this.segments).ToList();
                segmentLocations["M"] = segmentLocations["M"].Intersect(this.segments).ToList();
                segmentLocations["TL"] = segmentLocations["TL"].Intersect(this.segments).ToList();

                segmentLocations["T"] = segmentLocations["T"].Except(this.segments).ToList();
                segmentLocations["BL"] = segmentLocations["BL"].Except(this.segments).ToList();
                segmentLocations["B"] = segmentLocations["B"].Except(this.segments).ToList();
            }
            else if (this.segments.Count == 3)
            {
                // digit is a 4
                segmentLocations["TR"] = segmentLocations["TR"].Intersect(this.segments).ToList();
                segmentLocations["BR"] = segmentLocations["BR"].Intersect(this.segments).ToList();
                segmentLocations["T"] = segmentLocations["T"].Intersect(this.segments).ToList();

                segmentLocations["TL"] = segmentLocations["TL"].Except(this.segments).ToList();
                segmentLocations["M"] = segmentLocations["M"].Except(this.segments).ToList();
                segmentLocations["BL"] = segmentLocations["BL"].Except(this.segments).ToList();
                segmentLocations["B"] = segmentLocations["B"].Except(this.segments).ToList();
            }
        }

        public bool TryHardOnes(Dictionary<string, DisplaySegment> segmentLocations)
        {
            // 8, 0, 2, 3, 5, 6, or 9

            if (
                this.segments.Count == 7 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TL"]) &&
                this.segments.Contains(segmentLocations["BL"]) &&
                this.segments.Contains(segmentLocations["B"]) &&
                this.segments.Contains(segmentLocations["BR"]) &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["M"])
                )
            {
                val = 8;
            }
            else if (
                this.segments.Count == 6 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TL"]) &&
                this.segments.Contains(segmentLocations["BL"]) &&
                this.segments.Contains(segmentLocations["B"]) &&
                this.segments.Contains(segmentLocations["BR"]) &&
                this.segments.Contains(segmentLocations["TR"])
                )
            {
                val = 0;
            }
            else if (
                this.segments.Count == 6 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TL"]) &&
                this.segments.Contains(segmentLocations["BL"]) &&
                this.segments.Contains(segmentLocations["B"]) &&
                this.segments.Contains(segmentLocations["BR"]) &&
                this.segments.Contains(segmentLocations["M"])
                )
            {
                val = 6;
            }
            else if (
                this.segments.Count == 6 &&
                this.segments.Contains(segmentLocations["B"]) &&
                this.segments.Contains(segmentLocations["BR"]) &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TL"]) &&
                this.segments.Contains(segmentLocations["M"])
                )
            {
                val = 9;
            }
            else if (
                this.segments.Count == 5 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["M"]) &&
                this.segments.Contains(segmentLocations["BL"]) &&
                this.segments.Contains(segmentLocations["B"])
                )
            {
                val = 2;
            }
            else if (
                this.segments.Count == 5 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TL"]) &&
                this.segments.Contains(segmentLocations["M"]) &&
                this.segments.Contains(segmentLocations["BR"]) &&
                this.segments.Contains(segmentLocations["B"])
                )
            {
                val = 5;
            }
            else if (
                this.segments.Count == 5 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["M"]) &&
                this.segments.Contains(segmentLocations["BR"]) &&
                this.segments.Contains(segmentLocations["B"])
                )
            {
                val = 3;
            }
            else if (
                this.segments.Count == 4 &&
                this.segments.Contains(segmentLocations["TL"]) &&
                this.segments.Contains(segmentLocations["M"]) &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["BR"])
                )
            {
                val = 4;
            }
            else if (
                this.segments.Count == 3 &&
                this.segments.Contains(segmentLocations["T"]) &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["BR"])
                )
            {
                val = 7;
            }
            else if (
                this.segments.Count == 2 &&
                this.segments.Contains(segmentLocations["TR"]) &&
                this.segments.Contains(segmentLocations["BR"])
                )
            {
                val = 1;
            }
            else
            {
                return false;
            }

            return true;
        }

        public override string ToString()
        {
            string strRep = "[";
            foreach (var s in segments)
                strRep += DisplaySegmentToChar(s);
            strRep += "]";
            return strRep;
        }
    }
}
