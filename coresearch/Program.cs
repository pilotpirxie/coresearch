using System;
using System.Collections.Generic;
using System.IO;

namespace coresearch
{
    class Program
    {
        static void Main(string[] args)
        {
            // sample data http://mlg.ucd.ie/datasets/bbc.html
            Coresearch coresearch = new Coresearch(true);
            int filesCount = 0;

            foreach (string file in Directory.EnumerateFiles("./", "*.txt", SearchOption.AllDirectories))
            {
                filesCount++;
                foreach (string line in File.ReadLines(file))
                {
                    coresearch.InsertResource(file, line.Replace(";", ""));
                }
            }

            Console.WriteLine($"Words inserted {coresearch.Count} from {filesCount} files with memory usage of {GC.GetTotalMemory(false)}");

            while (true)
            {
                string userInput = Console.ReadLine();

                List<string> results = coresearch.Get(userInput);
                Console.WriteLine($"{results.Count} results for {userInput}");

                foreach (string el in results)
                {
                    Console.WriteLine(el);
                }
            }
        }
    }
}
