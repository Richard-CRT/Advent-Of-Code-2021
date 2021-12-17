using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_16_Packet_Decoder
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            string packetBinaryString = "";

            foreach (char c in inputList[0])
            {
                int hexDigit = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
                packetBinaryString += Convert.ToString(hexDigit, 2).PadLeft(4, '0');
            }

            Packet topLevelPacket = new Packet(packetBinaryString, 0);

            P1(topLevelPacket);
            P2(topLevelPacket);
        }

        static void P1(Packet topLevelPacket)
        {
            Console.WriteLine(topLevelPacket.SumVersionNumbers());
            Console.ReadLine();
        }

        static void P2(Packet topLevelPacket)
        {
            Console.WriteLine(topLevelPacket.Value);
            Console.ReadLine();
        }
    }

    public class Packet
    {
        public int BitLength = 0;
        public int Version;
        public int Type;
        private Int64 _literalValue = 0;
        public int LengthTypeId;
        public List<Packet> SubPackets = new List<Packet>();

        public Int64 Value
        {
            get
            {
                switch (Type)
                {
                    case 4:
                        {
                            return _literalValue;
                        }
                    case 0:
                        {
                            Int64 sum = 0;
                            foreach (Packet subPacket in SubPackets)
                                sum += subPacket.Value;
                            return sum;
                        }
                    case 1:
                        {
                            Int64 product = 1;
                            foreach (Packet subPacket in SubPackets)
                                product *= subPacket.Value;
                            return product;
                        }
                    case 2:
                        {
                            Int64 minVal = int.MaxValue;
                            foreach (Packet subPacket in SubPackets)
                                if (subPacket.Value < minVal)
                                    minVal = subPacket.Value;
                            return minVal;
                        }
                    case 3:
                        {
                            Int64 maxVal = int.MinValue;
                            foreach (Packet subPacket in SubPackets)
                                if (subPacket.Value > maxVal)
                                    maxVal = subPacket.Value;
                            return maxVal;
                        }
                    case 5:
                        {
                            return SubPackets[0].Value > SubPackets[1].Value ? 1 : 0;
                        }
                    case 6:
                        {
                            return SubPackets[0].Value < SubPackets[1].Value ? 1 : 0;
                        }
                    case 7:
                        {
                            return SubPackets[0].Value == SubPackets[1].Value ? 1 : 0;
                        }
                    default:
                        return 0;
                }
            }
        }

        public Packet(string packetBinaryString, int stringStartLoc)
        {
            int stringParseLoc = stringStartLoc;

            Version = Convert.ToInt32(packetBinaryString.Substring(stringParseLoc, 3), 2);
            stringParseLoc += 3;
            Type = Convert.ToInt32(packetBinaryString.Substring(stringParseLoc, 3), 2);
            stringParseLoc += 3;

            if (Type == 4)
            {
                // literal
                string groupOf5;
                do
                {
                    groupOf5 = packetBinaryString.Substring(stringParseLoc, 5);
                    stringParseLoc += 5;
                    Int64 nextBits = Convert.ToInt64(groupOf5.Substring(1), 2);
                    _literalValue = (_literalValue << 4) | nextBits;
                }
                while (groupOf5[0] == '1');
            }
            else
            {
                // operator
                LengthTypeId = Convert.ToInt32(packetBinaryString.Substring(stringParseLoc, 1), 2);
                stringParseLoc += 1;

                if (LengthTypeId == 1)
                {
                    // number of sub-packets immediately contained

                    int numberOfSubPackets = Convert.ToInt32(packetBinaryString.Substring(stringParseLoc, 11), 2);
                    stringParseLoc += 11;

                    for (int i = 0; i < numberOfSubPackets; i++)
                    {
                        Packet newSubPacket = new Packet(packetBinaryString, stringParseLoc);
                        SubPackets.Add(newSubPacket);
                        stringParseLoc += newSubPacket.BitLength;
                    }
                }
                else if (LengthTypeId == 0)
                {
                    // total length in bits of the sub-packets

                    int lengthInBitsOfSubPackets = Convert.ToInt32(packetBinaryString.Substring(stringParseLoc, 15), 2);
                    stringParseLoc += 15;

                    int endOfSubPacketsStringParseLoc = stringParseLoc + lengthInBitsOfSubPackets;
                    while (stringParseLoc < endOfSubPacketsStringParseLoc)
                    {
                        Packet newSubPacket = new Packet(packetBinaryString, stringParseLoc);
                        SubPackets.Add(newSubPacket);
                        stringParseLoc += newSubPacket.BitLength;
                    }
                }
            }

            BitLength = stringParseLoc - stringStartLoc;
        }

        public override string ToString()
        {
            if (Type == 4)
                return $"Literal - {Value}";
            else
                return $"Operator - {SubPackets.Count} SubPackets";
        }

        public int SumVersionNumbers()
        {
            int subSumVersion = 0;
            foreach (Packet packet in SubPackets)
                subSumVersion += packet.SumVersionNumbers();
            return Version + subSumVersion;
        }
    }
}
