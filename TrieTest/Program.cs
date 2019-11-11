using System;
using coresearch;

namespace TrieTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Trie trie = new Trie();
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


            Console.WriteLine("Done.");
            Console.ReadKey();
        }
    }
}
