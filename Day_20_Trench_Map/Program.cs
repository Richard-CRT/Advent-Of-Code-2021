using AdventOfCodeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_20_Trench_Map
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> inputList = AoCUtilities.GetInputLines();

            string algorithmStr = inputList[0];

            ImageContainer imageContainer = new ImageContainer();

            int i = 2;
            while (i < inputList.Count && inputList[i] != "")
            {
                imageContainer.AddInitialLine(inputList[i]);

                i++;
            }

            P1(imageContainer, algorithmStr);
            P2(imageContainer, algorithmStr);
        }

        static void P1(ImageContainer imageContainer, string algorithmStr)
        {
            //imageContainer.Display();
            imageContainer.Enhance(algorithmStr);
            //imageContainer.Display();
            imageContainer.Enhance(algorithmStr);
            //imageContainer.Display();

            Console.WriteLine(imageContainer.CountLitPixel());
            Console.ReadLine();
        }

        static void P2(ImageContainer imageContainer, string algorithmStr)
        {
            for (int i = 2; i < 50;i++)
            {
                imageContainer.Enhance(algorithmStr);
                //imageContainer.Display();
            }

            Console.WriteLine(imageContainer.CountLitPixel());
            Console.ReadLine();
        }
    }

    public class ImageContainer
    {
        public bool InfinitePixel = false;
        public List<List<bool>> image = new List<List<bool>>();
        public int Right = 0;
        public int Bottom = -1;
        public int Left = 0;
        public int Top = 0;

        public void AddInitialLine(string line)
        {
            List<bool> pixelLine = new List<bool>();
            Right = line.Length - 1; // this assumes input image is square
            foreach (char c in line)
            {
                if (c == '#')
                    pixelLine.Add(true);
                else if (c == '.')
                    pixelLine.Add(false);
            }
            image.Add(pixelLine);
            Bottom++;
        }

        public void Enhance(string algorithm)
        {
            // add columns & rows at edges

            int newLeft = Left - 1;
            int newTop = Top - 1;
            int newRight = Right + 1;
            int newBottom = Bottom + 1;
            List<List<bool>> newImage = new List<List<bool>>();
            newImage.Add(Enumerable.Repeat(InfinitePixel, newRight - newLeft + 1).ToList());
            for (int y = 0; y <= Bottom - Top; y++)
            {
                List<bool> temp = new List<bool>(image[y]);
                temp.Insert(0, InfinitePixel);
                temp.Add(InfinitePixel);
                newImage.Add(temp);
            }
            newImage.Add(Enumerable.Repeat(InfinitePixel, newRight - newLeft + 1).ToList());

            for (int y = newTop; y <= newBottom; y++)
            {
                for (int x = newLeft; x <= newRight; x++)
                {
                    newImage[y - newTop][x - newLeft] = EnhancePixel(algorithm, x, y);
                }
            }

            bool newInfinitePixel = EnhanceInfinitePixel(algorithm);

            Left = newLeft;
            Top = newTop;
            Right = newRight;
            Bottom = newBottom;
            image = newImage;
            InfinitePixel = newInfinitePixel;

            bool topRowChanged = false;
            bool bottomRowChanged = false;
            for (int x = Left; x <= Right; x++)
            {
                if (GetPixel(x,Top) != InfinitePixel)
                    topRowChanged = true;
                if (GetPixel(x, Bottom) != InfinitePixel)
                    bottomRowChanged = true;
                if (topRowChanged && bottomRowChanged)
                    break;
            }
            bool leftColumnChanged = false;
            bool rightColumnChanged = false;
            for (int y = Top; y <= Bottom; y++)
            {
                if (GetPixel(Left, y) != InfinitePixel)
                    leftColumnChanged = true;
                if (GetPixel(Right, y) != InfinitePixel)
                    rightColumnChanged = true;
                if (leftColumnChanged && rightColumnChanged)
                    break;
            }

            if (!topRowChanged)
            {
                Top++;
                image.RemoveAt(0);
            }
            if (!bottomRowChanged)
            {
                Bottom--;
                image.RemoveAt(image.Count - 1);
            }
            if (!leftColumnChanged)
            {
                Left++;
                foreach (var line in image)
                {
                    line.RemoveAt(0);
                }
            }
            if (!rightColumnChanged)
            {
                Right--;
                foreach (var line in image)
                {
                    line.RemoveAt(line.Count - 1);
                }
            }
        }

        public bool EnhanceInfinitePixel(string algorithm)
        {
            int val = InfinitePixel ? 0b111111111 : 0b000000000;
            return algorithm[val] == '#' ? true : false;
        }

        public bool EnhancePixel(string algorithm, int x, int y)
        {
            int val = 0;
            for (int dy = -1; dy <= 1; dy++)
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    val = (val << 1) | (GetPixel(x + dx, y + dy) ? 1 : 0);
                }
            }
            return algorithm[val] == '#' ? true : false;
        }

        public void Display()
        {
            Console.WriteLine($"Infinite Pixel {(InfinitePixel ? '#' : '.')}");
            for (int y = Top; y <= Bottom; y++)
            {
                for (int x = Left; x <= Right; x++)
                {
                    if (GetPixel(x, y))
                        Console.Write("#");
                    else
                        Console.Write(".");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool GetPixel(int x, int y)
        {
            if (x < Left || x > Right || y < Top || y > Bottom)
                return InfinitePixel;
            else
                return image[y - Top][x - Left];
        }

        public int CountLitPixel()
        {
            if (InfinitePixel)
                throw new Exception();
            else
            {
                int count = 0;
                for (int y = Top; y <= Bottom; y++)
                {
                    for (int x = Left; x <= Right; x++)
                    {
                        if (GetPixel(x, y))
                            count++;
                    }
                }
                return count;
            }
        }
    }
}
