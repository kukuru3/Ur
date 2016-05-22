using System;
using System.Collections.Generic;

namespace Ur.Graph {
    public class GraphStateException : Exception {
        public Graph Graph { get; }
        public GraphStateException(Graph g, string message) : base(message)  {
            Graph = g;
        }
    }

    public class GraphEdgeException : GraphStateException {
        public Edge Edge { get; }
        public GraphEdgeException(Edge e, string message) : base(e.Graph, message) {
            Edge = e;
        }
    }
}
