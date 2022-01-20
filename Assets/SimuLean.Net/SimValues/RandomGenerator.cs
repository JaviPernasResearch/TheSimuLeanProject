using System;

namespace SimuLean
{
    class RandomGenerator
    {
        System.Random r;

        public RandomGenerator()
        {
            r = new Random(DateTime.Now.Millisecond);
        }

        public double NextDouble()
        {
            return r.NextDouble();
        }

        public double NextInt(int minValue, int maxValue)
        {
            return r.Next(minValue, maxValue);
        }

    }
}
