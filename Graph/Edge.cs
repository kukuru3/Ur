using System;
using System.Collections.Generic;
using Ur.Utilities;

namespace Ur.Graph {
    public class Edge : IComparable<Edge> {
                
        #region Properties
        public Node A { get; }
        public Node B { get; }
        public int Index { get; }
        public Graph Graph { get { return A.Graph; } }
        #endregion

        #region Public methods
        public Node Other(Node a) {
            if (A == a) return B;
            if (B == a) return A;
            throw new GraphEdgeException(this, "Cannot find 'other' from non-mine node " + a.Index);
        }
        #endregion

        #region Guts
        protected internal Edge(Node a, Node b) {
            if (a.Index > b.Index) { A = b; B = a; } else { A = a; B = b; }
            Index = ++instanceCounter;
        }

        protected internal virtual void Clear() { } 
        #endregion

        #region Satisfaction of IComparable
        public int CompareTo(Edge other) {
            return (Index - other.Index).Sign();
        } 
        #endregion

        #region Declare edge equality to depend on its two nodes
        public override bool Equals(object obj) {
            var otherEdge = obj as Edge;
            if (otherEdge == null) return false;
            return otherEdge.A == A && otherEdge.B == B;
        }

        public override int GetHashCode() {
            unchecked {
                return A.GetHashCode() + B.GetHashCode();
            }
        } 
        #endregion

        #region Static instance counter
        static private int instanceCounter; 
        #endregion

    }
}
