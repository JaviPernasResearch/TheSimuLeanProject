using System;
using System.Collections.Generic;

namespace SimuLean
{
    public class GateQueue : Element
    {
        int capacity;
        int pendingRelease;
        int currentItems;

        Queue<Item> itemsQ;

        public GateQueue(int capacity, String myName, SimClock sClock) : base(myName, sClock)
        {
            this.capacity = capacity;
            itemsQ = new Queue<Item>(capacity);

            SimCosts.AddCost(SimCosts.storeCapacityCost * capacity);
        }

        public override void Start()
        {
            itemsQ.Clear();
            currentItems = 0;
            pendingRelease = 0;
        }

        public void Release(int quantity)
        {
            pendingRelease += quantity;

            DoTransfers();

        }

        void DoTransfers()
        {
            Item theItem;

            while (pendingRelease > 0 && currentItems > 0)
            {
                theItem = itemsQ.Peek();

                pendingRelease--;
                currentItems--;

                if (GetOutput().sendItem(theItem, this))
                {
                    itemsQ.Dequeue();
                    vElement.ReportState("Exit");
                }
                else
                {
                    pendingRelease++;
                    currentItems++;
                    break;
                }
            }

        }

        override public int GetQueueLength()
        {
            return currentItems;
        }

        public int GetPendingItems()
        {
            return pendingRelease;
        }

        override public int GetFreeCapacity()
        {
            return capacity - currentItems;
        }

        public override bool Unblock()
        {
            if (pendingRelease > 0 && currentItems > 0)
            {
                DoTransfers();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool Receive(Item theItem)
        {
            if (currentItems < capacity)
            {
                currentItems++;
                itemsQ.Enqueue(theItem);
                vElement.LoadItem(theItem);

                DoTransfers();
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return !(currentItems >= capacity);
        }

        public Queue<Item> GetItems()
        {
            Queue<Item> wholeItems = new Queue<Item>();

            foreach (Item it in itemsQ)
            {
                wholeItems.Enqueue(it);
            }

            return wholeItems;
        }

        public void SetCapacity(int capacity)
        {
            this.capacity = capacity;
        }
    }
}

