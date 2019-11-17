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

        public int CommonPrefixLength(Node node1, Node node2)
        {
            int lengthToReturn = 0;
            for (int i = 0; i < node1.Key.Length && i < node2.Key.Length; i++)
            {
                lengthToReturn++;
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

            for (int i = current.Depth; i < key.Length; i++)
            {
                Node newNode = new Node(current, key[i].ToString(), null, current.Depth + 1);
                current.Children.Add(newNode);
                current = newNode;
                _size += 1;
            }

            current.Data.Add(data);
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
