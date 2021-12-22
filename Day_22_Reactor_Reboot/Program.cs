using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_22_Reactor_Reboot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            List<Action> actions = inputList.Select(x => new Action(x)).ToList();

            P1(actions);
            P2(actions);
        }

        static void P1(List<Action> actions)
        {
            // Keeping track of each individual cube, knowing full well this isn't going to work
            //      for P2 but it's fast, and I don't know what I will need to implement for P2
            // This is horrible but it works and I'm keeping it as an example of something quick to write
            Dictionary<int, Dictionary<int, Dictionary<int, bool>>> onCubes = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

            foreach (Action action in actions)
            {
                for (int x = Math.Max(action.xMin, -50); x <= Math.Min(action.xMax, 50); x++)
                {
                    Dictionary<int, Dictionary<int, bool>> onCubesX = null;
                    if (onCubes.ContainsKey(x))
                        onCubesX = onCubes[x];
                    else if (action.On)
                    {
                        onCubesX = new Dictionary<int, Dictionary<int, bool>>();
                        onCubes[x] = onCubesX;
                    }

                    if (onCubesX != null)
                    {
                        for (int y = Math.Max(action.yMin, -50); y <= Math.Min(action.yMax, 50); y++)
                        {
                            Dictionary<int, bool> onCubesXY = null;
                            if (onCubesX.ContainsKey(y))
                                onCubesXY = onCubesX[y];
                            else if (action.On)
                            {
                                onCubesXY = new Dictionary<int, bool>();
                                onCubesX[y] = onCubesXY;
                            }

                            if (onCubesXY != null)
                            {
                                for (int z = Math.Max(action.zMin, -50); z <= Math.Min(action.zMax, 50); z++)
                                {
                                    if (action.On)
                                    {
                                        if (!onCubesXY.ContainsKey(z))
                                            onCubesXY[z] = true;
                                    }
                                    else
                                    {
                                        if (onCubesXY.ContainsKey(z))
                                            onCubesXY.Remove(z);
                                    }
                                }

                                if (!action.On && onCubesXY.Count == 0)
                                    onCubesX.Remove(y);
                            }
                        }

                        if (!action.On && onCubesX.Count == 0)
                            onCubes.Remove(x);
                    }
                }
            }

            int onCubeCount = 0;
            foreach (var kVPx in onCubes)
            {
                foreach (var kVPy in kVPx.Value)
                {
                    onCubeCount += kVPy.Value.Count;
                }
            }

            Console.WriteLine(onCubeCount);
            Console.ReadLine();
        }

        static void P2(List<Action> actions)
        {
            // Cube splitting
            // This solution would also be appropriate for P1 with a few Math.Min(???, 50) and Math.Max(???, -50) calls
            List<Cuboid> onCuboids = new List<Cuboid>();
            foreach (var action in actions)
            {
                Cuboid newCuboid = new Cuboid(action.xMin, action.xMax, action.yMin, action.yMax, action.zMin, action.zMax);
                List<Cuboid> cuboidsToBeAdded = new List<Cuboid>();
                for (int i = 0; i < onCuboids.Count;)
                {
                    Cuboid onCuboid = onCuboids[i];
                    var (overlaps, newCuboids) = onCuboid.RemoveCube(newCuboid);
                    if (overlaps)
                    {
                        onCuboids.RemoveAt(i);
                        cuboidsToBeAdded.AddRange(newCuboids);
                    }
                    else
                        i++;
                }
                onCuboids.AddRange(cuboidsToBeAdded);
                if (action.On)
                    onCuboids.Add(newCuboid);
            }

            Int64 onCuboidCount = 0;
            foreach (Cuboid nonOverlappingOnCuboid in onCuboids)
            {
                onCuboidCount += (Int64)(nonOverlappingOnCuboid.xMax - nonOverlappingOnCuboid.xMin + 1)
                    * (Int64)(nonOverlappingOnCuboid.yMax - nonOverlappingOnCuboid.yMin + 1)
                    * (Int64)(nonOverlappingOnCuboid.zMax - nonOverlappingOnCuboid.zMin + 1);
            }

            Console.WriteLine(onCuboidCount);
            Console.ReadLine();
        }
    }
}

public class Action
{
    public bool On;
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;
    public int zMin;
    public int zMax;

    public Action(string actionStr)
    {
        On = actionStr[1] == 'n';
        string[] split = actionStr.Split(',');
        string[] xsplit = split[0].Split('=')[1].Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
        string[] ysplit = split[1].Split('=')[1].Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
        string[] zsplit = split[2].Split('=')[1].Split(new string[] { ".." }, StringSplitOptions.RemoveEmptyEntries);
        xMin = int.Parse(xsplit[0]);
        xMax = int.Parse(xsplit[1]);
        yMin = int.Parse(ysplit[0]);
        yMax = int.Parse(ysplit[1]);
        zMin = int.Parse(zsplit[0]);
        zMax = int.Parse(zsplit[1]);
    }

    public override string ToString()
    {
        if (On)
            return $"on x={xMin}..{xMax},y={yMin}..{yMax},z={zMin}..{zMax}";
        else
            return $"off x={xMin}..{xMax},y={yMin}..{yMax},z={zMin}..{zMax}";
    }
}

