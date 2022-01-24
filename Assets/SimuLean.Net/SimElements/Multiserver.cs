using System;
using System.Collections.Generic;

namespace SimuLean
{
    /// <summary>
    /// Models a multiserver workstation.
    /// </summary>
    public class MultiServer : Element, WorkStation
    {
        Queue<ServerProcess> idleProccesses;
        public List<ServerProcess> workInProgress;
        Queue<ServerProcess> completed;

        int currentItems;
        int capacity;

        DoubleRandomProcess[] randomTimes;
        string name;

        public MultiServer(DoubleRandomProcess[] randomTimes, String name, SimClock sClock) : base(name, sClock)
        {
            idleProccesses = new Queue<ServerProcess>(randomTimes.Length);
            workInProgress = new List<ServerProcess>(randomTimes.Length);
            completed = new Queue<ServerProcess>(randomTimes.Length);

            this.randomTimes = randomTimes;
            this.name = name;

            this.capacity = randomTimes.Length;
        }

        public override void Start()
        {
            ServerProcess theServer;
            idleProccesses.Clear();
            workInProgress.Clear();
            completed.Clear();

            for (int i = 0; i < capacity; i++)
            {
                theServer = new ServerProcess(this, randomTimes[i], 1);
                idleProccesses.Enqueue(theServer);
            }

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

        string WorkStation.GetName()
        {
            return name;
        }

        public override bool Unblock()
        {
            if (completed.Count > 0)
            {
                ServerProcess theProcess;
                Item theItem;

                theProcess = completed.Peek();
                theItem = theProcess.theItem;

                if (GetOutput().SendItem(theItem, this))
                {
                    completed.Dequeue();
                    idleProccesses.Enqueue(theProcess);
                    currentItems--;
                    vElement.ReportState("Exit");

                    GetInput().NotifyAvaliable(this);
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
            double delay;
            if (currentItems >= capacity)
            {
                return false;
            }
            else
            {
                ServerProcess theProcess;

                currentItems++;

                theProcess = idleProccesses.Dequeue();
                theProcess.theItem = theItem;
                theProcess.loadTime = simClock.GetSimulationTime();

                vElement.LoadItem(theItem);

                delay = theProcess.GetDelay();
                theProcess.lastDelay = delay;
                workInProgress.Add(theProcess);

                simClock.ScheduleEvent(theProcess, delay);

                return true;
            }
        }
        void WorkStation.CompleteServerProcess(ServerProcess theProcess)
        {
            Item theItem = theProcess.theItem;

            workInProgress.Remove(theProcess);

            if (GetOutput().SendItem(theItem, this))
            {
                idleProccesses.Enqueue(theProcess);
                currentItems--;
                vElement.ReportState("Exit");
                GetInput().NotifyAvaliable(this);

            }
            else
            {
                completed.Enqueue(theProcess);
            }
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return !(currentItems >= capacity);
        }

        public Queue<Item> GetItems()
        {
            Queue<Item> myItems = new Queue<Item>();
            foreach (ServerProcess sp in workInProgress)
            {
                myItems.Enqueue(sp.GetCurrentItem());
            }

            foreach (ServerProcess sp in completed)
            {
                myItems.Enqueue(sp.GetCurrentItem());
            }

            return myItems;

        }

        public void SetCapacity(int capacity)
        {
            this.capacity = capacity;
        }

    }
}
