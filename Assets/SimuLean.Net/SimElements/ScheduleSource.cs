using System;
using System.Collections.Generic;
using System.IO;

namespace SimuLean
{
    class ScheduleSource : Element, Eventcs
    {
        Item lastItem;

        int numberIterms;

        String fileName;
        TextReader dataFile;

        Queue<Item> itemsInQueue;

        public ScheduleSource(String name, SimClock state, String fileName) : base(name, state)
        {
            this.fileName = fileName;
            dataFile = File.OpenText(fileName);
            itemsInQueue = new Queue<Item>();
        }

        public override void Start()
        {
            numberIterms = 0;
            ScheduleNext();
        }

        public override bool Unblock()
        {
            Item theItem;
            if (itemsInQueue.Count > 0)
            {
                theItem = itemsInQueue.Peek();
                if (this.GetOutput().sendItem(theItem, this))
                {
                    itemsInQueue.Dequeue();
                    ScheduleNext();
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
        void Eventcs.Execute()
        {

            if (this.GetOutput().sendItem(lastItem, this))
            {
                ScheduleNext();
            }
            else
            {
                itemsInQueue.Enqueue(lastItem);
                ScheduleNext();
            }
        }

        Item CreateItem()
        {
            Item nItem = new Item(simClock.GetSimulationTime());

            return nItem;
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return false;
        }

        void ScheduleNext()
        {
            lastItem = CreateItem();
            ReadFileAndSet();
            lastItem.vItem = vElement.GenerateItem(lastItem.GetId());
            numberIterms++;

            if (simClock.GetSimulationTime() > lastItem.GetCreationTime())
                simClock.ScheduleEvent(this, simClock.GetSimulationTime());
            else
                simClock.ScheduleEvent(this, lastItem.GetCreationTime());

        }

        void ReadFileAndSet()
        {

            string line;
            line = dataFile.ReadLine();
            string[] bits = line.Split(' ');

            lastItem.SetId(bits[0], int.Parse(bits[1]), int.Parse(bits[2]));
            lastItem.SetcreationTime(double.Parse(bits[3]));
        }

        public Queue<Item> GetItems()
        {
            return itemsInQueue;

        }
    }
}

