using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace coresearch
{
    public class Coresearch
    {
        private Trie words = new Trie();
        private int _count = 0;
        private Regex rgx = new Regex("[^a-zA-Z0-9 -]");

        public int Count {
            get => _count;
        }

        private void AddKey(string word, string resourceName)
        {
            word = rgx.Replace(word, "");
            _count++;

            word = word.Trim();

            if (Count % 5000 == 0)
            {
                Console.WriteLine($"Batch {Count}");
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

        public List<string> Get(string word)
        {
            word = rgx.Replace(word, "");
            List<string> toReturn = new List<string>();

            if (words.ContainsKey(word))
            {
                HashSet<string> hs = words.GetData(word);
                foreach (string el in hs)
                {
                    toReturn.Add(el);
                }
            }

            return toReturn;
        }
    }
}
