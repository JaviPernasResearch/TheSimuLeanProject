using System;
using UnityEngine;

namespace SimuLean
{
    public class ProviderSource : Element
    {
        public bool useSocket = false;

        int numberIterms;

        int totPendingItems;

        DoubleRandomProcess leadTime;

        Item blockedItem;

        public ProviderSource(bool useSocket, String name, SimClock state, double minTime = 30.0, double maxTime = 60.0) : base(name, state)
        {
            this.useSocket = useSocket;

            if (!useSocket)
            {
                this.leadTime = new UniformDistribution(minTime, maxTime);
            }
        }

        public override void Start()
        {
            numberIterms = 0;
            totPendingItems = 0;
            //totOrders = 0;

            blockedItem = null;
        }

        public void Order(int quantity)
        {
            if (quantity > 0)
            {
                totPendingItems += quantity;

                OrderArrival newArrival = new OrderArrival(quantity, this);

                double time = leadTime.NextValue();


                simClock.ScheduleEvent(newArrival, time);
                Debug.Log(time);
            }
        }

        public void CreateItems(int quantity)
        {
            Item newItem;

            while (totPendingItems > 0 && quantity > 0)
            {
                newItem = CreateItem();
                if (!GetOutput().sendItem(newItem, this))
                {
                    blockedItem = newItem;
                    break;
                }
                else
                {
                    totPendingItems--;
                    quantity--;
                }
            }
        }

        public override bool Unblock()
        {
            Item newItem;

            if (blockedItem == null)
            {
                this.Order(1);
                return true;
            }
            else
            {
                newItem = blockedItem;
                blockedItem = null;
            }

            if (!GetOutput().sendItem(newItem, this))
            {
                blockedItem = newItem;
                //break;
            }
            else
            {
                totPendingItems--;
            }

            return true;
        }

        public int GetNumberItems()
        {
            return numberIterms;
        }

        public int GetPendingArrivals()
        {
            //return totOrders - numberIterms;
            return totPendingItems;
        }

        public override bool Receive(Item theItem)
        {
            throw new System.InvalidOperationException("The Source cannot receive Items."); //To change body of generated methods, choose Tools | Templates.
        }

        override public int GetQueueLength()
        {
            return 0;
        }

        override public int GetFreeCapacity()
        {
            return 0;
        }

        Item CreateItem()
        {
            numberIterms++;
            Item nItem = new Item(simClock.GetSimulationTime());
            nItem.SetId("type", 1, 1);
            nItem.vItem = vElement.GenerateItem(nItem.GetId());

            return nItem;
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return false;
        }
    }
}

