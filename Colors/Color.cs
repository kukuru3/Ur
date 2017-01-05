using System;
using System.Collections.Generic;

namespace Ur {

    public struct Color : IInterpolable<Color>
    {

        public float a;
        public float r;
        public float g;
        public float b;

        public Color(float r, float g, float b, float a = 1f)
        {
            this.a = a;
            this.r = r;
            this.g = g;
            this.b = b;
        }

        private const float inv256 = 1f / 256f;
        public static Color FromUnsignedInteger(uint col)
        {
            var a = ((col & 0xff000000) >> 24);
            if (a == 0) a = 255;
            return new Color(
                inv256 * ((col & 0x00ff0000) >> 16),
                inv256 * ((col & 0x0000ff00) >> 8),
                inv256 * ((col & 0x000000ff) >> 0),
                inv256 * (a)
            );
        }

        public static uint ToUnsignedInteger(Color c)
        { 
            
            return ((uint)Numbers.Floor(255f * c.r.Choke(0f, 1f)) << 16) |
                   ((uint)Numbers.Floor(255f * c.g.Choke(0f, 1f)) << 8 ) |
                   ((uint)Numbers.Floor(255f * c.b.Choke(0f, 1f)) << 0 ) |
                   ((uint)Numbers.Floor(255f * c.a.Choke(0f, 1f)) << 24);
        }

        public static implicit operator Color(uint input)
        {
            return FromUnsignedInteger(input);
        }

        
        public static Color White { get { return new Color(1,1,1,1);} }
        public static Color Black { get { return new Color(0,0,0,1);} }
        public static Color Transparent { get { return new Color(0,0,0,0); } }

        public Color Add(Color other, float multiplier)
        {
            return new Color(r + other.r * multiplier, g + other.g * multiplier, b + other.b * multiplier, a + other.a * multiplier);
        }

        public void Constrain()
        {
            a = a.Choke(0f, 1f);
            r = r.Choke(0f, 1f);
            g = g.Choke(0f, 1f);
            b = b.Choke(0f, 1f);
        }

        public bool IsNonZero() {
            return 
                r > float.Epsilon || 
                g > float.Epsilon ||
                b > float.Epsilon
            ;
        }

        public Color ToBrightness(float brightness) {
            brightness = brightness.Choke(0f, 0.9f);
            r = r.Choke(0.2f, 0.8f);
            g = g.Choke(0.2f, 0.8f);
            b = b.Choke(0.2f, 0.8f);
            var avg = (r + g + b) / 3f;
            if (avg.Approximately(0f)) return new Color(brightness, brightness, brightness, a);
            var factor = brightness / avg;
            return new Color(r * factor, g * factor, b * factor, a);
        }

        public static Color grey(float grayness, float alpha = 1f) {
            return new Color(grayness, grayness, grayness, alpha);
        }

        public void MultiplyRGB(float value) {
            r *= value;
            g *= value;
            b *= value;            
        }
        public void MultiplyRGB(Color other) {
            r *= other.r;
            g *= other.g;
            b *= other.b;
        }

        static public Color operator + (Color a, Color b) {
            return new Color( a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a); // gesundheit 
        }
        static public Color operator - (Color a, Color b) {
            return new Color( a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a); // gesundheit 
        }

        static public Color operator * (Color ca, float cb) {
            return new Color(ca.r * cb, ca.g * cb, ca.b * cb, ca.a);
        }

        public static Color FromHSV(float h, float s, float v)
        {
            float m, n, f;
            var i = (h * 6f).Floor();
            f = h * 6f - i;
            if (i % 2 == 0) f = 1f - f;
            m = v * (1 - s);
            n = v * (1 - s * f);

            switch (i)
            {
                case 0: return new Color(v, n, m);
                case 1: return new Color(n, v, m);
                case 2: return new Color(m, v, n);
                case 3: return new Color(m, n, v);
                case 4: return new Color(n, m, v);
                case 5: return new Color(v, m, n);
            }
            throw new System.Exception("Invalid HSV to RGB!");
            
        }

        public Color Lerp(Color other, float ratio) {
            return Colors.Lerp(this, other, ratio);
        }
    }

    public interface IInterpolable<T>
    {
        T Lerp(T other, float ratio);  
        T Add (T other, float mult);
    }
}