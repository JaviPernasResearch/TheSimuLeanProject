using System.Collections.Generic;

namespace SimuLean
{
    /// <summary>
    /// Base class for simulation items.
    /// </summary>
    public class Item
    {
        static int ITEM_NUMBER = 0;

        double creationTime;

        private string type;
        private int myId;
        private int myConstrainedInput;

        public int priority;

        public object vItem;

        public List<Item> subItems;

        protected Dictionary<string, double> attribDouble;


        public Item(double creationTime)
        {
            Item.ITEM_NUMBER++;

            this.creationTime = creationTime;
        }

        public void SetId(string type, int myId, int priority)
        {

            this.type = type;
            this.myId = myId;
            this.priority = priority;
        }

        public int GetId()
        {
            return myId;
        }

        public void SetcreationTime(double creationTime)
        {
            this.creationTime = creationTime;
        }

        public double GetCreationTime()
        {
            return creationTime;
        }

        public void SetConstrainedInput(int myConstrainedInput)
        {
            this.myConstrainedInput = myConstrainedInput;
        }

        public int GetConstrainedInput()
        {
            return myConstrainedInput;
        }

        public void AddItem(Item theItem)
        {
            if (subItems == null)
            {
                subItems = new List<Item>();
            }
            subItems.Add(theItem);
        }

        public List<Item> GetSubItems()
        {
            return subItems;
        }
    }
}
