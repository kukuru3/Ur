using System;
using System.Collections.Generic;
using System.Linq;

namespace Ur.Graph {
    public class Node : IComparable<Node> {

        #region Fields
        private Dictionary<Edge, Node> edges;
        private Dictionary<Node, Edge> neighbours; 
        #endregion

        #region Properties
        public int Index { get; }
        public Graph Graph { get; } 
        #endregion

        public Node(Graph graph) {
            Graph = graph;
            Index = ++instanceCounter;
            edges = new Dictionary<Edge, Node>();
            neighbours = new Dictionary<Node, Edge>();            
        }

        #region Management options, called by the Graph object.
        /// <summary> Graph is the only object that should call this. </summary>        
        internal void Clear() {
            edges.Clear();
            neighbours.Clear();
        }

        /// <summary> Graph is the only object that should call this. </summary>        
        internal void RegisterEdge(Edge e) {
            if (edges.ContainsKey(e)) throw new GraphEdgeException(e, "Edge cannot be reregistered to node " + this.Index);
            var node = e.Other(this);
            edges[e] = node;
            neighbours[node] = e;
            AdjacencyChanged?.Invoke(this);
        }

        /// <summary> Graph is the only object that should call this. </summary>        
        internal void UnregisterEdge(Edge e) {
            if (!edges.ContainsKey(e)) throw new GraphEdgeException(e, "Cannot unregister edge we don't own");
            var neighbour = edges[e];
            edges.Remove(e);
            neighbours.Remove(neighbour);
            AdjacencyChanged?.Invoke(this);
        } 
        #endregion


        /// <summary> Invoked when ever adjacency properties of this node change, whether an edge was added or removed. </summary>
        public event Action<Node> AdjacencyChanged;

        #region Adjacency - edge and neighbour tests, access.
        public bool Connects(Node other) {
            return neighbours.ContainsKey(other);
        }

        public Node GetNeighbour(Edge via) {
            Node result; edges.TryGetValue(via, out result); return result;
        }

        public Edge GetEdgeTo(Node other) {
            Edge result; neighbours.TryGetValue(other, out result); return result;
        }

        public IEnumerable<Node> AllNeighbours { get { return neighbours.Keys; } }

        public IEnumerable<Edge> AllEdges      { get { return edges.Keys; } }

        public IEnumerable<Node> Neighbours(Predicate<Node> predicate) {
            return neighbours.Keys.Where(n => predicate(n));
        }

        public IEnumerable<Edge> Edges(Predicate<Edge> predicate) {
            return edges.Keys.Where(e => predicate(e));
        }

        #endregion
        
        #region Satisfaction of IComparable
        public int CompareTo(Node other) {
            return (Index - other.Index).Sign();
        } 
        #endregion

        #region Static instance counter
        static private int instanceCounter;
        #endregion
        
    }
}
