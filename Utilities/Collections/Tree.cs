using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur.Collections {

    /// <summary> Is it efficient? Not really. Is it ubiquitous? Nah.
    /// Does it even make sense to have a generic all-purpose tree class? Of course not.
    /// Will it do the job needed for now? Hell yeah.</summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> where T : class {

        #region Fields
        private Dictionary<T, TreeNode> itemLookup;
        private HashSet<TreeNode> nodeSet;
        private TreeNode rootNode;
        bool    treeDirty = true;
        #endregion

        #region C-tor
        public Tree() {
            itemLookup = new Dictionary<T, TreeNode>();
            nodeSet = new HashSet<TreeNode>();
        }
        #endregion

        #region Events
        public event Action<T> ItemRemoved;
        public event Action<T> ItemInserted;
        public event Action<Tree<T>> TreeFlattened;
        #endregion

        private TreeNode GetNodeOf(T item) => itemLookup[item];

        #region Getting the children and the parent
        public IEnumerable<T> ChildrenOf(T item) {
            foreach (var child in GetNodeOf(item).children) yield return child.MyItem;
        }

        public T ParentOf(T item) => GetNodeOf(item)?.parent?.MyItem;
        #endregion

        public bool ContainsItem(T item) => item != null && itemLookup.ContainsKey(item);

        public void Insert(T item, T asChildOf) {
            if (asChildOf == null) {
                CreateRootNode(item);
            } else {
                var p = GetNodeOf(asChildOf);
                var c = CreateNode(item);
                Attach(p, c);
            }
        }

        public T GetRootElement() => rootNode.MyItem;

        public void SortAllChildrenInTree(Comparison<T> comparison) {

            Comparison<TreeNode> childComparison = (TreeNode a, TreeNode b) => comparison(a.MyItem, b.MyItem);

            foreach (var node in _FlattenHierarchy(rootNode)) {
                node.children.Sort(childComparison);
            }
        }

        public List<T> flattenedTree;

        public IEnumerable<T> GetAllMembersInHierarchyOrder() {
            if (treeDirty) { 
                flattenedTree = _FlattenHierarchy(rootNode).Select(n => n.MyItem).ToList();
                treeDirty = false;
                TreeFlattened?.Invoke(this);
            }

            return flattenedTree;
        }

        public IEnumerable<T> FlattenHierarchyFromNode(T item) {
            return _FlattenHierarchy(GetNodeOf(item)).Select(n => n.MyItem);
        }

        /// <summary> Remove all nodes whose parent node is no longer in the set.</summary>
        private void ShakeTree() {
            bool repeat = true;
            while (repeat) {
                var orphanNodes = nodeSet.Where(n => !nodeSet.Contains(n.parent)).ToList();
                orphanNodes.Remove(rootNode);
                repeat = orphanNodes.Count() > 0;
                if (repeat) {
                    nodeSet.ExceptWith(orphanNodes);
                    foreach (var node in orphanNodes) itemLookup.Remove(node.MyItem);
                    repeat = true;
                }
            }
        }

        private IEnumerable<TreeNode> _FlattenHierarchy(TreeNode startingFrom) {
            if (startingFrom == null) yield break;

            
            var q = new Queue<TreeNode>();
            q.Enqueue(startingFrom);
            while (q.Count > 0) {
                var node = q.Dequeue();
                foreach (var child in node.children) q.Enqueue(child);
                yield return node;
            }
        }

        #region Node creation, deletion
        private void CreateRootNode(T forItem) {
            if (rootNode != null) throw new InvalidOperationException("Already has root node");
            EnsureNotInTree(forItem);
            var node = CreateNode(forItem);
            rootNode = node;
        }

        private TreeNode CreateNode(T forItem) {
            EnsureNotInTree(forItem);
            var node = new TreeNode(this, forItem);
            itemLookup.Add(forItem, node);
            nodeSet.Add(node);
            ItemInserted?.Invoke(forItem);
            return node;
        }

        public void Remove(T item) {
            if (!ContainsItem(item)) return;
            var node = GetNodeOf(item);
            DestroyNode(node);
            ShakeTree();
            treeDirty = true;
        }

        private void DestroyNode(TreeNode node) {
            Detach(node);
            itemLookup.Remove(node.MyItem);
            nodeSet.Remove(node);
            ItemRemoved?.Invoke(node.MyItem);
            foreach (var child in node.children.ToArray())
                DestroyNode(child);
            treeDirty = true;
        }
        #endregion

        #region Attachment, detachment, hierarchy
        private void Attach(TreeNode parent, TreeNode child) {
            if (child == parent) throw new InvalidOperationException("cannot parent to oneself");
            if (InParentHierarchy(parent, child)) throw new InvalidOperationException("This attachment process would create a cyclic hierarchy");
            if (child.parent == parent) return;
            if (child.parent != null) Detach(child);
            child.parent = parent;
            parent.children.Add(child);

            ShakeTree();
        }

        private bool InParentHierarchy(TreeNode node, TreeNode potentialParent) {
            for (var n = node; n != null; n = n.parent) {
                if (n == potentialParent) return true;
            }
            return false;
        }

        /// <summary> Remove parent-child relationship</summary>
        private void Detach(TreeNode node) {
            node.parent?.children.Remove(node);
            node.parent = null;
        }

        private void EnsureNotInTree(T forItem) {
            if (ContainsItem(forItem)) throw new InvalidOperationException("Node already exists...");
        }
        private void EnsureInTree(T forItem) {
            if (!ContainsItem(forItem)) throw new InvalidOperationException("Node does not exist, but should.");
        }
        #endregion

        #region Declarations
        private class TreeNode {
            public T MyItem { get; }
            public Tree<T> MyTree { get; }
            public TreeNode(Tree<T> tree, T item) {
                MyItem = item;
                MyTree = tree;
                parent = null;
                children = new List<TreeNode>();
            }

            internal List<TreeNode> children;
            internal TreeNode parent;
        }
        #endregion


    }
}
