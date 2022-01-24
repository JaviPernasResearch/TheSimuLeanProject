namespace SimuLean
{

    /// <summary>
    /// SimClock simulates the clock of the simulation. There must be one clock in the simulation to execute it. It is independent of unity Clock.
    /// </summary>
    public class SimClock
    {
        double simTime;

        DoubleMinBinaryHeap events;

        public SimClock()
        {
            events = new DoubleMinBinaryHeap(10);
            simTime = 0;


        }

        /// <summary>
        /// Default method to schedule an <paramref name="theEvent"/> at time (<paramref name="time"/> + simTime).
        /// </summary>
        /// <param name="theEvent"></param>
        /// <param name="time"></param>
        public void ScheduleEvent(Eventcs theEvent, double time)
        {
            events.Add(simTime + time, theEvent);
        }

        /// <summary>
        /// Advances sim time until <paramref name="time"/>.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool AdvanceClock(double time)
        {
            double t;
            Eventcs nextEvent;

            if (events.Count() == 0)
            {
                return false;
            }

            t = events.GetMinValue();

            while (t <= time)
            {
                simTime = t;


                nextEvent = (Eventcs)events.RetrieveFirst();
                nextEvent.Execute();

                if (events.Count() == 0)
                {
                    return false;
                }
                t = events.GetMinValue();
            }

            return true;
        }

        /// <summary>
        /// Resets SimClock and cleans events list.
        /// </summary>
        public void Reset()
        {
            simTime = 0;
            events.Reset();
        }

        /// <summary>
        /// Returns current simulation time in seconds.
        /// </summary>
        /// <returns></returns>
        public double GetSimulationTime()
        {
            return simTime;
        }
    }
}
