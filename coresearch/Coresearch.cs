using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace coresearch
{
    public class Coresearch : IDictionary<string, string>
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

        public ICollection<string> Keys => throw new NotImplementedException();

        public ICollection<string> Values => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        string IDictionary<string, string>.this[string key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public object this[string key] {
            get => Get(key.ToString());
            set => AddKey(key.ToString(), value.ToString());
        }

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

        public List<string> Get(string word)
        {
            string wordToSearch = PreProcessWord(word);

            List<string> toReturn = new List<string>();

            HashSet<string> hs = trie.GetData(wordToSearch);

            foreach (string el in hs)
            {
                toReturn.Add(el);
            }

            return toReturn;
        }

        public void Add(string key, string value)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out string value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
