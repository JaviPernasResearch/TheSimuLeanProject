using SimuLean;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for MultiServer (infinite servers) Element.
    /// </summary>
    public class UnityMultiServer : SElement, VElement
    {

        PoissonProcess[] cycleTime;
        MultiServer theWorkstation;

        public Transform itemPosition;
        public Transform outItemPosition;

        public string elementName = "WS";

        public double cTime = 2.0;
        public int capacity = 1;
        public float separation = 1f;
        Vector3 odVector;

        //Animation
        public Animator serverAnimator;

        void Start()
        {

            UnitySimClock.Instance.Elements.Add(this);
        }

        override public void InitializeSim()
        {
            cycleTime = new PoissonProcess[capacity];
            for (int i = 0; i < capacity; i++)
            {
                cycleTime[i] = new PoissonProcess(cTime);
            }

            theWorkstation = new MultiServer(cycleTime, elementName, UnitySimClock.Instance.clock);

            theWorkstation.vElement = this;
            if (outItemPosition != null)
                odVector = outItemPosition.position - itemPosition.position;
        }

        override public void StartSim()
        {
            if (itemPosition == null)
            {
                itemPosition = transform;
            }

            theWorkstation.Start();
        }

        void FixedUpdate()
        {

            float p;
            GameObject gItem;

            if (theWorkstation != null && outItemPosition != null)
            {
                foreach (ServerProcess sProcess in theWorkstation.workInProgress)
                {
                    gItem = (GameObject)sProcess.theItem.vItem;

                    p = ((float)Time.time - (float)sProcess.loadTime) / (float)sProcess.lastDelay;

                    if (gItem != null && p <= 1)
                    {
                        gItem.transform.position = itemPosition.position + odVector * p + new Vector3(0f, separation, 0f);
                    }
                }
                if (serverAnimator != null)
                {
                    if (theWorkstation.GetItems().Count > 0)
                    {
                        serverAnimator.SetBool("WorkInProgress", true);
                    }
                    else
                        serverAnimator.SetBool("WorkInProgress", false);
                }
            }
        }

        override public Element GetElement()
        {
            return theWorkstation;
        }

        void VElement.ReportState(string msg)
        {
            GameObject gItem;
            Queue<Item> items = theWorkstation.GetItems();
            int i = 0;

            foreach (Item it in items)
            {
                gItem = (GameObject)it.vItem;
                if (gItem != null)
                {
                    gItem.transform.position = itemPosition.position + new Vector3(0f, separation * i, 0f);
                }

                i++;
            }
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
                gItem.transform.position = itemPosition.position + new Vector3(0f, separation * (theWorkstation.GetQueueLength() - 1), 0f);
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