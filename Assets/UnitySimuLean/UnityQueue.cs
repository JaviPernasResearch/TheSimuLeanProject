using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for ItemsQueue Element.
    /// </summary>
    public class UnityQueue : SElement, VElement
    {

        ItemsQueue theQueue;

        private int capacity = 1;

        public float separation = 1f;

        public Transform itemPosition;

        public int Capacity { get => capacity; set => capacity = value; }
        public int Content { get => theQueue.GetQueueLength(); }

        void Start()
        {
            UnitySimClock.Instance.Elements.Add(this);
            if (itemPosition == null)
            {
                itemPosition = transform;
            }
        }

        override public void InitializeSim()
        {
            theQueue = new ItemsQueue(capacity, name, UnitySimClock.Instance.clock);

            theQueue.vElement = this;
        }
        override public void StartSim()
        {
            theQueue.Start();
        }

        override public Element GetElement()
        {
            return theQueue;
        }

        void VElement.ReportState(string msg)
        {
            GameObject gItem;
            Queue<Item> items = theQueue.GetItems();
            int i = 1;

            foreach (Item it in items)
            {
                gItem = (GameObject)it.vItem;

                if (gItem != null)
                    gItem.transform.position = itemPosition.position + new Vector3(0f, separation * i, 0f);

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
                gItem.transform.position = itemPosition.position + new Vector3(separation * theQueue.GetItems().Count, 0f, 0f);
        }

        void VElement.UnloadItem(Item vItem)
        {
        }

        public override void RestartSim()
        {
            Queue<Item> items = theQueue.GetItems();
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