using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace coresearch
{
    public class Node
    {
        private Node _parent;
        private char _key;
        private HashSet<string> _data = new HashSet<string>();
        private List<Node> _children = new List<Node>();
        private int _depth = 0;

        internal Node Parent { get => _parent; set => _parent = value; }
        public char Key { get => _key; set => _key = value; }
        public HashSet<string> Data { get => _data; set => _data = value; }
        public List<Node> Children { get => _children; set => _children = value; }
        public int Depth { get => _depth; set => _depth = value; }

        public Node(Node parent, char key, string data = null, int depth = 0)
        {
            Parent = parent;
            Key = key;
            if (data != null)
            {
                Data.Add(data);
            }
            Depth = depth;
        }

        public Node GetChildByKey(char key)
        {
            for(int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Key == key)
                {
                    return _children[i];
                }
            }

            return null;
        }

        public void DeleteChildByKey(char key)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Key == key)
                {
                    _children.RemoveAt(i);
                }
            }
        }

        public bool IsLeaf()
        {
            return _children.Count == 0;
        }

        public bool ContainsData()
        {
            return _data != null;
        }

        public bool ContainsData(string dataToCompare)
        {
            return _data.Contains(dataToCompare);
        }
    }

    public class Trie
    {
        private readonly Node _root;

        public Trie()
        {
            _root = new Node(null, ' ', null, 0);
        }

        public Node Prefix(string keyPrefix)
        {
            Node currentNode = _root;
            Node result = currentNode;

            foreach (char keyPrefixChar in keyPrefix)
            {
                currentNode = currentNode.GetChildByKey(keyPrefixChar);
                if (currentNode == null)
                {
                    break;
                }
                result = currentNode;
            }

            return result;
        }

        public void Insert(string key, string data)
        {
            Node commonPrefix = Prefix(key);
            Node current = commonPrefix;

            for (int i = 0; i < key.Length; i++)
            { 
                Node newNode = new Node(current, key[i], null, current.Depth + 1);
                current.Children.Add(newNode);
                current = newNode;
            }

            current.Data.Add(data);
        }
    }
}
