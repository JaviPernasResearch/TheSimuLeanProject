namespace SimuLean
{
    /// <summary>
    /// Default interface for a distribution class that returns double values.
    /// </summary>
    public interface DoubleRandomProcess
    {
        /// <summary>
        /// Default method to initialize distribution.
        /// </summary>
        /// <param name="initialValue"></param>
        /// <param name="parameters"></param>
        void Initialize(double initialValue, double[] parameters);

        /// <summary>
        /// Returns a new double value.
        /// </summary>
        /// <returns></returns>
        double NextValue();
    }
}
