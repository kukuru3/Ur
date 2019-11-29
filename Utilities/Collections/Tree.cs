using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur.Collections {

    public enum FlattenMethods {
        DepthFirst,
        BreadthFirst,
    }

    /// <summary> Is it efficient? Not really. Is it ubiquitous? Nah.
    /// Does it even make sense to have a generic all-purpose tree class? Of course not.
    /// Will it do the job needed for now? Hell yeah.</summary>
    /// <typeparam name="T"></typeparam>
    public class Tree<T> where T : class {

        #region Fields
        private Dictionary<T, TreeNode> itemLookup;
        private HashSet<TreeNode> nodeSet;
        private TreeNode rootNode;
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
        #endregion

        private TreeNode GetNodeOf(T item) => itemLookup[item];

        #region Getting the children and the parent
        public IEnumerable<T> ChildrenOf(T item) {
            foreach (var child in GetNodeOf(item).children) yield return child.MyItem;
        }

        public T ParentOf(T item) => GetNodeOf(item)?.parent?.MyItem;
        #endregion

        public bool IsMemberOfTree(T item) => item != null && itemLookup.ContainsKey(item);

        /// <summary> Remove all nodes whose parent node is no longer in the set.</summary>
        public void ShakeTree() {
            bool repeat = true;
            while(repeat) {
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

        public void Insert(T item, T asChildOf ) {
            if (asChildOf == null) {
                CreateRootNode(item);
            } else {
                var p = GetNodeOf(asChildOf);
                var c = CreateNode(item);
                Attach(p, c);
            }
        }
   
        public void SortAllChildrenInTree(Comparison<T> comparison) {

            Comparison<TreeNode> childComparison = (TreeNode a, TreeNode b) => comparison(a.MyItem, b.MyItem);
                                                                                                 
            foreach (var node in _FlattenHierarchy(rootNode)) {
                node.children.Sort(childComparison);
            }
        }

        
        public IEnumerable<T> FlattenTreeHierarchy(FlattenMethods method = FlattenMethods.BreadthFirst) {
            return _FlattenHierarchy(rootNode).Select(n => n.MyItem);
        }

        public IEnumerable<T> FlattenHierarchyFromNode(T item,  FlattenMethods method = FlattenMethods.BreadthFirst) {
            return _FlattenHierarchy(GetNodeOf(item), method).Select(n => n.MyItem);
        }

        private IEnumerable<TreeNode> _FlattenHierarchy(TreeNode startingFrom, FlattenMethods method = FlattenMethods.BreadthFirst) {
            if (startingFrom == null) yield break;

            if (method == FlattenMethods.BreadthFirst) {
                var q = new Queue<TreeNode>();
                q.Enqueue(startingFrom);
                while (q.Count > 0) {
                    var node = q.Dequeue();
                    foreach (var child in node.children) q.Enqueue(child);
                    yield return node;
                }
            } else if (method == FlattenMethods.DepthFirst) {
                throw new NotImplementedException("Coming soon");
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
            if (!IsMemberOfTree(item)) return;
            var node = GetNodeOf(item);
            DestroyNode(node);
            ShakeTree();
        }

        private void DestroyNode(TreeNode node) {
            Detach(node);
            itemLookup.Remove(node.MyItem);
            nodeSet.Remove(node);
            ItemRemoved?.Invoke(node.MyItem);
            foreach (var child in node.children.ToArray()) 
                DestroyNode(child);
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
            if (IsMemberOfTree(forItem)) throw new InvalidOperationException("Node already exists...");
        }
        private void EnsureInTree(T forItem) {
            if (!IsMemberOfTree(forItem)) throw new InvalidOperationException("Node does not exist, but should.");
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
