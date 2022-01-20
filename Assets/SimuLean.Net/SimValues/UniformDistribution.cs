namespace SimuLean
{
    public class UniformDistribution : DoubleRandomProcess, DoubleProvider
    {
        double min, width;

        RandomGenerator rg;

        public UniformDistribution(double min, double max)
        {
            this.min = min;
            this.width = max - min;
            rg = new RandomGenerator();
        }

        public double GetMin()
        {
            return min;
        }

        public void SetMin(double min)
        {
            this.min = min;
        }

        public double GetMax()
        {
            return min + width;
        }

        public void SetMax(double max)
        {
            this.width = max - min;
        }

        double DoubleProvider.ProvideValue()
        {
            return min + width * rg.NextDouble();
        }

        double DoubleRandomProcess.NextValue()
        {
            return min + width * rg.NextDouble();
        }

        void DoubleRandomProcess.Initialize(double initialValue, double[] parameters)
        {
            min = parameters[0];
            width = parameters[1] - min;
        }
    }
}

