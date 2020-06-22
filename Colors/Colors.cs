namespace Ur {
    static public class Colors {
        static public Color Lerp(this Color a, Color b, float t) {
            return new Color(Numbers.Lerp(a.r, b.r, t),
                                Numbers.Lerp(a.g, b.g, t),
                                Numbers.Lerp(a.b, b.b, t),
                                Numbers.Lerp(a.a, b.a, t));
        }

        static public Color Lerp(this uint a, uint b, float t) {
            return Lerp((Color)a, (Color)b, t);
        }

        const double A = 1.0 / 3.0;
        const double B = 2.0 / 3.0;
        

        // CGA:
        static public Color Black => (0,0,0);
        static public Color Blue => (0, 0, 1);
        static public Color Green => (0, B, 0);
        static public Color Cyan => (0, B, B);
        static public Color Red => (B, 0, 0);
        static public Color Magenta => (B, 0, B);
        static public Color Brown => (B, A, 0);
        static public Color DarkGrey => (A, A, A);
        static public Color LightGrey => (B, B, B);
        static public Color LightBlue => (A, A, 1);
        static public Color LightGreen => (A, 1, A);
        static public Color LightCyan => (A, 1, 1);
        static public Color LightRed => (1, A, A);
        static public Color LightMagenta => (1, A, 1);
        static public Color Yellow => (1, 1, A);
        static public Color White => (1,1,1);

    }
}
