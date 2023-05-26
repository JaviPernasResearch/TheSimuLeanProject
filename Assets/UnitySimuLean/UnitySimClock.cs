using SimuLean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        private static UnitySimClock instance;
        public static UnitySimClock Instance
        {
            get
            {
                if (instance == null)
                {
                    UnitySimClock.instance = FindObjectOfType<UnitySimClock>();
                    if (instance == null)
                        throw new System.Exception("No UnitySimclock Instance found in scene");
                }
                return instance;
            }
        }

        private double timeScale = 1;

        bool simOn = false;
        bool simStarted = false;
        bool simStopped = false;
        bool simRestarted = false;

        float pastTime = 0.0f;

        public SimClock clock = new SimClock();

        [Header("Events Scriptable Objects")]
        [SerializeField] private SimEvents simEvents;

        //Experimenter Mode
        [SerializeField] bool ExperimenterOn = false;
        [SerializeField] Experimenter experimenter;

        //Optional
        [SerializeField] private float maxTime = 1000.0f;

        private List<SElement> elements = new List<SElement>();
        private List<UnityMultiLink> mLinks = new List<UnityMultiLink>();


        Timer timer;


        #region Getters
        public float MaxTime { get => maxTime; set => maxTime = value; }
        public List<SElement> Elements { get => elements; set => elements = value; }
        public List<UnityMultiLink> MLinks { get => mLinks; set => mLinks = value; }

        public SimEvents SimEvents { get => this.simEvents; }
        #endregion

        #region Events Set Up
        private void OnEnable()
        {
            UnitySimClock.Instance.SimEvents.OnSimStart += this.RestartSim;
            UnitySimClock.Instance.SimEvents.OnSimStop += this.PauseSim;
            UnitySimClock.Instance.SimEvents.OnSimResume += this.ResumeSim;
            UnitySimClock.Instance.SimEvents.OnExperimentFinish += this.FinishSim;
        }

        private void OnDisable()
        {
            UnitySimClock.Instance.SimEvents.OnSimStart -= this.RestartSim;
            UnitySimClock.Instance.SimEvents.OnSimStop -= this.PauseSim;
            UnitySimClock.Instance.SimEvents.OnSimResume -= this.ResumeSim;
            UnitySimClock.Instance.SimEvents.OnExperimentFinish -= this.FinishSim;
        }
        #endregion

        void Awake()
        {
            instance = this;

            simOn = false;
        }

        void Start()
        {
            timer = new Timer();

            if (ExperimenterOn && experimenter != null) {
                UnitySimClock.Instance.SimEvents.OnExperimentStart.Invoke();
            }
            else
            {
                simOn = true;
            }
        }

        void Update()
        {
            if (simOn && !simRestarted &&!simStopped)
            {
                if (simStarted)
                {
                    clock.AdvanceClock((Time.time - timer.PastTime + Time.deltaTime) * timeScale);

                    if (clock.GetSimulationTime() > maxTime)
                    {
                        if (ExperimenterOn)
                        {
                            experimenter.NextScenario();
                        }
                        else
                        {
                            Debug.Log("Time is over");
                            UnitySimClock.Instance.SimEvents.OnSimFinish.Invoke();
                        }
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

                    clock.AdvanceClock((Time.time - timer.PastTime + Time.deltaTime) * timeScale);
                    simStarted = true;
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
                this.Quit();
        }

        /// <summary>
        /// Switches Off Unity TimeScale
        /// </summary>
        public void PauseSim()
        {
            simStopped = true;
        }        
        
        /// <summary>
        /// Switches Unity TimeScale
        /// </summary>
        public void ResumeSim()
        {
            simStopped = false;
        }


        /// <summary>
        /// Resets and restarts Simulation
        /// </summary>
        public void RestartSim(float maxTime = 0)
        {
            if (simStarted)
            {
                this.ResetSim();
                foreach (SElement theElem in elements)
                {
                    theElem.RestartSim();
                }
                StartCoroutine(this.BreatheSim());
            }

            simStarted = false;
            simRestarted = false;
            simStopped = false;

            this.maxTime = maxTime;
            timer.OnRestart();

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

        public void FinishSim() {
            simOn = false;
            ExperimenterOn = false;
            ResetSim();
        }

        public void SetTimeScale(int timeScale = 1)
        {
            this.timeScale = timeScale;

            if (timeScale <= 1)
            {
                Application.targetFrameRate = -1;
            }
            else
            {
                float factor = Mathf.Clamp(timeScale, 0, 600);
                Application.targetFrameRate = (int)(60 / timeScale);
            }

        }

        private IEnumerator BreatheSim()
        {
            yield return new WaitForSeconds(0.5f);
            // Code to be executed after the delay
        }

        public void Quit()
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