using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_19_Beacon_Scanner
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<Scanner> scanners = new List<Scanner>();

            Scanner newScanner = null;
            foreach (string line in inputList)
            {
                if (line.Length > 1 && line[1] == '-')
                {
                    if (newScanner != null)
                    {
                        newScanner.ConstructBeaconVectors();
                    }
                    newScanner = new Scanner(line);
                    scanners.Add(newScanner);
                }
                else if (line.Length > 0 && newScanner != null)
                {
                    newScanner.AddBeacon(line);
                }
            }
            scanners[scanners.Count - 1].ConstructBeaconVectors();

            scanners[0].FixedOrientation = 0;
            scanners[0].VectorFromS0 = new Point3D(0, 0, 0);
            scanners[0].CheckOverlapWithAllScanners(scanners);

            P1(scanners);
            P2(scanners);
        }

        static void P1(List<Scanner> scanners)
        {
            List<Point3D> beaconsFromS0 = new List<Point3D>();
            foreach (Scanner scanner in scanners)
            {
                foreach (Beacon beacon in scanner.Beacons)
                {
                    Point3D vectorFromS0 = scanner.VectorFromS0 + beacon.Orientations[scanner.FixedOrientation];
                    if (!beaconsFromS0.Contains(vectorFromS0))
                        beaconsFromS0.Add(vectorFromS0);
                }
            }
            Console.WriteLine(beaconsFromS0.Count);
            Console.ReadLine();
        }

        static void P2(List<Scanner> scanners)
        {
            int largestManhattanDistance = int.MinValue;
            foreach (Scanner scanner1 in scanners)
            {
                foreach (Scanner scanner2 in scanners)
                {
                    if (scanner2 != scanner1)
                    {
                        int manhattanDistance = (scanner2.VectorFromS0 - scanner1.VectorFromS0).ManhattanMagnitude;
                        if (manhattanDistance > largestManhattanDistance)
                            largestManhattanDistance = manhattanDistance;
                    }
                }
            }
            Console.WriteLine(largestManhattanDistance);
            Console.ReadLine();
        }
    }

    public class Scanner
    {
        public int ID;
        public List<Beacon> Beacons = new List<Beacon>();
        public int FixedOrientation = -1;
        public Point3D VectorFromS0;

        public Scanner(string line)
        {
            string[] lineSplit = line.Split(' ');
            ID = int.Parse(lineSplit[2]);
        }

        public void AddBeacon(string line)
        {
            Beacons.Add(new Beacon(line));
        }

        public void ConstructBeaconVectors()
        {
            foreach (Beacon beacon in Beacons)
            {
                beacon.ConstructVectorsToOtherBeacons(Beacons);
            }
        }

        public void CheckOverlapWithAllScanners(List<Scanner> scanners)
        {
            foreach (Scanner otherScanner in scanners)
            {
                if (otherScanner != this && otherScanner.FixedOrientation == -1)
                {
                    (int, Point3D) result = this.CheckOverlapWithScanner(otherScanner);
                    int correctOrientation = result.Item1;
                    if (correctOrientation != -1)
                    {
                        otherScanner.FixedOrientation = correctOrientation;
                        otherScanner.VectorFromS0 = VectorFromS0 + result.Item2;

                        Console.WriteLine($"S{this.ID} overlaps with S{otherScanner.ID}, therefore S{otherScanner.ID} is at {otherScanner.VectorFromS0} from S0");

                        otherScanner.CheckOverlapWithAllScanners(scanners);
                    }
                }
            }
        }

        public (int, Point3D) CheckOverlapWithScanner(Scanner otherScanner)
        {
            // foreach of the 24 orientations of other scanner
            for (int orientation = 0; orientation < 24; orientation++)
            {
                // need to look for beacons that follow the same relative offset to eachother
                // must find at least 12 to be an overlapping scanner with correct offset

                // all beacons know how to get to the other beacons on the same scanner
                // need to find one beacon on this scanner that is in overlap region so iterate through all
                // also iterate through all beacons on other scanner
                // if the beacon on this scanner and the beacon on the other scanner share at least 12 vectors to other beacons

                foreach (Beacon beacon in Beacons)
                {
                    foreach (Beacon otherScannerBeacon in otherScanner.Beacons)
                    {
                        List<Point3D> vectorsFromBeaconToBeaconsOnThisScanner = beacon.VectorsToOtherBeaconsByOrientation[FixedOrientation];
                        List<Point3D> vectorsFromOtherScannerBeaconToBeaconsOnOtherScanner = otherScannerBeacon.VectorsToOtherBeaconsByOrientation[orientation];

                        int numCommonVectors = 0;

                        // need to see if there are at least 12 common vectors
                        foreach (Point3D vectorFromBeaconToBeaconsOnThisScanner in vectorsFromBeaconToBeaconsOnThisScanner)
                        {
                            foreach (Point3D vectorFromOtherScannerBeaconToBeaconsOnOtherScanner in vectorsFromOtherScannerBeaconToBeaconsOnOtherScanner)
                            {
                                if (vectorFromBeaconToBeaconsOnThisScanner.Equals(vectorFromOtherScannerBeaconToBeaconsOnOtherScanner))
                                {
                                    // common vector
                                    numCommonVectors++;
                                }
                            }
                        }

                        if (numCommonVectors >= 11) // the last one is implied because if we've made 11 routes from same start then the start is also common
                        {
                            // these scanners overlap
                            Point3D vectorToOtherScannerFromThisScanner = beacon.Orientations[FixedOrientation] - otherScannerBeacon.Orientations[orientation];
                            return (orientation, vectorToOtherScannerFromThisScanner);
                        }
                    }
                }
            }
            return (-1, null);
        }
    }

    public class Beacon
    {
        public Point3D[] Orientations = new Point3D[24];
        public List<Point3D>[] VectorsToOtherBeaconsByOrientation = new List<Point3D>[24];

        public Beacon(string line)
        {
            string[] lineSplit = line.Split(',');
            int rX = int.Parse(lineSplit[0]);
            int rY = int.Parse(lineSplit[1]);
            int rZ = int.Parse(lineSplit[2]);

            // 6 faces
            // 4 rotations at each face (x fixed)

            // Face 1
            Orientations[0] = new Point3D(rX, rY, rZ);
            Orientations[1] = new Point3D(rX, rZ, -rY);
            Orientations[2] = new Point3D(rX, -rY, -rZ);
            Orientations[3] = new Point3D(rX, -rZ, rY);
            // Face 2
            Orientations[4] = new Point3D(-rY, rX, rZ);
            Orientations[5] = new Point3D(-rZ, rX, -rY);
            Orientations[6] = new Point3D(rY, rX, -rZ);
            Orientations[7] = new Point3D(rZ, rX, rY);
            // Face 3
            Orientations[8] = new Point3D(-rX, -rY, rZ);
            Orientations[9] = new Point3D(-rX, -rZ, -rY);
            Orientations[10] = new Point3D(-rX, rY, -rZ);
            Orientations[11] = new Point3D(-rX, rZ, rY);
            // Face 4
            Orientations[12] = new Point3D(rY, -rX, rZ);
            Orientations[13] = new Point3D(rZ, -rX, -rY);
            Orientations[14] = new Point3D(-rY, -rX, -rZ);
            Orientations[15] = new Point3D(-rZ, -rX, rY);
            // Face 5
            Orientations[16] = new Point3D(-rZ, rY, rX);
            Orientations[17] = new Point3D(rY, rZ, rX);
            Orientations[18] = new Point3D(rZ, -rY, rX);
            Orientations[19] = new Point3D(-rY, -rZ, rX);
            // Face 6
            Orientations[20] = new Point3D(rZ, rY, -rX);
            Orientations[21] = new Point3D(-rY, rZ, -rX);
            Orientations[22] = new Point3D(-rZ, -rY, -rX);
            Orientations[23] = new Point3D(rY, -rZ, -rX);
        }

        public void ConstructVectorsToOtherBeacons(List<Beacon> allBeaconsInSameScanner)
        {
            for (int orientation = 0; orientation < 24; orientation++)
            {
                VectorsToOtherBeaconsByOrientation[orientation] = new List<Point3D>();
                foreach (Beacon otherBeacon in allBeaconsInSameScanner)
                {
                    if (otherBeacon != this)
                    {
                        VectorsToOtherBeaconsByOrientation[orientation].Add(otherBeacon.Orientations[orientation] - Orientations[orientation]);
                    }
                }
            }
        }
    }

    public class Point3D
    {
        public int X;
        public int Y;
        public int Z;

        public int ManhattanMagnitude
        {
            get
            {
                return X + Y + Z;
            }
        }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D left, Point3D right)
        {
            return new Point3D(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Point3D operator -(Point3D left, Point3D right)
        {
            return new Point3D(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }

        public override bool Equals(object obj)
        {
            Point3D otherPoint3D = obj as Point3D;
            if (otherPoint3D != null)
                return this.X == otherPoint3D.X && this.Y == otherPoint3D.Y && this.Z == otherPoint3D.Z;
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }
    }
}
