using System;
using System.Collections;

namespace SimuLean
{
    /// <summary>
    /// Models Forklift behaviour absed on a single server.
    /// </summary>
    public class Forklift : Element, WorkStation
    {
        ServerProcess theProcess;

        int currentItems;
        int capacity;

        string name;

        public bool atPickPoint;
        public bool readyToLeave = false;
        bool isReceiving = false;

        public Forklift(String name, SimClock sClock, int capacity) : base(name, sClock)
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
                    if (GetOutput().SendItem(it, this))
                    {
                        itemsToRemove.Add(it);
                        currentItems--;
                    }
                    else
                    {
                        foreach (Item itt in itemsToRemove)
                        {
                            itemsStoraged.Remove(itt);
                        }
                        vElement.ReportState("Update items");
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
            readyToLeave = false;
            theProcess.ClearList();
            GetInput().NotifyAvaliable(this);

            return true;
        }

        public override bool Receive(Item theItem)
        {

            if (currentItems >= capacity || theProcess.state != 0)
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
                        isReceiving = false;
                        vElement.LoadItem(theItem);

                        Item myItems = theItem;

                        foreach (Item it in theProcess.GetItems())
                        {
                            myItems.AddItem(it);
                        }

                        theProcess.state = 1;
                        vElement.UnloadItem(myItems);
                    }
                    else
                    {
                        vElement.LoadItem(theItem);

                        if (isReceiving != true)
                        {
                            isReceiving = true;
                            simClock.ScheduleEvent(this.theProcess, 2);
                        }
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
            if (isReceiving == true)
            {
                if (!GetInput().NotifyAvaliable(this))
                {
                    Item myItems = null;

                    foreach (Item it in theProcess.GetItems())
                    {
                        if (myItems == null) //Item container
                        {
                            myItems = it;
                        }

                        myItems.AddItem(it);
                    }

                    isReceiving = false;
                    theProcess.state = 1;
                    vElement.UnloadItem(myItems);

                }

                return;
            }

            else if (readyToLeave == true)
            {
                ArrayList itemsStoraged = theProcess.GetItems();
                ArrayList itemsToRemove = new ArrayList();

                foreach (Item it in theProcess.GetItems())
                {

                    if (GetOutput().SendItem(it, this))
                    {
                        itemsToRemove.Add(it);
                        currentItems--;
                    }
                    else
                    {
                        foreach (Item itt in itemsToRemove)
                        {
                            itemsStoraged.Remove(itt);
                        }
                        theProcess.state = 2;
                        vElement.ReportState("Update Items");
                        return;
                    }
                }

                readyToLeave = false;
                theProcess.ClearList();
                theProcess.state = 0;
                GetInput().NotifyAvaliable(this);

                return;
            }

        }

        public override bool CheckAvaliability(Item theItem)
        {
            return !(currentItems >= capacity || theProcess.state != 0);
        }


        public void PickItem() //Called once the forklift arrives at the origin
        {
            GetInput().NotifyAvaliable(this);

        }

        public void LeaveItem() //Called once the forklift arrives at the destination
        {
            simClock.ScheduleEvent(theProcess, 0.0);

        }

        public ArrayList GetItems()
        {

            return theProcess.GetItems();

        }
    }

}
