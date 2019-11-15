using System;
using System.Collections.Generic;

namespace coresearch
{
    public class Node
    {
        private Node _parent;
        private string _key;
        private HashSet<string> _data = new HashSet<string>();
        private List<Node> _children = new List<Node>();
        private int _depth = 0;

        internal Node Parent { get => _parent; set => _parent = value; }
        public string Key { get => _key; set => _key = value; }
        public HashSet<string> Data { get => _data; set => _data = value; }
        public List<Node> Children { get => _children; set => _children = value; }
        public int Depth { get => _depth; set => _depth = value; }

        public Node(Node parent, string key, string data = null, int depth = 0)
        {
            if (parent != null)
            {
                Console.WriteLine($"{parent.Key} {key}");
            }

            Parent = parent;
            Key = key;
            if (data != null)
            {
                Data.Add(data);
            }
            Depth = depth;
        }

        public Node GetChildByKey(string key)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Key == key)
                {
                    return _children[i];
                }
            }

            return null;
        }

        public (Node, int) FindCommonChildByKey(string key)
        {
            Node nodeToReturn = null;
            int commonLengthToReturn = 0;
            for (int i = 0; i < _children.Count; i++)
            {
                for (int j = 0; j < key.Length && j < _children[i].Key.Length; j++)
                {
                    if (_children[i].Key[j] == key[j])
                    {
                        nodeToReturn = _children[i];
                        commonLengthToReturn = j + 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return (nodeToReturn, commonLengthToReturn);
        }

        public void DeleteChildByKey(string key)
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
        private int _size;

        public int Size { get => _size; set => _size = value; }

        public Trie()
        {
            _root = new Node(null, " ", null, 0);
            _size = 0;
        }

        public (Node, int) Prefix(string keyPrefix)
        {
            Node currentNode = _root;
            Node nodeToReturn = currentNode;

            int currentCommonLength = 0;
            int lengthToReturn = currentCommonLength;

            while (currentNode != null)
            {
                (currentNode, currentCommonLength) = currentNode.FindCommonChildByKey(keyPrefix.Substring(currentCommonLength));
                if (currentNode != null)
                {
                    nodeToReturn = currentNode;
                    lengthToReturn += currentCommonLength;
                }
            }

            return (nodeToReturn, lengthToReturn);
        }

        public void Insert(string key, string data)
        {
            (Node current, int commonLength) = Prefix(key);
            Console.WriteLine($"--- {key} {current.Key} {commonLength}");

            if (commonLength > 0)
            {
                if (commonLength != current.Key.Length)
                {
                    Node newNode = new Node(current, key.Substring(0, commonLength), data, current.Depth + 1);
                    newNode.Children.Add(current);

                    current.Parent.Children.Add(newNode);
                    current.Key = key.Substring(commonLength);
                }
                else
                {
                    Node newNode = new Node(current, key.Substring(commonLength), data, current.Depth + 1);
                    current.Children.Add(current);
                }
            }
            else
            {
                Node newNode = new Node(current, key, data, current.Depth + 1);
                current.Children.Add(newNode);
            }

            _size += 1;

        }

        public bool ContainsKey(string key)
        {
            (Node prefix, _) = Prefix(key);
            return prefix.Depth == key.Length && prefix.ContainsData();
        }

        public HashSet<string> GetData(string key)
        {
            if (ContainsKey(key))
            {
                (Node prefix, _) = Prefix(key);
                return prefix.Data;
            } 

            return new HashSet<string> () { };
        }

        public void Remove(string key)
        {
            if (ContainsKey(key))
            {
                (Node prefix, _) = Prefix(key);

                while (prefix.IsLeaf())
                {
                    Node parent = prefix.Parent;
                    parent.DeleteChildByKey(prefix.Key);
                    prefix = parent;
                }
            }
        }

        public void BatchInsert(List<KeyValuePair<string, string>> items)
        {
            foreach(KeyValuePair<string, string> item in items)
            {
                Insert(item.Key, item.Value);
            }
        }
    }
}
