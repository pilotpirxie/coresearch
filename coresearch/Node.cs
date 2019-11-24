using System.Collections;
using System.Collections.Generic;

namespace coresearch
{
    public class Node : ICollection<string>
    {
        private Node _parent;
        private char _key;
        private HashSet<string> _data = new HashSet<string>();
        private List<Node> _children = new List<Node>();
        private int _depth = 0;

        internal Node Parent { get => _parent; set => _parent = value; }
        public char Key { get => _key; set => _key = value; }
        public List<Node> Children { get => _children; set => _children = value; }
        public int Depth { get => _depth; set => _depth = value; }

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public Node(Node parent, char key, string data = null, int depth = 0)
        {
            Parent = parent;
            Key = key;
            if (data != null)
            {
                _data.Add(data);
            }
            Depth = depth;
        }

        public Node GetChildByKey(char key)
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
            return _children.Count == 0 && _parent != null;
        }

        public bool ContainsData()
        {
            return _data != null;
        }

        public bool ContainsData(string dataToCompare)
        {
            return _data.Contains(dataToCompare);
        }

        public void Add(string item)
        {
            _data.Add(item);
        }

        public void Clear()
        {
            _data.Clear();
        }

        public string[] GetData()
        {
            string[] dataToReturn = new string[_data.Count];
            _data.CopyTo(dataToReturn);
            return dataToReturn;
        }

        public bool Contains(string item)
        {
            return _data.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return _data.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }
    }
}
