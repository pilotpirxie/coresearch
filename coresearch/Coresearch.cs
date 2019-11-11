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
            _count++;

            string wordToInsert = rgx.Replace(word, " ");
            wordToInsert = wordToInsert.Trim();
            wordToInsert = wordToInsert.ToLower();

            if (Count % 5000 == 0)
            {
                Console.WriteLine($"Batch {Count} with total {words.Size}");
            }

            words.Insert(wordToInsert, resourceName);
            
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
            string wordToSearch = rgx.Replace(word, "");
            wordToSearch = wordToSearch.Trim();
            wordToSearch = wordToSearch.ToLower();

            List<string> toReturn = new List<string>();

            HashSet<string> hs = words.GetData(word);

            foreach (string el in hs)
            {
                toReturn.Add(el);
            }

            return toReturn;
        }
    }
}
