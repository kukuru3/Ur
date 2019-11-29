//#define DOTNET_35
using System;
using System.Collections.Generic;

namespace Ur.Graph {
    /// <summary> A common bidirectional graph with reasonably optimized management of nodes and edges </summary>
    public class Graph {

        #region Fields
#if DOTNET_35
            
#else
        protected SortedSet<Edge> edges;
        protected SortedSet<Node> nodes;
#endif
        #endregion

        #region Ctor
        public Graph() {
#if DOTNET_35
            throw new NotImplementedException();
#else
            edges = new SortedSet<Edge>();
            nodes = new SortedSet<Node>();
#endif
        }

        #endregion

        #region Clearing
        public virtual void Clear() {
#if DOTNET_35
            throw new NotImplementedException();
#else
            foreach (var node in nodes) node.Clear();
            foreach (var edge in edges) edge.Clear();
            edges.Clear();
            nodes.Clear();
#endif

        }
        #endregion

        #region Connection management
        public virtual Edge Connect(Node a, Node b) {
            if (a == b) throw new GraphStateException(this, "You want to connect item with itself " + a.Index);
            var edge = new Edge(a, b);
            TryRegisterEdge(edge);
            return edge;
        }

        protected void TryRegisterEdge(Edge e) {
            var existing = FindConnection(e.A, e.B);
            if (existing != null) throw new GraphEdgeException(existing, "edge already exists!");
            e.A.RegisterEdge(e);
            e.B.RegisterEdge(e);
#if DOTNET_35
                throw new NotImplementedException();
#else
            edges.Add(e);
#endif
        }

        public void BreakConnection(Node a, Node b) {
            var edge = FindConnection(a, b);
            if (edge == null) throw new GraphEdgeException(null, "nonexistent edge cannot be broken.");
            a.UnregisterEdge(edge);
            b.UnregisterEdge(edge);
#if DOTNET_35
#else
            edges.Remove(edge);
#endif
        }

        public Edge FindConnection(Node a, Node b) {
            return a.GetEdgeTo(b);
        }
        #endregion

        #region Node and edge access

        public void AddNode(Node node) {
            nodes.Add(node);
        }

        public IEnumerable<Node> AllNodes {
            get {
#if DOTNET_35
                    throw new NotImplementedException();
#else
                return nodes;
#endif
            }
        }

        public IEnumerable<Edge> AllEdges {
            get {
#if DOTNET_35
                    throw new NotImplementedException();
#else
                return edges;
#endif
            }
        }

        public IEnumerable<Node> Nodes(Predicate<Node> predicate) {
#if DOTNET_35
            throw new NotImplementedException();
#else
            foreach (var node in nodes) if (predicate(node)) yield return node;
#endif
        }

        public IEnumerable<Edge> Edges(Predicate<Edge> predicate) {
#if DOTNET_35
                throw new NotImplementedException();
#else
            foreach (var edge in edges) if (predicate(edge)) yield return edge;
#endif
        }

        #endregion

    }
}
