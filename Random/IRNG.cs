namespace Ur.Random {
    public interface IRng {
        int NextInt();
        float Next();
        float Next(float min, float max);
        int Next(int min, int max);
        int[] RandomBuffer(int count);
        byte[] RandomBytes(int count);
        float NextGaussian(float mean, float sigma);
    }
}
