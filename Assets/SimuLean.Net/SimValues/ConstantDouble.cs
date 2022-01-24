namespace SimuLean
{
    /// <summary>
    /// Models a constant distribution.
    /// </summary>
    public class ConstantDouble : DoubleRandomProcess, DoubleProvider
    {
        double value = 1.0;

        public ConstantDouble(double val)
        {
            value = val;
        }

        double DoubleProvider.ProvideValue()
        {
            return value;
        }

        double DoubleRandomProcess.NextValue()
        {
            return value;
        }

        void DoubleRandomProcess.Initialize(double initialValue, double[] parameters)
        {
            value = initialValue;
        }
    }
}

