using System;
using System.Collections.Generic;
using coresearch;

namespace TrieTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Trie trie = new Trie();


            trie.Insert("ala", "value1");
            trie.Insert("alan", "value2");
            trie.Insert("alanek", "value3");
            trie.Insert("alek", "value4");


            Console.WriteLine($"Done. {trie.Size} inserted");
            trie.WriteAllData();

            while (true)
            {
                string userInput = Console.ReadLine();

                HashSet<string> results = trie.GetData(userInput);
                Node tr = trie.Prefix(userInput, trie.Root);
                Console.WriteLine($"{tr.GetHashCode()} {tr.Key}");

                foreach (string el in results)
                {
                    Console.WriteLine(el);
                }
            }
        }
    }
}
