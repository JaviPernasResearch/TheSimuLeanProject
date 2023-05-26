using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySimuLean.Utilities;

namespace UnitySimuLean
{
    public class Experimenter : MonoBehaviour
    {
        [SerializeField] float maxTime=100.0f;
        [SerializeField] int timeScale=100;

        [System.Serializable]
        public class ListWrapper
        {
            public List<float> myList;
        }

        [Header ("Input Variables")]
        [SerializeField] List<SElement> iElements;
        [SerializeField] List<string> iParameters;
        [SerializeField] List<ListWrapper> scenarios;

        //FileWriter fileWriter;
        FileWriterCSV fileWriter;

        [Header("Output Variables")]
        [SerializeField] List<SElement> oElements;
        [SerializeField] List<string> Variables;

        int currentScenario = 0;

        #region Events Set Up
        private void OnEnable()
        {
            UnitySimClock.Instance.SimEvents.OnExperimentStart += this.StartExperiment;

        }

        private void OnDisable()
        {
            UnitySimClock.Instance.SimEvents.OnExperimentStart -= this.StartExperiment;
        }
        #endregion

        private void Start()
        {
            fileWriter = new FileWriterCSV("OutData-" + DateTime.Today.Day.ToString());
        }

        private void StartExperiment()
        {
            currentScenario = 0;
            this.SetScenario(currentScenario);
            UnitySimClock.Instance.SimEvents.OnSimStart.Invoke(maxTime);

            UnitySimClock.Instance.SetTimeScale(this.timeScale);

            fileWriter.AddText(new string[] { "Instance", "Element", "Variable", "Value" });

            Debug.Log("Experiment Start. Timescale Adjusted");
        }
        private void FinishExperiment()
        {
            currentScenario = 0;
            UnitySimClock.Instance.SimEvents.OnExperimentFinish.Invoke();
            UnitySimClock.Instance.SetTimeScale(1);

            Debug.Log("Experiment Finish. Output file created.");
        }


        private bool SetScenario(int nextScenario)
        {
            if (nextScenario >= scenarios.Count)
                return false;
            
            for (int i = 0; i < iElements.Count; i++)
            {
                SetPropertyValue(iElements[i], iParameters[i], scenarios[nextScenario].myList[i]);

            }
            return true;
        }
        public static void SetPropertyValue(object obj, string propertyName, float value)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property != null && property.CanWrite)
            {
                if (property.PropertyType == typeof(int))
                {
                    // Convert the nullable int value to int if the property type is int
                    property.SetValue(obj, (int)value);
                }
                else if (property.PropertyType == typeof(float))
                {
                    // Convert the nullable int value to float if the property type is float
                    property.SetValue(obj, (float)value);
                }
                else
                {
                    throw new ArgumentException($"Property '{propertyName}' does not have a compatible type.");
                }
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' not found or is read-only.");
            }
        }

        public static int? GetPropertyValue(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);
            if (property != null && property.CanRead)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    if (property.PropertyType == typeof(int))
                    {
                        // Convert the property value to int if the property type is int
                        return (int)value;
                    }
                    else if (property.PropertyType == typeof(float))
                    {
                        // Convert the float property value to int
                        return Mathf.RoundToInt((float)value);
                    }
                    else
                    {
                        throw new ArgumentException($"Property '{propertyName}' does not have a compatible type.");
                    }
                }
            }
            else
            {
                throw new ArgumentException($"Property '{propertyName}' not found or is write-only.");
            }

            // Return null if the property value is null or the property is not found
            return null;
        }

        internal void NextScenario()
        {
            currentScenario++;

            for (int i = 0; i < oElements.Count; i++)
            {
                fileWriter.AddText(new string[] { (currentScenario - 1).ToString(), oElements[i].name, Variables[i], GetPropertyValue(oElements[i], Variables[i]).ToString() });

            }

            if (!SetScenario(currentScenario))
                this.FinishExperiment();
            else
                UnitySimClock.Instance.SimEvents.OnSimStart.Invoke(maxTime);

        }
    }
}
