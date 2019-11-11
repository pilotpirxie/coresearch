using System;
using System.Collections.Generic;
using System.IO;

namespace coresearch
{
    class Program
    {
        static void Main(string[] args)
        {
            Coresearch coresearch = new Coresearch();

            int i = 0;
            foreach (string file in Directory.EnumerateFiles("./", "*.txt", SearchOption.AllDirectories))
            {
                foreach (string line in File.ReadLines(file))
                {
                    coresearch.InsertResource(file, line, file);
                    if (i % 10000 == 0)
                    {
                        GC.Collect();
                    }
                }

                GC.Collect();
                i++;
            }

            Console.WriteLine($"Files added {i}");
            Console.WriteLine($"Words inserted {coresearch.Count}");

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
