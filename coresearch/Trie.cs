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
            for(int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Key == key)
                {
                    return _children[i];
                }
            }

            return null;
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

        private static int CommonKeyLength(string key1, string key2)
        {
            int lengthToReturn = 0;
            for (int i = 0; i < key1.Length && i < key2.Length; i++)
            {
                if (key1[i] == key2[i])
                {
                    lengthToReturn++;
                }
                else
                {
                    break;
                }
            }
            return lengthToReturn;
        }

        public Node Prefix(string keyPrefix, Node startNode)
        {
            Node currentNode = startNode;
            Node nodeToReturn = currentNode;

            foreach (char keyPrefixChar in keyPrefix)
            {
                currentNode = currentNode.GetChildByKey(keyPrefixChar.ToString());
                if (currentNode == null)
                {
                    break;
                }
                nodeToReturn = currentNode;
            }

            return nodeToReturn;
        }

        public void Insert(string key, string data)
        {
            Node commonPrefix = Prefix(key, _root);
            Node current = commonPrefix;
            int commonKeyLength = CommonKeyLength(key, commonPrefix.Key);

            if (commonKeyLength > 0)
            {
                Node nodeWithCommonKeyPrefix = new Node(current.Parent, key.Substring(0, commonKeyLength - 1), null, current.Depth + 1);
                _size++;

                nodeWithCommonKeyPrefix.Children.Add(current);
                current.Parent.Children.Add(nodeWithCommonKeyPrefix);
                current.Key = current.Key.Substring(commonKeyLength);
                current.Parent.Children.Remove(current);
                current.Parent = nodeWithCommonKeyPrefix;
            }

            if (key.Substring(commonKeyLength).Length > 0)
            {
                if (current.Parent != null)
                {
                    Node nodeWithUncommonKeyPrefix = new Node(current.Parent, key.Substring(commonKeyLength), data, current.Depth + 1);
                    current.Parent.Children.Add(nodeWithUncommonKeyPrefix);
                }
                else
                {
                    Node nodeWithUncommonKeyPrefix = new Node(current, key.Substring(commonKeyLength), data, current.Depth + 1);
                    current.Children.Add(nodeWithUncommonKeyPrefix);
                }
                _size++;
            }

            /* 
            for (int i = current.Depth; i < key.Length; i++)
            {
                Node newNode = new Node(current, key[i].ToString(), null, current.Depth + 1);
                current.Children.Add(newNode);
                current = newNode;
                _size += 1;
            }
            */
        }

        public bool ContainsKey(string key)
        {
            Node prefix = Prefix(key, _root);
            return prefix.Depth == key.Length && prefix.ContainsData();
        }

        public HashSet<string> GetData(string key)
        {
            if (ContainsKey(key))
            {
                Node prefix = Prefix(key, _root);
                return prefix.Data;
            } 

            return new HashSet<string> () { };
        }

        public void Remove(string key)
        {
            if (ContainsKey(key))
            {
                Node prefix = Prefix(key, _root);

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
