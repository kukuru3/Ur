namespace Ur.Random.Implementers {
    // adapted from http://www.pcg-random.org/download.html
    public class PCGSimple {
        ulong state;
        ulong increment;

        public PCGSimple(ulong sequenceConstant, ulong startingState) {
            this.increment = sequenceConstant;
            this.state = startingState;
        }

        public uint NextRandom() {
            var oldstate = state;
            state = oldstate * 6364136223846793005UL + (increment | 1);
            // calculate output function (xsh rr), uses old state for max ILP
            var xorshifted = (uint)(((oldstate >> 18) ^ oldstate) >> 27);
            var rot = (uint)(oldstate >> 59);
            return (xorshifted >> (int)rot) | (xorshifted << (int)((-rot) & 31));
        }

        public float NextFloat() {
            return (float)NextRandom() / uint.MaxValue;
        }

    }
}
