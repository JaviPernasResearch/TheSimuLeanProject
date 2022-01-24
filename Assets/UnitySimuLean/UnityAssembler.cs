using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for simple Assembler (1 server) Element.
    /// </summary>
    public class UnityAssembler : SElement, VElement
    {

        PoissonProcess cycleTime;
        Assembler theAssembler;
        public GameObject itemPrefab;

        public Transform itemPosition;
        List<GameObject> gItem;
        public float separation = 1f;

        public string elementName = "AS";
        public int requirements;

        public double cTime = 2.0;

        void Start()
        {
            UnitySimClock.instance.elements.Add(this);

            gItem = new List<GameObject>(requirements);

            gItem.Clear();
        }

        override public void InitializeSim()
        {
            cycleTime = new PoissonProcess(cTime);

            theAssembler = new Assembler(elementName, UnitySimClock.instance.clock, cycleTime, requirements);

            theAssembler.vElement = this;
        }
        override public void StartSim()
        {
            if (itemPosition == null)
            {
                itemPosition = transform;
            }

            theAssembler.Start();
        }

        override public Element GetElement()
        {
            return theAssembler;
        }

        void VElement.ReportState(string msg)
        {
        }

        object VElement.GenerateItem(int type)
        {
            foreach (GameObject obj in gItem)
            {
                Destroy(obj);
            }
            gItem.Clear();

            GameObject newItem = Instantiate(itemPrefab) as GameObject;

            newItem.transform.position = itemPosition.position + new Vector3(0f, separation, 0f);

            return newItem;
        }

        void VElement.LoadItem(Item vItem)
        {
            GameObject mItem;

            mItem = (GameObject)vItem.vItem;

            gItem.Add(mItem);

            mItem.transform.position = itemPosition.position + new Vector3(0f, separation * gItem.Count, 0f);
        }

        void VElement.UnloadItem(Item vItem)
        {
        }

        public override void RestartSim()
        {
            List<Item> items = theAssembler.GetItem().GetSubItems();
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