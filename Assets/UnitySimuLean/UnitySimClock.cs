using SimuLean;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
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

        public Text timeCounter;
        public Text earningCounter;
        public float maxTime;

        public List<SElement> elements = new List<SElement>();
        public List<UnityMultiLink> mLinks = new List<UnityMultiLink>();

        Timer updateTime;

        //Data to Export
        string fileName;
        StreamWriter sr;

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

                    if (timeCounter != null)
                    {
                        timeCounter.text = Math.Round(clock.GetSimulationTime(), 1).ToString();
                    }
                    if (earningCounter != null)
                    {
                        earningCounter.text = Math.Round(SimCosts.GetEarnings()).ToString();
                    }

                    if (Time.time - pastTime > maxTime)
                    {
                        GenerateReport();
                        Debug.Log("Time is over");
                        Application.Quit();
                    }
                }
                else
                {
                    foreach (SElement theElem in elements)
                    {
                        theElem.InitializeSim(); //Es necesario darle nombres fijos a cada uno. Los assembler y Multiserver se identifican por él
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

        public void ConfigureScenario()
        {
            simOn = !simOn;

            SimCosts.RestartEarnings();

            if (simRestarted == true)
            {
                foreach (SElement theElem in elements)
                {
                    theElem.RestartSim();
                }

                pastTime = Time.time;
                simRestarted = false;
            }
        }

        public void RestartSim()
        {
            simRestarted = true;
            simOn = false;

            earningCounter.text = "0";
            timeCounter.text = "0";

            clock.Reset();
        }

        public float GetPastTime()
        {
            return pastTime;
        }

        //UI
        public void GenerateReport()
        {
            DateTime moment = new DateTime();

            fileName = "Report_" + moment.Hour + "_" + moment.Day + "_" + moment.Month + ".txt";
            sr = File.CreateText(fileName);
            sr.Write("On" + DateTime.Today + Environment.NewLine);
            foreach (SElement theElem in elements)
            {
                if (theElem.GetReport() != null)
                {
                    sr.Write(theElem.GetReport());
                    sr.WriteLine(Environment.NewLine);
                }
            }

            sr.Write("Beneficio total:" + earningCounter.text);
            sr.WriteLine(Environment.NewLine);

            sr.Close();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}