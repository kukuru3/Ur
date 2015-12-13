using System;
using System.Collections.Generic;

namespace Urb.Grid {
    /// <summary> Tiles are added and removed haphazardly in this class, and there are no bounds limits (0 is not lowest)</summary>
    /// <typeparam name="T">Type of grid tile</typeparam>
    public class SparseGrid<T> : IGrid {
      
        #region Field
        private Dictionary<Coords, T> backingCollection;
        #endregion

        public SparseGrid() {
            backingCollection = new Dictionary<Coords, T>();
        }

        public void AddItem(T item, int atX, int atY) {
            var key = new Coords(atX, atY);
            backingCollection[key] = item;
            if (!invalidateBoundingBox && !BoundingBox.Contains(key)) invalidateBoundingBox = true;            
        }

        public void RemoveItem(int atX, int atY) {
            var key = new Coords(atX, atY);
            backingCollection.Remove(key);
            invalidateBoundingBox = true;

        }

        #region Value access - publicly exposed
       
        public bool HasValueAt(int x, int y) {
            return backingCollection.ContainsKey(new Coords(x, y));
        } 

        public IEnumerable<T> GetAll() {
            return backingCollection.Values;
        }

        #endregion

        #region Bounds getters - publicly exposed
        
        public Rect BoundingBox { get {
            if (invalidateBoundingBox) RecalculateBoundingBox();
            return boundingBox;
        } }

        
        int IGrid.W { get { return BoundingBox.Width; } }

        int IGrid.H { get { return BoundingBox.Height; } }
        #endregion

        #region Value access
        public T GetValue(int x, int y) {
            T val;
            backingCollection.TryGetValue(new Coords(x, y), out val);
            return val;
        }

        public T GetValue(Coords crds) {
            T val;
            backingCollection.TryGetValue(crds, out val);
            return val;
        }

        #endregion

        #region Maintain and compute bounding box

        private Rect boundingBox;
        private bool invalidateBoundingBox;
        private void RecalculateBoundingBox() {
            invalidateBoundingBox = false;
            int minX = int.MaxValue; int maxX = int.MinValue;
            int minY = int.MaxValue; int maxY = int.MinValue;

            foreach (var tile in backingCollection.Keys) {
                if (tile.X < minX ) minX = tile.X; if (tile.X > maxX ) maxX = tile.X;
                if (tile.Y < minY ) minY = tile.Y; if (tile.Y > maxY ) maxY = tile.Y;
            }
            boundingBox = Rect.FromBounds(minX, maxX, minY, maxY);
        }

        #endregion

    }

    
}
