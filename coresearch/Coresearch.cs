using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace coresearch
{
    public class Coresearch
    {
        private Trie words = new Trie();
        private int _count = 0;
        private bool _debug = false;
        private Regex rgx = new Regex("[^a-zA-Z0-9 -]");

        public Coresearch(bool debug)
        {
            _debug = debug;
        }

        public bool Debug { get => _debug; set => _debug = value; }

        public int Count { get => _count; }

        private void AddKey(string word, string resourceName)
        {
            _count++;

            string wordToInsert = PreProcessWord(word);

            if (_debug && Count % 50000 == 0)
            {
                Console.WriteLine($"Batch {Count} with total {words.Size}");
            }

            words.Insert(word, resourceName);
            
        }

        public void InsertResource(string resourceName, string content, string meta)
        {
            string[] contentWords = content.Split(' ');

            foreach (string word in contentWords)
            {
                AddKey(word, resourceName);
            }
        }

        public string PreProcessWord(string word)
        {
            string wordToReturn = rgx.Replace(word, "");
            wordToReturn = wordToReturn.Trim();
            wordToReturn = wordToReturn.ToLower();

            return wordToReturn;
        }

        public List<string> Get(string word)
        {
            string wordToSearch = PreProcessWord(word);

            List<string> toReturn = new List<string>();

            HashSet<string> hs = words.GetData(wordToSearch);

            foreach (string el in hs)
            {
                toReturn.Add(el);
            }

            return toReturn;
        }
    }
}
