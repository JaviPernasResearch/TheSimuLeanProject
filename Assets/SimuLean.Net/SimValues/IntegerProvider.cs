namespace SimuLean
{
    /// <summary>
    /// Default interface for a distribution class that returns integer values.
    /// </summary>
    public interface IntegerProvider
    {
        /// <summary>
        /// Returns new integer value.
        /// </summary>
        /// <returns></returns>
        int ProvideValue();
    }
}
