using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace coresearch
{
    public class Coresearch
    {
        private Trie _trie = new Trie();
        public Trie Trie {
            get => _trie;
        }

        private int _count = 0;
        private bool _debug = false;
        private readonly bool _normalize = true;
        private readonly Regex _rgx;
        private readonly int _memoryLimit = 0;

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
                    Console.WriteLine($"Batch {Count} with total {_trie.Size} nodes with memory size of {GC.GetTotalMemory(false)} bytes ");
                }

                _count++;
                _trie.Insert(wordToInsert, resourceName);

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

        private string PreProcessWord(string word)
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

            string[] data = _trie.GetDataFromExactNode(wordToSearch);

            foreach (string element in data)
            {
                toReturn.Add(element);
            }

            return toReturn;
        }

        public List<string> QueryDeep(string key)
        {
            string wordToSearch = PreProcessWord(key);

            List<string> toReturn = new List<string>();

            HashSet<string> data = _trie.GetDataFromChildrenNodesRecursive(wordToSearch);

            foreach (string element in data)
            {
                toReturn.Add(element);
            }

            return toReturn;
        }

        public List<string> QueryShallow(string key)
        {
            string wordToSearch = PreProcessWord(key);

            List<string> toReturn = new List<string>();

            HashSet<string> data = _trie.GetDataFromChildrenNodes(wordToSearch);

            foreach (string element in data)
            {
                toReturn.Add(element);
            }

            return toReturn;
        }

        public bool Remove(string key)
        {
            return _trie.Remove(key);
        }

        public void Flush()
        {
            _count = 0;
            _trie.Flush();
        }
    }
}
