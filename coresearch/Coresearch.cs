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
        private bool _normalize = false;
        private Regex _rgx;
        private int _memoryLimit = 0;

        public Coresearch(bool debug = false, bool normalize = true, string normalizePattern = "[^a-zA-Z0-9 -]", int memoryLimit = 0)
        {
            _debug = debug;
            _normalize = normalize;
            _rgx = new Regex(normalizePattern);
            _memoryLimit = memoryLimit;
        }

        public bool Debug { get => _debug; set => _debug = value; }

        public int Count { get => _count; }

        private bool Insert(string key, string resourceName)
        {
            if (_memoryLimit == 0 || GC.GetTotalMemory(false) < _memoryLimit)
            {
                string wordToInsert = PreProcessWord(key);

                if (_debug && Count % 50000 == 0)
                {
                    Console.WriteLine($"Batch {Count} with total {trie.Size} nodes with memory size of {GC.GetTotalMemory(false)} bytes ");
                }

                _count++;
                trie.Insert(key, resourceName);

                return true;
            }
            else
            {
                return false;
            }
        }

        public void InsertResource(string resourceName, string content)
        {
            string[] contentWords = content.Split(' ');

            foreach (string word in contentWords)
            {
                Insert(word, resourceName);
            }
        }

        public string Echo(string text)
        {
            return text;
        }

        public string PreProcessWord(string word)
        {
            string wordToReturn;
            if (_normalize)
            {
                wordToReturn = _rgx.Replace(word, "");
                wordToReturn = wordToReturn.Trim();
                wordToReturn = wordToReturn.ToLower();
            }
            else
            {
                wordToReturn = word;
            }

            return wordToReturn;
        }

        public List<string> Get(string key)
        {
            string wordToSearch = PreProcessWord(key);

            List<string> toReturn = new List<string>();

            HashSet<string> data = trie.GetData(wordToSearch);

            foreach (string element in data)
            {
                toReturn.Add(element);
            }

            return toReturn;
        }

        public bool Remove(string key)
        {
            return trie.Remove(key);
        }

        public void Flush()
        {
            trie.Flush();
        }
    }
}
