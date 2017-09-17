using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur.Collections {

    public enum FlattenMethods {
        DepthFirst,
        BreadthFirst,
    }


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
        
        private TreeNode GetNodeOf(T item) => itemLookup[item];

        #region Getting the children and the parent
        public IEnumerable<T> ChildrenOf(T item) {
            foreach (var child in GetNodeOf(item).children) yield return child.MyItem;
        }

        public T ParentOf(T item) => GetNodeOf(item)?.parent?.MyItem;
        #endregion

        public bool AlreadyInTree(T item) => itemLookup.ContainsKey(item);

        public void Remove(T item) {
            var node = GetNodeOf(item);
            Detach(node);
            itemLookup.Remove(item);
            ShakeTree();
        }

        /// <summary> Remove all nodes whose parent node is no longer in the set.
        /// Then, if any nodes were removed thus, repeat the process.</summary>
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

        
        public IEnumerable<T> FlattenHierarchy(FlattenMethods method = FlattenMethods.BreadthFirst) {
            return _FlattenHierarchy(rootNode).Select(n => n.MyItem);
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
            return node;
        }


        private void DestroyNode(TreeNode node) {
            EnsureInTree(node.MyItem);
            itemLookup.Remove(node.MyItem);
            nodeSet.Remove(node);
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
            node.parent.children.Remove(node);
            node.parent = null;
        }

        private void EnsureNotInTree(T forItem) {
            if (AlreadyInTree(forItem)) throw new InvalidOperationException("Node already exists...");
        }
        private void EnsureInTree(T forItem) {
            if (!AlreadyInTree(forItem)) throw new InvalidOperationException("Node does not exist, but should.");
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
