namespace BTreeNs
{



    public interface IBTreeValue
    {
        int CompareTo(IBTreeValue? other);
        string ToString();
        void Save(BinaryWriter bw);
        IBTreeValue Load(BinaryReader br);
    }

    public class BTreeNode<T> where T : IBTreeValue
    {
        public BTreeNode<T> parent = null;
        public List<T> keys = new List<T>();
        public List<BTreeNode<T>> children = new List<BTreeNode<T>>();

        public BTreeNode() { }

        public BTreeNode(List<T> keys)
        {
            this.keys = keys;
        }

        public bool IsLeaf()
        {
            return children.Count == 0;
        }

        public bool IsInternal()
        {
            return !IsLeaf();
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(keys.Count);
            foreach (T t in keys)
                t.Save(bw);
            bw.Write(children.Count);
            foreach (BTreeNode<T> n in children)
                n.Save(bw);
        }

        public BTreeNode<T> Load(BinaryReader br, T dummy)
        {
            BTreeNode<T> n = new BTreeNode<T>();
            int keyCount = br.ReadInt32();
            for (int i = 0; i < keyCount; i++)
                n.keys.Add((T)dummy.Load(br));
            int childrenCount = br.ReadInt32();
            for (int i = 0; i < childrenCount; i++)
            {
                BTreeNode<T> child = Load(br, dummy);
                child.parent = this;
                n.children.Add(child);
            }

            return n;
        }

        public override string ToString()
        {
            string s = "[";
            foreach (T key in keys)
            {
                if (s != "[")
                {
                    s += ", ";
                }
                s += key.ToString();
            }
            s += "]";
            return s;
        }
    }

    public class BTree<T> where T : IBTreeValue
    {
        public int t;
        public BTreeNode<T> root = null;

        public BTree()
        {

        }

        public BTree(int t)
        {
            this.t = t;
        }

        public void Insert(T key)
        {
            if (root == null)
            {
                BTreeNode<T> node = new BTreeNode<T>();
                root = node;
            }

            FindLeafToInsert(root, key);
        }

        public void Delete(T key)
        {
            while (DeleteInternal(root, key))
                ;
        }

        private bool DeleteInternal(BTreeNode<T> node, T key)
        {
            if (node == null)
                return false;

            int biggerKeyIndex = 0;
            for (; biggerKeyIndex < node.keys.Count; biggerKeyIndex++)
            {
                if (key.CompareTo(node.keys[biggerKeyIndex]) < 0)
                    break;
            }

            int keyIndex = biggerKeyIndex - 1;
            if (keyIndex != -1 && node.keys[keyIndex].CompareTo(key) == 0)
            {
                // key found in this node
                if (node.IsLeaf())
                {
                    node.keys.RemoveAt(keyIndex);
                    if (node.keys.Count == 0 && node == root)
                        root = null;
                    return true;
                }
                else
                {
                    BTreeNode<T> leftChildNode = node.children[keyIndex];
                    BTreeNode<T> rightChildNode = node.children[keyIndex + 1];

                    if (leftChildNode.keys.Count > t - 1)
                    {
                        RotateClockwise(node, keyIndex);
                        return DeleteInternal(rightChildNode, key);
                    }
                    else if (rightChildNode.keys.Count > t - 1)
                    {
                        RotateCounterClockwise(node, keyIndex);
                        return DeleteInternal(leftChildNode, key);
                    }
                    else
                    {
                        BTreeNode<T> mergedNode = MergeNode(node, keyIndex);
                        return DeleteInternal(mergedNode, key);
                    }
                }
            }
            else
            {
                if (node.IsLeaf())
                    return false;

                // key not found in this node, move downward and assure keys count > t - 1
                int childIndex = biggerKeyIndex;
                BTreeNode<T> childNode = node.children[childIndex];

                if (childNode.keys.Count > t - 1)
                    return DeleteInternal(childNode, key);

                if (childIndex == 0)
                {
                    if (node.children[1].keys.Count > t - 1)
                    {
                        RotateCounterClockwise(node, 0);
                        return DeleteInternal(childNode, key);
                    }
                    else
                    {
                        BTreeNode<T> mergedNode = MergeNode(node, 0);
                        return DeleteInternal(mergedNode, key);
                    }
                }
                else
                {
                    if (node.children[childIndex - 1].keys.Count > t - 1)
                    {
                        RotateClockwise(node, childIndex - 1);
                        return DeleteInternal(childNode, key);
                    }
                    else
                    {
                        BTreeNode<T> mergedNode = MergeNode(node, childIndex - 1);
                        return DeleteInternal(mergedNode, key);
                    }
                }
            }
        }

