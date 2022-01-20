namespace SimuLean
{
    public class InfiniteSource : Element, Eventcs
    {
        Item lastItem;

        int numberIterms;

        public InfiniteSource(string name, SimClock state) : base(name, state)
        {
        }

        public override void Start()
        {
            numberIterms = 0;

            simClock.ScheduleEvent(this, 0.0);
        }

        public override bool Unblock()
        {

            if (this.GetOutput().sendItem(lastItem, this))
            {
                lastItem = CreateItem();
                numberIterms++;
                return true;
            }

            else
                return false;
        }

        public int GetNumberItems()
        {
            return numberIterms;
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
            int i = 0;
            do
            {
                lastItem = CreateItem();
                numberIterms++;
            }
            while (this.GetOutput().sendItem(lastItem, this));
        }

        Item CreateItem()
        {
            Item nItem = new Item(simClock.GetSimulationTime());
            nItem.SetId("type", 1, 1);
            nItem.vItem = vElement.GenerateItem(nItem.GetId());

            return nItem;
        }

        public override bool CheckAvaliability(Item theItem)
        {
            return false;
        }
    }
}
