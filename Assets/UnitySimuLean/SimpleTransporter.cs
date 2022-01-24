using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for Transporter (1DoF crane) Element.
    /// </summary>
    public class SimpleTransporter : SElement, VElement
    {

        ConstantDouble[] travelTime;
        MultiServer theWorkstation;

        public double speed = 1.0;
        public Transform origin;
        public Transform destination;
        public float height = 1f;
        public int capacity = 1;

        Vector3 odVector;
        float length;

        void Start()
        {
            travelTime = new ConstantDouble[capacity];
            UnitySimClock.instance.elements.Add(this);
        }

        override public void InitializeSim()
        {
            if (origin != null & destination != null)
            {
                odVector = destination.position - origin.position;
                length = odVector.magnitude;
                Debug.Log("Length " + length.ToString() + " " + name);

                for (int i = 0; i < capacity; i++)
                {
                    travelTime[i] = new ConstantDouble(length / speed);
                }
            }
            else
            {
                for (int i = 0; i < capacity; i++)
                {
                    travelTime[i] = new ConstantDouble(1.0);
                }
            }

            theWorkstation = new MultiServer(travelTime, name, UnitySimClock.instance.clock);

            theWorkstation.vElement = this;
        }
        override public void StartSim()
        {
            if (origin == null)
            {
                origin = transform;
            }
            if (destination == null)
            {
                destination = transform;
            }

            theWorkstation.Start();
        }

        void FixedUpdate()
        {
            float p;
            GameObject gItem;

            if (theWorkstation != null)
            {

                foreach (ServerProcess sProcess in theWorkstation.workInProgress)
                {
                    gItem = (GameObject)sProcess.theItem.vItem;

                    p = ((float)Time.time - (float)sProcess.loadTime) / (float)sProcess.lastDelay;

                    if (gItem != null && p <= 1)
                    {
                        gItem.transform.position = origin.position + odVector * p + new Vector3(0f, height, 0f);
                    }
                }
            }
        }

        override public Element GetElement()
        {
            return theWorkstation;
        }

        void VElement.ReportState(string msg)
        {
        }

        object VElement.GenerateItem(int type)
        {
            return null;
        }

        void VElement.LoadItem(Item vItem)
        {
            GameObject gItem;
            gItem = (GameObject)vItem.vItem;

            if (gItem != null)
            {
                gItem.transform.position = origin.position;
            }

        }

        void VElement.UnloadItem(Item vItem)
        {
        }
        public override void RestartSim()
        {
            Queue<Item> items = theWorkstation.GetItems();
            int i = 0;

            foreach (Item it in items)
            {
                Destroy((GameObject)it.vItem);
                i++;
            }

            StartSim();
        }
    }
}