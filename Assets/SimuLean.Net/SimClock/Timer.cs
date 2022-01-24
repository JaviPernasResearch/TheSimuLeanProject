using UnitySimuLean;

namespace SimuLean
{
    /// <summary>
    /// Class to simulate a timer that executes an event every 0.1 seconds.
    /// It may be used to show sim time in a more fluently mode when existing GUI.
    /// </summary>
    class Timer : Eventcs
    {
        void Eventcs.Execute()
        {
            UnitySimClock.instance.clock.ScheduleEvent(this, 0.1);
        }
    }
}
