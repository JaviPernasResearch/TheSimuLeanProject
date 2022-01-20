using System;
using System.Collections.Generic;

namespace SimuLean
{
    public class CustomerSink : Element, Eventcs
    {
        static int truckCapacity = 10;

        int numberItems;
        int pendingOrders;
        int capacity;

        IntegerProvider orderQuantity;
        DoubleProvider demand;

        Queue<Item> itemsQ;

        //UI
        int totalShipments;
        int totalOrders;
        int totalTrucks;

        public CustomerSink(int capacity, String name, SimClock state, int minOrder, int maxOrder) : base(name, state)
        {
            this.capacity = capacity;

            numberItems = 0;
            orderQuantity = new IntegerUniformDistribution(minOrder, maxOrder);

            demand = new UniformDistribution(5.0, 10.0);

            itemsQ = new Queue<Item>();

            SimCosts.AddCost(SimCosts.storeCapacityCost * capacity);
        }

        public int GetNumberItems()
        {
            return numberItems;
        }

        public int GetPendingOrders()
        {
            return pendingOrders;
        }

        public override void Start()
        {
            numberItems = 0;
            pendingOrders = 0;

            totalOrders = 0;
            totalShipments = 0;
            totalTrucks = 0;

            itemsQ.Clear();

            simClock.ScheduleEvent(this, demand.ProvideValue());
        }


        void Eventcs.Execute()
        {
            int currentOrder = this.orderQuantity.ProvideValue();

            this.pendingOrders += currentOrder;
            this.totalOrders += currentOrder;

            simClock.ScheduleEvent(this, demand.ProvideValue());
        }

        public override bool Unblock()
        {
            throw new System.InvalidOperationException("The Sink cannot receive notifications."); //To change body of generated methods, choose Tools | Templates.
        }

        override public int GetQueueLength()
        {
            return itemsQ.Count;
        }
        override public int GetFreeCapacity()
        {
            return -1;
        }

        public void ShipTruck()
        {
            int q;

            q = Math.Min(itemsQ.Count, CustomerSink.truckCapacity);
            q = Math.Min(q, pendingOrders);

            if (q == 0)
            {
                return;
            }

            Item itemsContainer = itemsQ.Peek();

            for (int i = 0; i < q; i++)
            {
                itemsContainer.AddItem(itemsQ.Dequeue());
                numberItems--;

                SimCosts.AddRevenue(SimCosts.salePrice);
            }

            vElement.UnloadItem(itemsContainer);
            pendingOrders -= q;
            totalShipments += q;
            totalTrucks++;

            if (q > 0)
            {
                SimCosts.AddCost(SimCosts.shipmentCost);
            }
        }


        public override bool Receive(Item theItem)
        {
            if (itemsQ.Count < capacity)
            {
                numberItems++;
                itemsQ.Enqueue(theItem);
                vElement.LoadItem(theItem);
                return true;
            }
            else
                return false;

        }

        public override bool CheckAvaliability(Item theItem)
        {
            return !(numberItems >= capacity);
        }

        public Queue<Item> GetItems()
        {
            return itemsQ;
        }

        public void SetCapacity(int capacity)
        {
            this.capacity = capacity;
        }

        public int GetTotalShipments()
        {
            return totalShipments;
        }
        public int GetTotalTrucks()
        {
            return totalTrucks;
        }
        public int GetTotalOrders()
        {
            return totalOrders;
        }
    }
}

