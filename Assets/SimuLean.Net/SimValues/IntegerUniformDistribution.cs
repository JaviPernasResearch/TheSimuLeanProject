namespace SimuLean
{
    /// <summary>
    /// Models a discrete uniform distribution.
    /// </summary>
    public class IntegerUniformDistribution : IntegerProvider
    {
        int min, max;

        RandomGenerator rg;

        public IntegerUniformDistribution(int min, int max)
        {
            this.min = min;
            this.max = max;
            rg = new RandomGenerator();
        }

        public int GetMin()
        {
            return min;
        }

        public void SetMin(int min)
        {
            this.min = min;
        }

        public int GetMax()
        {
            return max;
        }

        public void SetMax(int max)
        {
            this.max = max;
        }

        int IntegerProvider.ProvideValue()
        {
            return (int)rg.NextInt(min, max);
        }
    }
}

