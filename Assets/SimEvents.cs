using System;
using UnityEngine;

namespace UnitySimuLean
{
    [CreateAssetMenu(fileName = "GameEvents", menuName = "Creator/Events/SO/GameEvents")]
    public class SimEvents : ScriptableObject
    {
        //-------------------------------------------------General Simulation Events-------------------------------------------------

        public delegate void OnStartSimulationCallback(float maxTime = 0);
        public OnStartSimulationCallback OnSimStart;

        public Action OnSimStop;
        public Action OnSimResume;
        public Action OnSimFinish;


        //Experimenter
        public Action OnExperimentStart;
        public Action OnExperimentFinish;

        //XML Serializer 
        public Action OnMonthPassed;

    }
}
