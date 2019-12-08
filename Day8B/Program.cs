using System;
using System.IO;
using System.Linq;
using MoreLinq;

namespace Day8B
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = "Day8.txt";
            int width = 25;
            int height = 6;
            var text = File.ReadAllText(inputFile);
            var imageLayers = text.Batch(width * height).Select(x => x.ToList()).ToList();

            var maxZeroLayer = imageLayers.OrderBy(x => x.Count(y => y == '0')).First();
            var product = maxZeroLayer.Count(x => x == '1') * maxZeroLayer.Count(x => x == '2');

            Console.WriteLine($"The product is {product}");

            var mergedImage = Enumerable.Range(0, width * height)
                .Select(p => Enumerable.Range(0, imageLayers.Count)
                    .Select(l => imageLayers[l][p])
                    .Aggregate('2', (a, b) => a != '2' ? a : b))
                .Select(x => x == '0' ? ' ' : x)
                .Batch(width);

            Console.WriteLine(string.Join('\n', mergedImage.Select(x => string.Join(' ', x))));
        }
    }
}