        private void RotateCounterClockwise(BTreeNode<T> parentNode, int keyIndex)
        {
            BTreeNode<T> leftChildNode = parentNode.children[keyIndex];
            BTreeNode<T> rightChildNode = parentNode.children[keyIndex + 1];

            leftChildNode.keys.Add(parentNode.keys[keyIndex]);
            parentNode.keys[keyIndex] = rightChildNode.keys[0];
            rightChildNode.keys.RemoveAt(0);

            if (!leftChildNode.IsLeaf())
            {
                leftChildNode.children.Add(rightChildNode.children[0]);
                rightChildNode.children.RemoveAt(0);
            }
        }

        private void RotateClockwise(BTreeNode<T> parentNode, int keyIndex)
        {
            BTreeNode<T> leftChildNode = parentNode.children[keyIndex];
            BTreeNode<T> rightChildNode = parentNode.children[keyIndex + 1];

            rightChildNode.keys.Insert(0, parentNode.keys[keyIndex]);
            parentNode.keys[keyIndex] = leftChildNode.keys[leftChildNode.keys.Count - 1];
            leftChildNode.keys.RemoveAt(leftChildNode.keys.Count - 1);

            if (!rightChildNode.IsLeaf())
            {
                rightChildNode.children.Insert(0, leftChildNode.children[leftChildNode.children.Count - 1]);
                leftChildNode.children.RemoveAt(leftChildNode.children.Count - 1);
            }
        }

        private BTreeNode<T> MergeNode(BTreeNode<T> node, int keyIndex)
        {
            BTreeNode<T> mergedNode = new BTreeNode<T>();
            mergedNode.keys.AddRange(node.children[keyIndex].keys);
            mergedNode.keys.Add(node.keys[keyIndex]);
            mergedNode.keys.AddRange(node.children[keyIndex + 1].keys);
            mergedNode.children.AddRange(node.children[keyIndex].children);
            mergedNode.children.AddRange(node.children[keyIndex + 1].children);
            mergedNode.parent = node;

            node.keys.RemoveAt(keyIndex);
            node.children.RemoveAt(keyIndex + 1);
            node.children.RemoveAt(keyIndex);

            node.children.Insert(keyIndex, mergedNode);

            if (node == root && node.keys.Count == 0)
                root = mergedNode;

            return mergedNode;
        }

        private void InsertUpward(BTreeNode<T> node, T key, BTreeNode<T> left, BTreeNode<T> right)
        {
            int i = GetKeyInsertPosition(node, key);

            node.keys.Insert(i, key);

            if (node.children.Count != 0)
                node.children.RemoveAt(i);

            node.children.Insert(i, left);
            node.children.Insert(i + 1, right);

            if (node.keys.Count == 2 * t)
            {
                Split(node);
            }
        }

        private int GetKeyInsertPosition(BTreeNode<T> node, T key)
        {
            int i = 0;
            for (; i < node.keys.Count; i++)
            {
                if (key.CompareTo(node.keys[i]) < 0)
                    break;
            }
            return i;
        }

        private void FindLeafToInsert(BTreeNode<T> node, T key)
        {
            int i = GetKeyInsertPosition(node, key);

            if (node.IsLeaf())
            {
                node.keys.Insert(i, key);

                if (node.keys.Count == 2 * t)
                {
                    Split(node);
                }
            }
            else
            {
                FindLeafToInsert(node.children[i], key);
            }
        }

