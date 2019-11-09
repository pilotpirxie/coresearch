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
            foreach (string file in Directory.EnumerateFiles("./bbc", "*.txt", SearchOption.AllDirectories))
            {
                string contents = File.ReadAllText(file);
                coresearch.Insert(file, contents, file);
                i++;
            }

            Console.WriteLine($"Files added {i}");
            Console.WriteLine($"Words inserted ${coresearch.Count}");

            while (true)
            {
                string userInput = Console.ReadLine();

                List<string> results = coresearch.Get(userInput);

                foreach (string el in results)
                {
                    Console.WriteLine(el);
                }
            }
        }
    }
}
