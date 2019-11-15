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

            /**
            trie.Insert("one", "english");
            trie.Insert("two", "english");
            trie.Insert("three", "english");
            trie.Insert("four", "english");
            trie.Insert("five", "english");
            trie.Insert("six", "english");

            trie.Insert("four", "english");
            trie.Insert("five", "english");

            trie.Insert("two", "alice");
            trie.Insert("rabbit", "alice");
            trie.Insert("rabbit", "english");

            trie.Insert("ein", "german");
            trie.Insert("zwei", "german");
            trie.Insert("drei", "german");
            trie.Insert("vier", "german");
            trie.Insert("fünf", "german");
            trie.Insert("sechs", "german");

            trie.Insert("bier", "german");
            trie.Insert("vier", "german");

            trie.Insert("auto", "german");
            trie.Insert("auto", "czech");
            trie.Insert("auto", "french");
            trie.Insert("car", "english"); 
            **/

            trie.Insert("ala", "value1");
            trie.Insert("alan", "value2");
            trie.Insert("alanek", "value3");
            trie.Insert("alek", "value4");
            /*
            trie.Insert("roman", "value5");
            trie.Insert("romanus", "value6");
            trie.Insert("rome", "value7");
            trie.Insert("romanum", "value8");
            */

            Console.WriteLine($"Done. {trie.Size} nodes");

            while (true)
            {
                string userInput = Console.ReadLine();

                HashSet<string> results = trie.GetData(userInput);

                foreach (string el in results)
                {
                    Console.WriteLine(el);
                }
            }
        }
    }
}
