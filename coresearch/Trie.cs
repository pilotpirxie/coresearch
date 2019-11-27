using System.Collections.Generic;
using System;

namespace coresearch
{
    public class Trie
    {
        private Node _root;
        private int _size;
        private HashSet<string> _searchData;

        public int Size { get => _size; set => _size = value; }

        public Trie()
        {
            _root = new Node(null, ' ', null, 0);
            _size = 0;
        }

        public Node TraversePrefix(string keyPrefix)
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
            Node commonPrefix = TraversePrefix(key);
            Node current = commonPrefix;

            for (int i = current.Depth; i < key.Length; i++)
            {
                Node newNode = new Node(current, key[i], null, current.Depth + 1);
                current.Children.Add(newNode);
                current = newNode;
                _size += 1;
            }

            current.Add(data);
        }

        public bool ContainsKey(string key)
        {
            Node prefix = TraversePrefix(key);
            return prefix.Depth == key.Length && prefix.ContainsData();
        }

        public string[] GetDataFromExactNode(string key)
        {
            if (ContainsKey(key))
            {
                Node prefix = TraversePrefix(key);
                return prefix.GetData();
            }

            return new string[] { };
        }

        public HashSet<string> GetDataFromChildrenNodes(string key)
        {
            Node prefix = TraversePrefix(key);
            HashSet<string> toReturn = new HashSet<string>();

            for (int i = 0; i < prefix.Children.Count; i++)
            {
                string[] currentChildrenData = prefix.Children[i].GetData();
                for (int j = 0; j < currentChildrenData.Length; j++)
                {
                    toReturn.Add(currentChildrenData[j]);

                }
            }

            return toReturn;
        }

        public HashSet<string> GetDataFromChildrenNodesRecursive(string key)
        {
            _searchData = new HashSet<string>();
            Node prefix = TraversePrefix(key);
            GetDataRecursive(prefix);
            return _searchData;
        }

        public void GetDataRecursive(Node prefix)
        {
            for (int i = 0; i < prefix.Children.Count; i++)
            {
                GetDataRecursive(prefix.Children[i]);
            }

            foreach (string dataChild in prefix.GetData())
            {
                _searchData.Add(dataChild);
            }
        }

        public bool Remove(string key)
        {
            if (ContainsKey(key))
            {
                Node prefix = TraversePrefix(key);

                while (prefix.IsLeaf())
                {
                    Node parent = prefix.Parent;
                    parent.DeleteChildByKey(prefix.Key);
                    _size -= 1;
                    prefix = parent;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Flush()
        {
            _root = new Node(null, ' ', null, 0);
            _size = 0;
            GC.Collect();
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
