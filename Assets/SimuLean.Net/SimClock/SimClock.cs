namespace SimuLean
{

    public class SimClock
    {
        double simTime;

        DoubleMinBinaryHeap events;

        public SimClock()
        {
            events = new DoubleMinBinaryHeap(10);
            simTime = 0;


        }

        public void ScheduleEvent(Eventcs theEvent, double time)
        {
            events.Add(simTime + time, theEvent);
        }

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
                SimCosts.AddCost((t - simTime) * Element.GetInventory() * SimCosts.inventoryUnitCost);

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

        public void Reset()
        {
            simTime = 0;
            events.Reset();
        }

        public double GetSimulationTime()
        {
            return simTime;
        }
    }
}
