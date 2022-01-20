using System;
using System.Collections.Generic;

namespace SimuLean
{
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

                GetOutput().sendItem(theItem, this);

                GetInput().notifyAvaliable(this);
                return true;
            }
            else if (capacity == 0)
            {

                return GetInput().notifyAvaliable(this);
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
                if (!GetOutput().sendItem(theItem, this))
                {
                    itemsQ.Enqueue(theItem);
                    currentItems++;
                    vElement.LoadItem(theItem);
                }
                return true;
            }
            else if (capacity == 0)
            {
                if (GetOutput().sendItem(theItem, this))
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
