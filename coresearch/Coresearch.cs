using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace coresearch
{
    public class Coresearch
    {
        private Trie trie = new Trie();
        private int _count = 0;
        private bool _debug = false;
        private Regex rgx = new Regex("[^a-zA-Z0-9 -]");

        public Coresearch(bool debug)
        {
            _debug = debug;
        }

        public bool Debug { get => _debug; set => _debug = value; }

        public int Count { get => _count; }

        private void AddKey(string key, string resourceName)
        {
            _count++;

            string wordToInsert = PreProcessWord(key);

            if (_debug && Count % 50000 == 0)
            {
                Console.WriteLine($"Batch {Count} with total {trie.Size} nodes");
            }

            trie.Insert(key, resourceName);
            
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

        public List<string> Get(string key)
        {
            string wordToSearch = PreProcessWord(key);

            List<string> toReturn = new List<string>();

            HashSet<string> hs = trie.GetData(wordToSearch);

            foreach (string el in hs)
            {
                toReturn.Add(el);
            }

            return toReturn;
        }

        public bool Remove(string key)
        {
            return trie.Remove(key);
        }
    }
}
