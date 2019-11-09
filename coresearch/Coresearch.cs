using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace coresearch
{
    class Coresearch
    {
        private SortedDictionary<string, HashSet<string>> words = new SortedDictionary<string, HashSet<string>>();
        private int _count = 0;

        public int Count {
            get => _count;
        }

        public void Insert(string name, string content, string meta)
        {
            string[] contentWords = content.Split(' ');

            foreach (string word in contentWords)
            {
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");
                string _word = rgx.Replace(word, "");
                _count++;

                if (Count % 50000 == 0)
                {
                    Console.WriteLine($"Batch {Count}");
                }

                if (words.ContainsKey(_word))
                {
                    words[_word].Add(name);
                }
                else
                {
                    words.Add(_word, new HashSet<string>() { name });
                }
            }
        }

        public List<string> Get(string word)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            string _word = rgx.Replace(word, "");
            List<string> toReturn = new List<string>();

            if (words.ContainsKey(_word))
            {
                HashSet<string> hs = words[_word];
                foreach (string el in hs)
                {
                    toReturn.Add(el);
                }
            }

            return toReturn;
        }
    }
}
