using UnityEngine;
using UnitySimuLean;

namespace UnitySimuLean
{
    /// <summary>
    /// Tracks simulation time
    /// </summary>
    internal class Timer
    {
        float pastTime = 0;

        public float PastTime { get => pastTime; set => pastTime = value; }

        internal Timer()
        {
            pastTime = 0;
        }
           
        public void OnRestart()
        {
            pastTime = Time.time;
        }

        public float GetSimTime()
        {
            return Time.fixedTime - pastTime;
        }
    }
}
