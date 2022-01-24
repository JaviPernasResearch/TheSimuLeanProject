using System;
using System.Collections.Generic;

namespace SimuLean
{
    /// <summary>
    /// Models MultiAssemble input queue to manage receptions.
    /// </summary>
    public class ConstrainedInput : Element
    {
        int capacity;
        int currentItems;

        int inputId;

        Queue<Item> itemsQ;

        ArrivalListener aListener;

        public ConstrainedInput(int capacity, ArrivalListener aListener, int inputId, String myName, SimClock sClock) : base(myName, sClock)
        {
            this.aListener = aListener;
            this.capacity = capacity;
            this.inputId = inputId;

            itemsQ = new Queue<Item>(capacity);
        }

        public override void Start()
        {
            itemsQ.Clear();
            currentItems = 0;
        }

        public Queue<Item> Release(int quantity)
        {
            Queue<Item> wholeItems = new Queue<Item>();

            for (int i = 0; i < quantity; i++)
            {
                wholeItems.Enqueue(itemsQ.Dequeue());
                currentItems--;

                GetInput().NotifyAvaliable(this);
            }

            return wholeItems;
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
            return true;
        }

        public override bool Receive(Item theItem)
        {
            if (currentItems < capacity || capacity < 0)
            {
                currentItems++;
                theItem.SetConstrainedInput(this.inputId); //For item placement
                itemsQ.Enqueue(theItem);
                aListener.GetVElement().LoadItem(theItem);

                aListener.ItemReceived(theItem, inputId);


                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return !(currentItems >= capacity || capacity < 0);
        }

        public int GetCapacity()
        {
            return capacity;
        }

        public Queue<Item> GetItems()
        {
            return itemsQ;
        }
    }
}

