using System;
using System.Collections.Generic;
using System.Text;

namespace coresearch
{
    class Node
    {
        private Node parent;
        private char key;
        private string data;
        private Dictionary<char, Node> children = new Dictionary<char, Node>();
        private int depth = 0;

        internal Node Parent { get => parent; set => parent = value; }
        public char Key { get => key; set => key = value; }
        public string Data { get => data; set => data = value; }
        public Dictionary<char, Node> Children { get => children; set => children = value; }
        public int Depth { get => depth; set => depth = value; }

        public Node(char key, string Data = null, Node parent = null, int depth = 0)
        {
            Parent = parent;
            Key = key;
            Data = data;
            Depth = depth;
        }

        public Node GetChildByKey(char key)
        {
            if (children.ContainsKey(key))
            {
                return children[key];
            }

            return null;
        }

        public void DeleteChildByKey(char key)
        {
            if (children.ContainsKey(key))
            {
                children.Remove(key);
            }
        }

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public bool ContainsData()
        {
            return data != null;
        }

        public bool ContainsData(string dataToCompare)
        {
            return data == dataToCompare;
        }
    }

    class Trie
    {

    }
}
