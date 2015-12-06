using System;
using System.Collections.Generic;

namespace Urb.Grid {
    /// <summary> Tiles are added and removed haphazardly in this class, and there are no bounds limits (0 is not lowest)</summary>
    /// <typeparam name="T">Type of grid tile</typeparam>
    public abstract class SparseGrid<T> : IGrid {
      
        #region Field
        private Dictionary<Coords, T> backingCollection;
        #endregion

        public SparseGrid() {
            backingCollection = new Dictionary<Coords, T>();
        }

        protected void AddItem(T item, int atX, int atY) {
            var key = new Coords(atX, atY);
            backingCollection[key] = item;
            // todo: check for bounding box
        }

        protected void RemoveItem(int atX, int atY) {
            var key = new Coords(atX, atY);
            backingCollection.Remove(key);
            // todo : check for bounding box update
        }

        #region Value access - publicly exposed
        public T this[int x, int y] { get { return GetValue(x, y); } }

        public bool HasValueAt(int x, int y) {
            return backingCollection.ContainsKey(new Coords(x, y));
        } 
        #endregion
            
        #region Bounds getters        
        public Rect BoundingBox { get; private set; }

        int IGrid.W { get { return BoundingBox.Width; } }

        int IGrid.H { get { return BoundingBox.Height; } }
        #endregion

        #region Value access
        T GetValue(int x, int y) {
            T val;
            backingCollection.TryGetValue(new Coords(x, y), out val);
            return val;
        }

        T GetValue(Coords crds) {
            T val;
            backingCollection.TryGetValue(crds, out val);
            return val;
        }
        
        #endregion

    }

    
}
