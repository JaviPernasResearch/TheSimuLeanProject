using SimuLean;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
    /// <summary>
    /// Component that instantiates the clock of the simulation.
    /// <remarks>
    /// UnitySimClock initializes all sim elements, starts and advances the simulation.
    /// It must be just one component of UnitySimClock script.
    /// </remarks>
    /// </summary>
    public class UnitySimClock : MonoBehaviour
    {
        static public UnitySimClock instance;

        public double timeScale = 1;

        public bool simOn = false;
        bool simStarted = false;
        public bool pause;

        public bool simRestarted = false;
        float pastTime = 0.0f;

        public SimClock clock = new SimClock();

        //Optional
        public float maxTime = Mathf.Infinity;

        public List<SElement> elements = new List<SElement>();
        public List<UnityMultiLink> mLinks = new List<UnityMultiLink>();

        Timer updateTime;

        void Awake()
        {
            instance = this;

            simOn = false;
        }

        void Start()
        {
            updateTime = new Timer();
        }

        void Update()
        {
            if (simOn && !simRestarted)
            {
                if (simStarted)
                {
                    clock.AdvanceClock((Time.time - pastTime + Time.deltaTime) * timeScale);

                    if (Time.time - pastTime > maxTime)
                    {
                        Debug.Log("Time is over");
                        this.QuitGame();
                    }
                }
                else
                {
                    foreach (SElement theElem in elements)
                    {
                        theElem.InitializeSim();
                    }
                    foreach (SElement theElem in elements)
                    {
                        theElem.ConnectSim();
                    }
                    foreach (UnityMultiLink umLink in mLinks)
                    {
                        umLink.ConnectSim();

                    }
                    foreach (SElement theElem in elements)
                    {
                        theElem.StartSim();
                    }

                    clock.AdvanceClock((Time.time - pastTime + Time.deltaTime) * timeScale);
                    simStarted = true;

                    clock.ScheduleEvent(updateTime, 0.1);
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        }

        /// <summary>
        /// Switches Unity TimeScale
        /// </summary>
        public void Pause()
        {
            pause = !pause;

            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        /// <summary>
        /// Resets and restarts Simulation
        /// </summary>
        public void RestartSim()
        {
            if (!simRestarted)
            {
                this.ResetSim();
            }

            foreach (SElement theElem in elements)
            {
                theElem.RestartSim();
            }

            pastTime = Time.time;
            simRestarted = false;

            simOn = !simOn;
        }

        /// <summary>
        /// Resets clock and stops simulation.
        /// </summary>
        public void ResetSim()
        {
            simRestarted = true;
            simOn = false;

            clock.Reset();
        }

        public float GetPastTime()
        {
            return pastTime;
        }

        /// <summary>
        /// Exits application
        /// </summary>
        public void QuitGame()
        {
#if UNITY_EDITOR
            Debug.Log("Quit Scene");
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}