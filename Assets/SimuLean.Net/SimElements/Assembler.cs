using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/* @author Javi Pernas  */

namespace SimuLean
{
    public class Assembler : Element, WorkStation
    {
        ServerProcess theServer;
        DoubleRandomProcess randomTimes;

        Item theItem;

        int requirements;
        int itemsCompleted;
        int[] serverId;

        protected Dictionary<int, Item[]> matchedItems;
        string name;


        public Assembler(string name, SimClock sClock, DoubleRandomProcess randomTimes, int requirements) : base(name, sClock)
        {
            this.randomTimes = randomTimes;
            this.name = name;
            this.requirements = requirements;
        }

        public override void Start()
        {

            theServer = new ServerProcess(this, randomTimes, requirements);

            itemsCompleted = 0;

        }

        override public int GetQueueLength()
        {
            return theServer.GetQueueLength();
        }
        override public int GetFreeCapacity()
        {
            return requirements - theServer.GetQueueLength();
        }

        string WorkStation.GetName()
        {
            return name;
        }

        public override bool Unblock()
        {
            if (theServer.state == 2)
            {
                if (GetOutput().sendItem(theServer.theItem, this))
                {

                    this.SetType(0);
                    theServer.state = 0;
                    theServer.theItem = null;
                    theServer.ClearList();
                    GetInput().notifyAvaliable(this);
                    theItem = null;

                    itemsCompleted++;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public override bool Receive(Item theItem)
        {
            double delay;
            bool notFound = true;

            if (theServer.state == 0)
            {
                if (this.GetType().Equals(theItem.GetId()))
                {
                    notFound = !notFound;
                }
                else if (this.GetType() == 0)
                {
                    this.SetType(theItem.GetId());
                    notFound = !notFound;
                }
            }
            if (notFound == true)
            {
                return false;
            }
            else
            {
                this.theItem = theItem;
                theServer.AddItem(theItem);
                vElement.LoadItem(theItem);

                if (theServer.GetQueueLength() == requirements)
                {
                    theServer.state = 1;
                    theServer.theItem = theItem;

                    delay = this.randomTimes.NextValue();
                    theServer.lastDelay = delay; //For SimpleTransporter
                    simClock.ScheduleEvent(theServer, delay);
                }
                else
                    this.GetInput().notifyAvaliable(this);

                return true;
            }
        }

        void WorkStation.CompleteServerProcess(ServerProcess theProcess)
        {

            if (theServer.state == 1)
            {
                Item theItem;
                ArrayList itemsStoraged = theServer.GetItems();

                theItem = (Item)itemsStoraged[0];
                theServer.loadTime = Time.time; // SimpleTransporter
                theItem.vItem = vElement.GenerateItem(theItem.GetId());

                for (int i = 0; i < itemsStoraged.Count; i++)  //Saving items setting the first one as container (previously ordered)
                {
                    theItem.AddItem((Item)itemsStoraged[i]);
                }

                if (GetOutput().sendItem(theItem, this))
                {

                    this.SetType(0);
                    theServer.state = 0;
                    theServer.theItem = null;
                    theServer.ClearList();
                    theItem = null;

                    itemsCompleted++;

                    GetInput().notifyAvaliable(this);
                }
                else
                {
                    theServer.state = 2;
                    theServer.theItem = theItem;
                    theServer.ClearList();
                }
            }
            else if (!this.GetInput().notifyAvaliable(this))
            {
                simClock.ScheduleEvent(theServer, 2);
            }
        }

        public override bool CheckAvaliability(Item theItem)
        {
            if (this.GetType() == theItem.GetId() && theServer.state == 0)
                return true;
            else if (this.GetType() == 0)
                return true;
            else
                return false;
        }

        public Item GetItem()
        {
            return theItem;
        }

        //Restart
        public void SetCapacity(int capacity)
        {
            this.requirements = capacity;
        }
    }
}