public class Cuboid
{
    public int xMin;
    public int xMax;
    public int yMin;
    public int yMax;
    public int zMin;
    public int zMax;

    public Cuboid(int _xMin, int _xMax, int _yMin, int _yMax, int _zMin, int _zMax)
    {
        xMin = _xMin;
        xMax = _xMax;
        yMin = _yMin;
        yMax = _yMax;
        zMin = _zMin;
        zMax = _zMax;
    }

    public bool EncompassedBy(Cuboid otherCuboid)
    {
        return otherCuboid.xMin <= xMin && otherCuboid.xMax >= xMax
            && otherCuboid.yMin <= yMin && otherCuboid.yMax >= yMax
            && otherCuboid.zMin <= zMin && otherCuboid.zMax >= zMax;
    }

    public bool Overlaps(Cuboid otherCuboid)
    {
        if (xMax < otherCuboid.xMin || xMin > otherCuboid.xMax)
            return false;
        else if (yMax < otherCuboid.yMin || yMin > otherCuboid.yMax)
            return false;
        else if (zMax < otherCuboid.zMin || zMin > otherCuboid.zMax)
            return false;
        return true;
    }

    public (bool, List<Cuboid>) RemoveCube(Cuboid otherCuboid)
    {
        if (!this.Overlaps(otherCuboid))
            return (false, null);

        List<Cuboid> cuboids = new List<Cuboid>() { this };
        List<Cuboid> newCuboids;

        newCuboids = new List<Cuboid>();
        foreach (Cuboid cuboid in cuboids)
        {
            var (cuboid1, cuboid2) = cuboid.CutX(otherCuboid.xMin);
            if (cuboid1 != null)
                newCuboids.Add(cuboid1);
            if (cuboid2 != null)
                newCuboids.Add(cuboid2);
        }
        cuboids = newCuboids;

        newCuboids = new List<Cuboid>();
        foreach (Cuboid cuboid in cuboids)
        {
            var (cuboid1, cuboid2) = cuboid.CutX(otherCuboid.xMax + 1);
            if (cuboid1 != null)
                newCuboids.Add(cuboid1);
            if (cuboid2 != null)
                newCuboids.Add(cuboid2);
        }
        cuboids = newCuboids;

        newCuboids = new List<Cuboid>();
        foreach (Cuboid cuboid in cuboids)
        {
            var (cuboid1, cuboid2) = cuboid.CutY(otherCuboid.yMin);
            if (cuboid1 != null)
                newCuboids.Add(cuboid1);
            if (cuboid2 != null)
                newCuboids.Add(cuboid2);
        }
        cuboids = newCuboids;

        newCuboids = new List<Cuboid>();
        foreach (Cuboid cuboid in cuboids)
        {
            var (cuboid1, cuboid2) = cuboid.CutY(otherCuboid.yMax + 1);
            if (cuboid1 != null)
                newCuboids.Add(cuboid1);
            if (cuboid2 != null)
                newCuboids.Add(cuboid2);
        }
        cuboids = newCuboids;

        newCuboids = new List<Cuboid>();
        foreach (Cuboid cuboid in cuboids)
        {
            var (cuboid1, cuboid2) = cuboid.CutZ(otherCuboid.zMin);
            if (cuboid1 != null)
                newCuboids.Add(cuboid1);
            if (cuboid2 != null)
                newCuboids.Add(cuboid2);
        }
        cuboids = newCuboids;

        newCuboids = new List<Cuboid>();
        foreach (Cuboid cuboid in cuboids)
        {
            var (cuboid1, cuboid2) = cuboid.CutZ(otherCuboid.zMax + 1);
            if (cuboid1 != null)
                newCuboids.Add(cuboid1);
            if (cuboid2 != null)
                newCuboids.Add(cuboid2);
        }
        cuboids = newCuboids;

        for (int i = 0; i < cuboids.Count;)
        {
            if (cuboids[i].EncompassedBy(otherCuboid))
                cuboids.RemoveAt(i);
            else
            {
                if (cuboids[i].Overlaps(otherCuboid))
                    throw new Exception();
                i++;
            }
        }

        return (true, cuboids);
    }

    public (Cuboid, Cuboid) CutX(int x)
    {
        if (x > xMin && x <= xMax)
        {
            return (new Cuboid(xMin, x - 1, yMin, yMax, zMin, zMax), new Cuboid(x, xMax, yMin, yMax, zMin, zMax));
        }
        else
        {
            return (this, null);
        }
    }

    public (Cuboid, Cuboid) CutY(int y)
    {
        if (y > yMin && y <= yMax)
        {
            return (new Cuboid(xMin, xMax, yMin, y - 1, zMin, zMax), new Cuboid(xMin, xMax, y, yMax, zMin, zMax));
        }
        else
        {
            return (this, null);
        }
    }

    public (Cuboid, Cuboid) CutZ(int z)
    {
        if (z > zMin && z <= zMax)
        {
            return (new Cuboid(xMin, xMax, yMin, yMax, zMin, z - 1), new Cuboid(xMin, xMax, yMin, yMax, z, zMax));
        }
        else
        {
            return (this, null);
        }
    }
}
