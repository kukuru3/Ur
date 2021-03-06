﻿using System.Collections.Generic;

namespace Ur {
    /// <summary> Contains extension methods </summary>
    public static class Arrays2D {

        /// <summary> Iterates through items of a 2-dimensional array</summary>
        ///<returns> An iterator object </returns>
        static public IEnumerable<Iterator2D<T>> Iterate<T>(this T[,] array) {
            var i = new Iterator2D<T>(array);
            var w = i.W; var h = i.H;
            for (var x = 0; x < w; x++)
                for (var y = 0; y < h; y++) {
                    i.X = x; i.Y = y;
                    yield return i;
                }
        }

        static public IEnumerable<Grid.Coords> Iterate(int w, int h) {
            for (var y = 0; y < w; y++)
                for (var x = 0; x < w; x++) {
                    yield return new Grid.Coords(x, y);
                }
        }

        static public IEnumerable<Iterator2D> IterateB(int w, int h) {
            var i = new Iterator2D(w, h);
            for (var y = 0; y < w; y++)
                for (var x = 0; x < w; x++) {
                    i.X = x; i.Y = y;
                    yield return i;
                }
        }

        public class Iterator2D {
            public int W { get; }
            public int H { get; }
            public int X { get; internal set; }
            public int Y { get; internal set; }
            public Iterator2D(int w, int h) {
                W = w;
                H = h;
            }

        }

        /// <summary> Encapsulates array values.</summary>        
        public class Iterator2D<T> {
            private readonly T[,] arrayRef;
            /// <summary> The total width of the array.</summary>
            public int W { get; }
            /// <summary> The total height (2nd dim) of the array.</summary>
            public int H { get; }

            /// <summary> Current item X position in an array</summary>
            public int X { get; internal set; }

            /// <summary> Current item Y position in an array</summary>
            public int Y { get; internal set; }

            /// <summary> Value of the current item. It can be set too even if it is an array of value types!</summary>
            public T Value {
                get { return arrayRef[X, Y]; }
                set { arrayRef[X, Y] = value; }
            }

            internal Iterator2D(T[,] array) {
                arrayRef = array;
                W = array.GetLength(0);
                H = array.GetLength(1);
            }
        }


    }





}
