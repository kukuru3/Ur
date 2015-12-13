﻿using System;
using System.Collections.Generic;

namespace Urb.Graph {
    /// <summary> A common bidirectional graph with reasonably optimized management of nodes and edges </summary>
    public class Graph {

        #region Fields
        protected SortedSet<Edge> edges;
        protected SortedSet<Node> nodes;
        #endregion

        #region Ctor
        public Graph() {
            edges = new SortedSet<Edge>();
            nodes = new SortedSet<Node>();
        }

        #endregion

        #region Clearing
        public virtual void Clear() {
            foreach (var node in nodes) node.Clear();
            foreach (var edge in edges) edge.Clear();
            edges.Clear();
            nodes.Clear();
        } 
        #endregion

        #region Connection management
        public virtual Edge Connect(Node a, Node b) {
            var edge = FindConnection(a, b);
            if (edge != null) throw new GraphEdgeException(edge, "edge already exists!");
            edge = new Edge(a, b);
            a.RegisterEdge(edge);
            b.RegisterEdge(edge);
            edges.Add(edge);
            return edge;
        }

        public void BreakConnection(Node a, Node b) {
            var edge = FindConnection(a, b);
            if (edge == null) throw new GraphEdgeException(null, "nonexistent edge cannot be broken.");
            a.UnregisterEdge(edge);
            b.UnregisterEdge(edge);
            edges.Remove(edge);
        }

        public Edge FindConnection(Node a, Node b) {
            return a.GetEdgeTo(b);
        }
        #endregion

        #region Node and edge access
        public IEnumerable<Node> AllNodes {
            get {
                return nodes;
            }
        }

        public IEnumerable<Edge> AllEdges {
            get {
                return edges;
            }
        }

        public IEnumerable<Node> Nodes(Predicate<Node> predicate) {
            foreach (var node in nodes) if (predicate(node)) yield return node;
        }

        public IEnumerable<Edge> Edges(Predicate<Edge> predicate) {
            foreach (var edge in edges) if (predicate(edge)) yield return edge;
        }

        #endregion

    }
}