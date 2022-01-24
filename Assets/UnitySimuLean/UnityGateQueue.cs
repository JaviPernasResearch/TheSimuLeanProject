using SimuLean;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for GateQueue Element.
    /// </summary>
    public class UnityGateQueue : SElement, VElement
    {
        public string name = "GateQueue";

        public int capacity = 1;

        public double xSeparation = 1.0, ySeparation = 1.0, zSeparation = 1.0;
        public int yLevels = 2, zLevels = 5;

        public Transform storagePosition;

        GateQueue myGateQueue;

        //UI
        public Text releaseQuantity;
        public Text pendingItems;
        public Text stockedItems;
        public Text capacityInputField;

        //Right production orders
        public UnityMultiAssembler[] Assemblers;
        public int assemblerInput;
        int assemblerMultiplier = 0;

        void Start()
        {
            UnitySimClock.instance.elements.Add(this);
        }

        void Update()
        {
            if (pendingItems != null && myGateQueue != null && assemblerMultiplier != 0)
            {
                pendingItems.text = (myGateQueue.GetPendingItems() / assemblerMultiplier).ToString();
            }
            if (stockedItems != null && myGateQueue != null)
            {
                stockedItems.text = myGateQueue.GetQueueLength().ToString();
            }

        }

        public void Release()
        {
            int q;

            q = int.Parse(releaseQuantity.text) * assemblerMultiplier;

            Debug.Log("Release " + q.ToString());

            myGateQueue.Release(q);
        }

        override public void InitializeSim()
        {
            if (capacityInputField != null)
                capacity = int.Parse(capacityInputField.text);

            myGateQueue = new GateQueue(capacity, name, UnitySimClock.instance.clock);

            myGateQueue.vElement = this;
        }
        override public void StartSim()
        {
            if (storagePosition == null)
            {
                storagePosition = gameObject.transform;
            }

            if (Assemblers.Length != 0)
            {
                foreach (UnityMultiAssembler assem in Assemblers)
                {
                    MultiAssembler myAssembler = (MultiAssembler)assem.GetElement();

                    if (myAssembler.GetInputsCount() > 0)
                    {
                        assemblerMultiplier += myAssembler.GetInput(assemblerInput).GetCapacity();
                    }
                    else
                        assemblerMultiplier += myAssembler.GetInput(0).GetCapacity();
                }
            }

            if (capacityInputField != null)
            {
                myGateQueue.SetCapacity(int.Parse(capacityInputField.text));
            }

            myGateQueue.Start();
        }

        override public Element GetElement()
        {
            return myGateQueue;
        }

        void VElement.ReportState(string msg)
        {
            int xL, yL, zL;
            GameObject gItem;
            Queue<Item> items = myGateQueue.GetItems();
            int i = 0;

            foreach (Item it in items)
            {
                gItem = (GameObject)it.vItem;

                yL = i % yLevels;
                zL = i / yLevels % zLevels;
                xL = i / (yLevels * zLevels);

                if (gItem != null)
                {
                    gItem.transform.position = storagePosition.position + new Vector3((float)(xL * xSeparation), (float)(yL * ySeparation), (float)(zL * zSeparation));
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
            VElement vElem = this;
            vElem.ReportState("");
        }

        void VElement.UnloadItem(Item vItem)
        {
            VElement vElem = this;
            vElem.ReportState("");
        }

        public override void RestartSim()
        {
            Queue<Item> items = myGateQueue.GetItems();
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