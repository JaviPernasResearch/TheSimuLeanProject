using System;

namespace SimuLean
{
    /// <summary>
    /// Models a poisson process.
    /// </summary>
    public class PoissonProcess : DoubleRandomProcess, DoubleProvider
    {
        double mean;

        RandomGenerator rg;

        public PoissonProcess(double mean)
        {
            this.mean = mean;
            rg = new RandomGenerator();
        }

        public double GetMean()
        {
            return mean;
        }

        public void SetMean(double mean)
        {
            this.mean = mean;
        }

        double DoubleProvider.ProvideValue()
        {
            return -Math.Log(1 - rg.NextDouble()) * mean;
        }

        double DoubleRandomProcess.NextValue()
        {
            return -Math.Log(1 - rg.NextDouble()) * mean;
        }

        void DoubleRandomProcess.Initialize(double initialValue, double[] parameters)
        {
            mean = parameters[0];
        }
    }
}
