using SimuLean;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for CustomerSink Element.
    /// </summary>
    public class UnityCustomer : SElement, VElement
    {
        public bool automaticDemand = true;

        public int capacity = 1;

        public int MinQuantity = 1;
        public int MaxQuantity = 10;

        public double xSeparation = 1.0, ySeparation = 1.0, zSeparation = 1.0;
        public int yLevels = 2, zLevels = 5;


        public Transform storagePosition;
        public Transform itemTruckPosition;

        public GameObject truck;
        List<GameObject> truckList;
        public Transform truckPosition;

        CustomerSink theCustomerSink;

        double lastSimTime = 0;

        //UI
        public Text displayPending;
        public Text capacityInputField;
        public Text displayStock;

        void Start()
        {
            UnitySimClock.instance.elements.Add(this);
        }

        void Update()
        {
            if (theCustomerSink != null && displayPending != null && displayStock != null)
            {
                double deltaT = UnitySimClock.instance.clock.GetSimulationTime() - lastSimTime;
                if (deltaT > 0)
                {
                    lastSimTime = UnitySimClock.instance.clock.GetSimulationTime();
                }

                displayPending.text = theCustomerSink.GetPendingOrders().ToString();
                displayStock.text = theCustomerSink.GetNumberItems().ToString();
            }
        }

        public void ShipTruck()
        {
            theCustomerSink.ShipTruck();
        }

        override public void InitializeSim()
        {
            if (capacityInputField != null)
                capacity = int.Parse(capacityInputField.text);

            theCustomerSink = new CustomerSink(capacity, name, UnitySimClock.instance.clock, MinQuantity, MaxQuantity);

            theCustomerSink.vElement = this;
        }
        override public void StartSim()
        {
            if (storagePosition == null)
            {
                storagePosition = gameObject.transform;
            }

            if (capacityInputField != null)
            {
                theCustomerSink.SetCapacity(int.Parse(capacityInputField.text));
            }

            theCustomerSink.Start();
        }

        override public Element GetElement()
        {
            return theCustomerSink;
        }

        void VElement.ReportState(string msg)
        {
            int xL, yL, zL;
            GameObject gItem;
            Queue<Item> items = theCustomerSink.GetItems();
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

            GameObject myTruck = Instantiate(truck);

            myTruck.transform.position = truckPosition.position;

            int i = 0;
            int yL, zL;
            foreach (Item it in vItem.GetSubItems())
            {
                if (it.vItem != null)
                {
                    GameObject myItem = (GameObject)it.vItem;

                    yL = i % yLevels;
                    zL = i / yLevels % zLevels;

                    myItem.transform.position = itemTruckPosition.position + new Vector3(0f, (float)(yL * ySeparation), (float)(-zL * zSeparation));
                    Destroy((GameObject)it.vItem, 10);

                    i++;
                }

            }
            Destroy(myTruck, 10);
            vElem.ReportState("");
        }

        public override void RestartSim()
        {
            Queue<Item> items = theCustomerSink.GetItems();
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