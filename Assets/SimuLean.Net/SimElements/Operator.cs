using System;
using System.Collections;

namespace SimuLean
{
    public class Operator : Element, WorkStation
    {
        ServerProcess theProcess;

        int currentItems;
        int capacity;

        string name;

        public bool atPickPoint;

        public Operator(String name, SimClock sClock, int capacity) : base(name, sClock)
        {
            this.name = name;
            this.capacity = capacity;
        }

        public override void Start()
        {

            theProcess = new ServerProcess(this, new PoissonProcess(1), 1);

            currentItems = 0;
            atPickPoint = false;
        }

        override public int GetQueueLength()
        {
            return currentItems;
        }
        override public int GetFreeCapacity()
        {
            return capacity - currentItems;
        }

        string WorkStation.GetName()
        {
            return name;
        }

        public override bool Unblock()
        {
            if (theProcess.state == 2)
            {
                ArrayList itemsStoraged = theProcess.GetItems();
                ArrayList itemsToRemove = new ArrayList();

                foreach (Item it in itemsStoraged)
                {
                    if (GetOutput().sendItem(it, this))
                    {
                        itemsToRemove.Add(it);
                        currentItems--;
                        vElement.ReportState("Exit 1");
                    }
                    else
                    {
                        foreach (Item itt in itemsToRemove)
                        {
                            itemsStoraged.Remove(itt);
                        }
                        theProcess.state = 2;
                        return false;
                    }

                    vElement.ReportState("Exit all");
                }
            }
            else
            {
                return false;
            }

            theProcess.state = 0;
            theProcess.ClearList();
            GetInput().notifyAvaliable(this);

            return true;
        }

        public override bool Receive(Item theItem)
        {
            if (currentItems >= capacity || theProcess.state == 2)
            {
                return false;
            }
            else
            {
                if (atPickPoint == true)
                {
                    theProcess.AddItem(theItem);
                    currentItems++;

                    if (currentItems >= capacity)
                    {
                        Item myItems = theItem;
                        foreach (Item it in theProcess.GetItems())
                        {
                            myItems.AddItem(it);
                        }

                        vElement.UnloadItem(myItems);
                    }

                    return true;
                }
                else
                {
                    vElement.LoadItem(theItem);

                    return false;
                }

            }
        }

        void WorkStation.CompleteServerProcess(ServerProcess theProcess)
        {

            ArrayList itemsStoraged = theProcess.GetItems();
            ArrayList itemsToRemove = new ArrayList();


            foreach (Item it in theProcess.GetItems())
            {

                if (GetOutput().sendItem(it, this))
                {
                    itemsToRemove.Add(it);
                    currentItems--;
                    vElement.ReportState("Exit 1");
                }
                else
                {
                    foreach (Item itt in itemsToRemove)
                    {
                        itemsStoraged.Remove(itt);
                    }
                    theProcess.state = 2;
                    return;
                }

            }

            theProcess.ClearList();
            GetInput().notifyAvaliable(this);

            return;

        }

        public override bool CheckAvaliability(Item theItem)
        {
            return !(currentItems >= capacity);
        }

        public void PickItem() //Called once the operator arrives at the origin
        {
            GetInput().notifyAvaliable(this);

        }

        public void LeaveItem() //Called once the operator arrives at the destination
        {
            simClock.ScheduleEvent(theProcess, 0.0);

        }
    }
}
