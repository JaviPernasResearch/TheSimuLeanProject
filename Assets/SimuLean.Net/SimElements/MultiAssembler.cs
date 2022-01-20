using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimuLean
{
    public class MultiAssembler : Element, WorkStation, ArrivalListener
    {
        Queue<ServerProcess> idleProccesses;
        public List<ServerProcess> workInProgress;
        Queue<ServerProcess> completed;

        bool batchMode;
        int completedItems;
        int capacity;

        ConstrainedInput[] inputs;
        int[] requirements;

        DoubleRandomProcess delay;
        string name;

        bool receivingItems = false;

        public MultiAssembler(int capacity, int[] requirements, DoubleRandomProcess delay, String name, SimClock sClock, bool batchMode = false) : base(name, sClock)
        {
            idleProccesses = new Queue<ServerProcess>(capacity);
            workInProgress = new List<ServerProcess>(capacity);
            completed = new Queue<ServerProcess>(capacity);

            this.requirements = requirements;

            this.delay = delay;
            this.name = name;
            this.batchMode = batchMode;

            this.capacity = capacity;

            inputs = new ConstrainedInput[requirements.Length];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = new ConstrainedInput(requirements[i], this, i, this.name + ".Input" + i, simClock);
            }

            SimCosts.AddCost(SimCosts.assemblerCapacityCost * capacity);
        }

        public override void Start()
        {
            ServerProcess theServer;

            idleProccesses.Clear();
            workInProgress.Clear();
            completed.Clear();

            for (int i = 0; i < capacity; i++)
            {
                theServer = new ServerProcess(this, delay, 1);
                idleProccesses.Enqueue(theServer);
            }


            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].Start();
            }

            completedItems = 0;
        }

        public ConstrainedInput GetInput(int i)
        {
            return inputs[i];
        }

        public int GetInputsCount()
        {
            return inputs.Length;
        }

        override public int GetQueueLength()
        {
            int q = 0;

            foreach (ConstrainedInput ci in inputs)
            {
                q += ci.GetQueueLength();
            }

            return workInProgress.Count + completed.Count + q;
        }
        override public int GetFreeCapacity()
        {
            return capacity;
        }

        public int GetCompletedItems()
        {
            return completedItems;
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

                if (GetOutput().sendItem(theItem, this))
                {

                    completed.Dequeue();

                    idleProccesses.Enqueue(theProcess);

                    vElement.ReportState("Exit");

                    CheckRequirements();

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
            return true;
        }

        void ArrivalListener.ItemReceived(Item theItem, int source)
        {
            if (!receivingItems)
            {
                CheckRequirements();
            }
        }

        VElement ArrivalListener.GetVElement()
        {
            return vElement;
        }

        void CheckRequirements()
        {
            ServerProcess theProcess;
            bool ready = true;
            Item newItem;
            Queue<Item> items;
            double thisDelay;

            if (idleProccesses.Count > 0)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    if (inputs[i].GetQueueLength() < requirements[i])
                    {
                        ready = false;
                    }
                }

                if (ready)
                {
                    completedItems++;
                    receivingItems = true;
                    newItem = CreateNewItem();

                    theProcess = idleProccesses.Dequeue();

                    for (int i = 0; i < inputs.Length; i++)
                    {
                        items = inputs[i].Release(requirements[i]);


                        foreach (Item it in items)
                        {
                            if (batchMode)
                            {
                                vElement.UnloadItem(it);
                                newItem.AddItem(it);
                            }
                            else
                            {
                                vElement.UnloadItem(it);
                            }
                        }

                        items.Clear();
                    }


                    receivingItems = false;
                    CheckRequirements();

                    theProcess.theItem = newItem;


                    theProcess.loadTime = simClock.GetSimulationTime();
                    workInProgress.Add(theProcess);

                    vElement.ReportState("Sort");

                    thisDelay = this.delay.NextValue();

                    Debug.Log("Assembler delay " + thisDelay);

                    theProcess.lastDelay = thisDelay;
                    simClock.ScheduleEvent(theProcess, thisDelay);
                    SimCosts.AddCost(SimCosts.processingCost);


                }
            }
        }

        Item CreateNewItem()
        {
            Item newItem = new Item(simClock.GetSimulationTime());
            newItem.SetId("type", 1, 1);

            newItem.vItem = vElement.GenerateItem(0);

            return newItem;
        }

        void WorkStation.CompleteServerProcess(ServerProcess theProcess)
        {
            Item theItem = theProcess.theItem;

            workInProgress.Remove(theProcess);


            Debug.Log("Process completed ");

            if (GetOutput().sendItem(theItem, this))
            {
                Debug.Log("Assembler Sent Item ");

                idleProccesses.Enqueue(theProcess);

                vElement.ReportState("Exit");

                CheckRequirements();

            }
            else
            {
                Debug.Log("Assembler blocked ");
                completed.Enqueue(theProcess);
            }
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return true;
        }


        //Getting all the current items to display them
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

