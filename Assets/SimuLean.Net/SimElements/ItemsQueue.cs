using System;
using System.Collections.Generic;

namespace SimuLean
{
    /// <summary>
    /// Standard queue with FIFO rule.
    /// </summary>
    public class ItemsQueue : Element
    {
        int capacity;
        int currentItems;

        Queue<Item> itemsQ;

        public ItemsQueue(int capacity, String myName, SimClock sClock) : base(myName, sClock)
        {
            this.capacity = capacity;
            itemsQ = new Queue<Item>(capacity);
        }

        public override void Start()
        {
            itemsQ.Clear();
            currentItems = 0;
        }

        override public int GetQueueLength()
        {
            return currentItems;
        }
        override public int GetFreeCapacity()
        {
            return capacity - currentItems;
        }

        public override bool Unblock()
        {
            if (itemsQ.Count > 0)
            {
                Item theItem = itemsQ.Dequeue();
                currentItems--;
                vElement.ReportState("Exit");

                GetOutput().SendItem(theItem, this);

                GetInput().NotifyAvaliable(this);
                return true;
            }
            else if (capacity == 0)
            {
                return GetInput().NotifyAvaliable(this);
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
                if (!GetOutput().SendItem(theItem, this))
                {
                    itemsQ.Enqueue(theItem);
                    currentItems++;
                    vElement.LoadItem(theItem);
                }
                return true;
            }
            else if (capacity == 0)
            {
                return GetOutput().SendItem(theItem, this);
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
            return itemsQ;
        }

        public void SetCapacity(int capacity)
        {
            this.capacity = capacity;
        }
    }
}
