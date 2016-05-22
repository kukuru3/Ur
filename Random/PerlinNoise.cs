// courtesy of http://freespace.virgin.net/hugo.elias/models/m_perlin.htm
using System;
using System.Collections.Generic;
using Ur.Utilities;

namespace Ur.Random {
    public class PerlinNoise {

        Generator rng;
        
        float[,] bank;
        const int LatticeDimensions = 128;

        /// <summary> Generates a single instance of perlin noise. Once created the noise function will 
        /// always return the same values when polled at the same locations. </summary>
        /// <param name="persistence">Keep between 0.5 and 0.9 for best results</param>
        /// <param name="octaves">Keep above 3 for best results</param>        
        /// <remarks>Recommended usage : </remarks>
        public PerlinNoise(float persistence, int octaves, Generator explicitGenerator = null) {
            
            this.persistence = persistence;
            this.octaves     = octaves;
            rng = explicitGenerator ?? new Generator();
            
            bank = new float[LatticeDimensions, LatticeDimensions];
            foreach (var i in bank.Iterate()) i.Value = rng.Next(-1f, 1f);
        }

        readonly float persistence;
        readonly int   octaves;


        public float[,] ExtractMap(int w, int h, float scaleValue, float offsetX = 0f, float offsetY = 0f, Func<float,float> conversionFunction = null  ) {
            var map = new float[w,h];
            foreach (var i in map.Iterate()) {
                var val = this[offsetX + scaleValue * i.X / w, offsetY + scaleValue * i.Y / h];
                if (conversionFunction!= null) val = conversionFunction(val);
                map[i.X, i.Y] = val;
            }
            return map;
        }

        float NoiseAt(int x, int y) {
            return bank[x.Wrap(LatticeDimensions), y.Wrap(LatticeDimensions)];
        }
        
        float SmoothNoise(int x, int y) {
            var corners = NoiseAt(x-1, y-1) + NoiseAt(x+1, y-1) + NoiseAt(x-1, y+1) + NoiseAt(x+1, y+1);
            var sides   = NoiseAt(x-1, y  ) + NoiseAt(x+1, y)   + NoiseAt(x,   y-1) + NoiseAt(x, y+1);
            var center  = NoiseAt(x, y);
            return center / 4 + sides / 8 + corners / 16;
        }

        float InterpolatedNoise(float x, float y) {
            var ix = x.Floor(); var truncx = x - ix;
            var iy = y.Floor(); var truncy = y - iy;
            var v1 = SmoothNoise(ix,   iy);
            var v2 = SmoothNoise(ix+1, iy);
            var v3 = SmoothNoise(ix  , iy+1);
            var v4 = SmoothNoise(ix+1, iy+1);
            var i1 = v1.Lerp(v2, truncx);
            var i2 = v3.Lerp(v4, truncx);
            return i1.Lerp(i2, truncy);
        }

        /// <summary>Polls the Perlin map for a value at floating point coordinates x, y. The resolution is technically infinite as every value is calculated on the fly. </summary>
        /// <param name="x">From 0 to 1. After that it cycles.</param>
        /// <param name="y">Same as x. </param>
        /// <returns>The value of the map at the specified coordinates. The values generated resemble a normal distribution with mean at around 0.5, with over 95% of tiles in 
        /// the range [0,1] . The actual skew of the distribution depends on retention and octaves settings, among other things.
        /// </returns>
        /// <remarks> If you want a chunkier map with larger masses, do not poll for the entire spectrum of [0, 1], but rather focus on a "subrect" 
        /// (i.e. experiment with offsets and scales)</remarks>
        public float this[float x, float y] { get {
            x *= LatticeDimensions;
            y *= LatticeDimensions;        
            var sum = 0f;
            var amplitude = 1f;
            var frequency = 1;
            for (var i = 0; i < octaves; i++) {
                amplitude *= persistence;
                sum += InterpolatedNoise(x * frequency, y * frequency) * amplitude;                
                frequency *= 2;
            }
            return 0.5f + sum; // the 0.5 is compensation for the fact we are generating source values ranging from -1 to 1
        } }

        


    }
}
