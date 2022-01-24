using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for ScheduleSource Element.
    /// </summary>
    public class UnityScheduleSource : SElement, VElement
    {
        ScheduleSource theSource;

        public GameObject itemPrefab;

        public string myName;
        public string fileName;

        override public void InitializeSim()
        {
            theSource = new ScheduleSource(myName, UnitySimClock.instance.clock, fileName);
            theSource.vElement = this;
        }
        override public void StartSim()
        {
            theSource.Start();
        }

        // Use this for initialization
        void Start()
        {
            UnitySimClock.instance.elements.Add(this);
        }

        // Update is called once per frame
        void Update()
        {
        }

        override public Element GetElement()
        {
            return theSource;
        }

        void VElement.ReportState(string msg)
        {
        }

        object VElement.GenerateItem(int myId)
        {
            GameObject newItem = Instantiate(itemPrefab) as GameObject;

            newItem.SetActive(true);

            newItem.transform.position = transform.position;

            return newItem;
        }

        void VElement.LoadItem(Item vItem)
        {
            GameObject gItem;

            gItem = (GameObject)vItem.vItem;

            gItem.transform.position = transform.position;
        }

        void VElement.UnloadItem(Item vItem)
        {
        }

        public override void RestartSim()
        {
            Queue<Item> items = theSource.GetItems();
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