﻿using System;
using System.Collections.Generic;
using System.Linq;
namespace Ur.Grid {
    /// <summary> Tiles are added and removed haphazardly in this class, and there are no bounds limits (0 is not lowest)</summary>
    /// <typeparam name="T">Type of grid tile</typeparam>
    public class SparseGrid<T> : IGrid<T> where T : IHasPosition {
      
        #region Field
        private Dictionary<Coords, T> backingCollection;
        #endregion

        public SparseGrid() {
            backingCollection = new Dictionary<Coords, T>();
        }

        public virtual void AddItems(IEnumerable<T> items) {
            foreach (var item in items) backingCollection[item.Position] = item;
            invalidateBoundingBox = true;
        }

        public virtual void RemoveItems(IEnumerable<T> items) {
            foreach (var item in items) backingCollection.Remove(item.Position);
            invalidateBoundingBox = true;
        }

        //public void AddItem(T item, int atX, int atY) {            
        //    var key = new Coords(atX, atY);
        //    backingCollection[key] = item;
        //    if (!invalidateBoundingBox && !BoundingBox.Contains(key)) invalidateBoundingBox = true;            
        //}

        //public void RemoveItem(int atX, int atY) {
        //    var key = new Coords(atX, atY);
        //    backingCollection.Remove(key);
        //    invalidateBoundingBox = true;

        //}

        public void Clear() {
            backingCollection.Clear();
            invalidateBoundingBox = true;
        }

        #region Value access - publicly exposed
       
        public T this[int x, int y] => GetTile(x, y);
        public T this[Coords c]     => GetTile(c);
        public IEnumerable<T> GetAllTiles() => backingCollection.Values;
        
        public T GetTile(int x, int y) {
            backingCollection.TryGetValue(new Coords(x, y), out T val);
            return val;
        }
        
        public T GetTile(Coords crds) {
            backingCollection.TryGetValue(crds, out T val);
            return val;
        }

        public bool HasTile(int x, int y) {
            return backingCollection.ContainsKey(new Coords(x, y));
        } 

       
        #endregion

        #region Bounds getters - publicly exposed
        
        public Rect BoundingBox { get {
            if (invalidateBoundingBox) RecalculateBoundingBox();
            return boundingBox;
        } }

        
        int IGrid.W => BoundingBox.Width;
        int IGrid.H => BoundingBox.Height;
        
        #endregion

        protected int W => BoundingBox.Width;
        protected int H => BoundingBox.Height;

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
