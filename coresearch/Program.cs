using System;
using System.Collections.Generic;
using System.IO;

namespace coresearch
{
    static class Program
    {
        private static int _filesCount = 0;
        private static Coresearch _coresearch;
        private static string prompt = "> ";
        private static void LoadFromSource(string path, string extension)
        {
            foreach (string file in Directory.EnumerateFiles(path, extension, SearchOption.AllDirectories))
            {
                _filesCount++;
                foreach (string line in File.ReadLines(file))
                {
                    _coresearch.InsertResource(file, line);
                }
            }

            Console.WriteLine($"Words inserted {_coresearch.Count} from {_filesCount} files with memory usage of {GC.GetTotalMemory(false)}");
        }

        private static void Search(string key)
        {
            List<string> results = _coresearch.Get(key);
            Console.WriteLine($"{results.Count} results for {key}");

            foreach (string el in results)
            {
                Console.WriteLine(el);
            }
        }

        private static void Insert(string resourceName, string content)
        {
            _coresearch.InsertResource(resourceName, content);
        }

        private static void Delete(string key)
        {
            _coresearch.Remove(key);
        }

        private static void Echo(string content)
        {
            Console.WriteLine(_coresearch.Echo(content));
        }

        private static void Flush()
        {
                _coresearch.Flush();
        }

        static void Main(string[] args)
        {
            // sample data http://mlg.ucd.ie/datasets/bbc.html
            _coresearch = new Coresearch(true);

            while (true)
            {
                Console.Write(prompt);
                string userInput = Console.ReadLine();

                string[] command = userInput.Split(' ');

                switch (command[0])
                {
                    case "source":
                    case "load":
                        if (command.Length == 3) LoadFromSource(command[1], command[2]);
                        break;
                    case "get":
                    case "search":
                        if (command.Length == 2) Search(command[1]);
                        break;
                    case "add":
                    case "insert":
                        if (command.Length == 3) Insert(command[1], command[2]);
                        break;
                    case "delete":
                        if (command.Length == 2) Delete(command[1]);
                        break;
                    case "echo":
                        if (command.Length == 2) Echo(command[1]);
                        break;
                    case "flush":
                        Flush();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
