using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day8.txt";

            var text = File.ReadAllText(inputFile);
            var image = ConstructImage(text, 25, 6);
            var product = AnalyzeImage(image, 25, 6);

            Console.WriteLine($"The product was {product}");

            var message = AnalyzeImage2(image, 25, 6);
            for(int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 25; x++)
                {
                    if (message[x, y] == 0)
                        Console.Write(' ');
                    else
                        Console.Write(message[x, y]);
                }
                Console.WriteLine();
            }
        }

        public static IList<int[,]> ConstructImage(string imageInput, int width, int height)
        {
            var layers = imageInput.Length / (width * height);
            var images = new List<int[,]>();

            int i = 0;

            for (int l = 0; l < layers; l++)
            {
                var image = new int[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        image[x, y] = (int)char.GetNumericValue(imageInput[i++]);
                    }
                }
                images.Add(image);
            }

            return images;
        }

        public static int AnalyzeImage(IList<int[,]> image, int width, int height)
        {
            var maxLayer = image.OrderBy(x => x.Cast().Count(y => y == 0)).First();

            var items = image[0].Cast();
            var counts = image.Select(x => x.Cast().Count(y => y == 0)).ToList();
            var max = counts.Max();
            var maxLayerIndex = counts.FindIndex(x => x == max);

            var imageLayer = image[maxLayerIndex];
            return maxLayer.Cast().Count(x => x == 1) * maxLayer.Cast().Count(x => x == 2);
        }

        public static int[,] AnalyzeImage2(IList<int[,]> image, int width, int height)
        {
            var result = new int[width, height];

            for(int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    result[x, y] = GetMaskedPixel(image.Select(i => i[x, y]));
            }

            return result;

            int GetMaskedPixel(IEnumerable<int> pixels) =>
                pixels.Aggregate(2, (a, b) => a == 2 ? b : a);
        }
    }
}
