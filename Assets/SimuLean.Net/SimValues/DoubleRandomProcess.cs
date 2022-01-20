namespace SimuLean
{
    public interface DoubleRandomProcess
    {
        void Initialize(double initialValue, double[] parameters);

        double NextValue();
    }
}