        private void Split(BTreeNode<T> node)
        {
            T key = node.keys[t];

            BTreeNode<T> left = new BTreeNode<T>(node.keys.GetRange(0, t));
            BTreeNode<T> right = new BTreeNode<T>(node.keys.GetRange(t + 1, t - 1));

            if (node.children.Count > 0)
            {
                left.children.AddRange(node.children.GetRange(0, t + 1));
                right.children.AddRange(node.children.GetRange(t + 1, t));

                foreach (BTreeNode<T> child in left.children)
                    child.parent = left;
                foreach (BTreeNode<T> child in right.children)
                    child.parent = right;
            }

            if (node.parent == null)
            {
                root = new BTreeNode<T>();
                node.parent = root;
            }
            left.parent = node.parent;
            right.parent = node.parent;

            InsertUpward(node.parent, key, left, right);
        }

        public int KeyCount()
        {
            if (root == null)
                return 0;

            return KeyCountInternal(root);
        }

        private int KeyCountInternal(BTreeNode<T> node)
        {
            int keyCount = node.keys.Count;
            foreach (BTreeNode<T> n in node.children)
                keyCount += KeyCountInternal(n);

            return keyCount;
        }

        public List<T> Find(T key)
        {
            if (root == null)
                return new List<T>();

            return FindInternal(root, key);
        }

        private List<T> FindInternal(BTreeNode<T> node, T key)
        {
            List<T> found = new List<T>();

            int start = 0;
            for (; start < node.keys.Count; start++)
            {
                if (key.CompareTo(node.keys[start]) <= 0)
                    break;
            }
            int end = start + 1;
            for (; end < node.keys.Count; end++)
            {
                if (key.CompareTo(node.keys[end]) != 0)
                    break;
            }

            if (node.IsInternal())
                found.AddRange(FindInternal(node.children[start], key));

            for (int i = start; i < end && i < node.keys.Count; i++)
            {
                if (key.CompareTo(node.keys[i]) == 0)
                    found.Add(node.keys[i]);

                if (node.IsInternal())
                    found.AddRange(FindInternal(node.children[i + 1], key));
            }

            return found;
        }

        private int getString(int level, BTreeNode<T> currentNode, List<List<string>> nodeStringByLevel)
        {
            if (level == nodeStringByLevel.Count)
                nodeStringByLevel.Add(new List<string>());

            string currentNodeString = currentNode.ToString();
            int childrenWidth = 0;
            foreach (BTreeNode<T> child in currentNode.children)
                childrenWidth += getString(level + 1, child, nodeStringByLevel);

            for (int i = 0; currentNodeString.Length < childrenWidth; i++)
            {
                if (i % 2 == 0)
                    currentNodeString = " " + currentNodeString;
                else
                    currentNodeString = currentNodeString + " ";
            }

            nodeStringByLevel[level].Add(currentNodeString);

            return childrenWidth >= currentNodeString.Length ? childrenWidth : currentNodeString.Length;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(t);
            if (root == null)
                bw.Write(false);
            else
            {
                bw.Write(true);
                root.Save(bw);
            }
        }

        public static BTree<T> Load(BinaryReader br, T dummy)
        {
            BTree<T> tree = new BTree<T>();

            tree.t = br.ReadInt32();
            bool hasRoot = br.ReadBoolean();

            if (hasRoot)
                tree.root = new BTreeNode<T>().Load(br, dummy);

            return tree;
        }

        public override string ToString()
        {
            string ret = "";

            if (root == null)
                return ret;

            List<List<string>> nodeStringByLevel = new List<List<string>>();
            getString(0, root, nodeStringByLevel);

            foreach (List<string> level in nodeStringByLevel)
            {
                foreach (string nodeString in level)
                {
                    ret += nodeString;
                }
                ret += "\n";
            }

            return ret;
        }
    }

}