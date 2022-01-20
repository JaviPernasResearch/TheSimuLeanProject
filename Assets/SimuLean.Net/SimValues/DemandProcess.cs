using System;

namespace SimuLean
{
    public class DemandProcess : DoubleProvider
    {
        const double initTBA = 120.0;
        const double maxTBA = 60.0;
        const double endTBA = 60.0;

        const double midTime = 1800.0;
        const double endTime = 3600.0;

        RandomGenerator rg;

        SimClock theSimClock;

        public DemandProcess(SimClock theSimClock)
        {
            this.theSimClock = theSimClock;
            rg = new RandomGenerator();
        }

        double DoubleProvider.ProvideValue()
        {
            double time = theSimClock.GetSimulationTime();
            double mean = 1.0;

            if (time < midTime)
            {
                mean = initTBA + (maxTBA - initTBA) * (time) / (midTime);
            }
            else if (time < endTime)
            {
                mean = maxTBA + (endTBA - maxTBA) * (time - midTime) / (endTime - midTime);
            }
            else
            {
                mean = endTBA;
            }

            return -Math.Log(1 - rg.NextDouble()) * mean;
        }

    }
}

