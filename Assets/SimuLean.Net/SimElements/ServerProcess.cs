using System.Collections;

namespace SimuLean
{
    public class ServerProcess : Eventcs
    {
        WorkStation myServer;

        public Item theItem;

        public double loadTime = 0.0;
        public double lastDelay = 0.0;

        int capacity;

        DoubleRandomProcess delay;
        ArrayList itemsOrdered;

        public int state = 0;
        //0: idle
        //1: bussy
        //2: blocked

        int typeItem;
        //0:no type

        public ServerProcess(WorkStation myServer, DoubleRandomProcess randomDelay, int capacity)
        {
            this.myServer = myServer;

            delay = randomDelay;

            state = 0;

            this.capacity = capacity;

            itemsOrdered = new ArrayList(capacity);

            typeItem = 0;
        }

        public double GetDelay()
        {
            double delay = this.delay.NextValue();

            return delay;
        }

        void Eventcs.Execute()
        {
            myServer.CompleteServerProcess(this);
        }

        public void AddItem(Item theItem)
        {

            if (typeItem == 0)
            {
                itemsOrdered.Clear();
                typeItem = theItem.GetId();
            }

            itemsOrdered.Add(theItem);

            if (itemsOrdered.Count > 1)
            {
                for (int i = itemsOrdered.Count - 2; i >= 0; i--)
                {
                    Item oneItem = (Item)itemsOrdered[i];
                    if (theItem.priority < oneItem.priority)
                    {
                        Swap(i);
                    }
                    else
                        i = -1;
                }
            }
        }

        private void Swap(int i)
        {
            Item intermItem = (Item)itemsOrdered[i];
            itemsOrdered[i] = itemsOrdered[i + 1];
            itemsOrdered[i + 1] = intermItem;
        }

        public ArrayList GetItems()
        {
            typeItem = 0;

            return itemsOrdered;
        }

        public Item GetCurrentItem()
        {
            return theItem;
        }

        public int GetQueueLength()
        {
            return itemsOrdered.Count;
        }

        public int GetTypeProcess()
        {
            return typeItem;
        }

        public void ClearList()
        {
            itemsOrdered.Clear();
        }
    }
}
