using UnitySimuLean;

namespace SimuLean
{
    class Timer : Eventcs
    {
        void Eventcs.Execute()
        {
            UnitySimClock.instance.clock.ScheduleEvent(this, 0.1);
        }
    }
}
