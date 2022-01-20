using System;

namespace SimuLean
{
    public class Sink : Element
    {
        int numberIterms;

        public Sink(String name, SimClock state) : base(name, state)
        {
        }

        public int GetNumberIterms()
        {
            return numberIterms;
        }

        public override void Start()
        {
            numberIterms = 0;
        }

        public override bool Unblock()
        {
            throw new System.InvalidOperationException("The Sink cannot receive notifications."); //To change body of generated methods, choose Tools | Templates.
        }

        override public int GetQueueLength()
        {
            return 0;
        }
        override public int GetFreeCapacity()
        {
            return -1;
        }

        public override bool Receive(Item theItem)
        {
            vElement.LoadItem(theItem);
            numberIterms++;
            return true;
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return true;
        }
    }
}
