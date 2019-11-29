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
    }
}
