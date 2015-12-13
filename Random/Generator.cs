using System;
using System.Collections.Generic;
using System.Linq;
using Urb.Utilities;

namespace Urb.Random {
    public class Generator {
        readonly Implementers.MersenneTwister twister;

        #region Constructors

        /// <summary> Unseeded mode will generate a random seed based on the OS randomizer, which sorta 
        /// defeats the points</summary>
        public Generator() {
            var systemRand = new System.Random();
            var arr = new int[10];
            for (var i = 0; i < 10; i++) arr[i] = systemRand.Next();
            twister = new Implementers.MersenneTwister(intsToUlongs(arr));
        }
        
        public Generator(params int[] seed) {
            twister = new Implementers.MersenneTwister(intsToUlongs(seed));
        } 
        #endregion

        #region Private guts
        /// <summary> Digest the seed list to something more palatable to the rng </summary>        
        private ulong[] intsToUlongs(int[] ints) {
            #region
            var longs = new List<ulong>();
            for (var i = 0; i < ints.Length; i += 2) {
                //var k = unchecked((ulong)(long)ints[i]) << 16 | unchecked((ulong)(long)ints[i + 1]);
                longs.Add(unchecked((ulong)(long)ints[i]));
            }
            //if (ints.Length % 2 == 1) longs.Add(unchecked((ulong)(long)ints.Last()));
            return longs.ToArray();
            #endregion
        }

        #endregion
        
        /// <returns> Next uniform integer in the rng sequence, INCLUSIVE with min and max</returns>
        public int Next(int min, int max) {
            return min + twister.genrand_N(max - min + 1);
        }

        public float Next(float min, float max) {            
            return min + (max - min) * (float)twister.RandomDouble();
        }

        public float Next() {
            return (float)twister.RandomDouble();
        }

        public int[] RandomBuffer(int count) {
            var buffer = new int[count];
            for (var i = 0; i < count; i++) {
                buffer[i] = Next(int.MinValue / 2, int.MaxValue / 2);
            }
            return buffer;
        }

        public byte[] RandomBytes(int count) {
            var buffer = new byte[count];
            for (var i = 0; i < count; i++) {
                buffer[i] = (byte)Next(0, 255);
            }
            return buffer;
        }
        
        public float NextGaussian(float mean, float sigma) {
            return mean + BoxMuller() * sigma;
        }
        
        private float RatioOfUniforms() {
            while(true) {
                var u1 = Next(0f, 1f);
                var v2 = Next(0f, 1f);
                var u2 = (2 * v2 - 1f) * System.Math.Sqrt(2 / System.Math.E );
                if (u1 * u1 <=  System.Math.Exp( u2 * u2 / (u1 * u1 * -2 ))) {
                    return (float)(u2 / u1);
                }
            }
        }

        /// <summary> Since Box-Muller generates 2 independent uniform variables during execution, we can save 1 for the next call.</summary>
        private float? boxMullerSpare;
        private float BoxMuller() {

            if (boxMullerSpare != null) {
                var val = boxMullerSpare.Value;
                boxMullerSpare = null;
                return val;
            }            
            float x, y, z;
            do {
                x = 2f * ( Next() - 1f );
                y = 2f * ( Next() - 1f );
                z = x * x + y * y;
            } while (z > 1f || z == 0f);
            
            var fac = Numbers.Root(-2f * System.Math.Log(z) / z);
            boxMullerSpare = x * fac;
            return y * fac;
        }

    }
}
