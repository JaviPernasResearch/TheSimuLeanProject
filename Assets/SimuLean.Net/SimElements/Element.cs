using System.Collections;

namespace SimuLean
{
    public abstract class Element
    {

        private Link input, output;

        protected SimClock simClock;

        public VElement vElement;

        readonly string name;

        private int type;
        //0:no type, no item in process

        static ArrayList elements;

        public static ArrayList GetElements()
        {
            if (elements == null)
            {
                elements = new ArrayList();
            }
            return elements;
        }

        public static int GetInventory()
        {
            ArrayList elems = GetElements();
            int inv = 0;

            foreach (Element e in elements)
            {
                inv += e.GetQueueLength();
            }

            return inv;
        }


        public Element(string name, SimClock simClock)
        {
            this.name = name;
            this.simClock = simClock;

            GetElements().Add(this);

        }

        public Link GetInput()
        {
            return input;
        }

        public void SetInput(Link input)
        {
            this.input = input;
        }

        public Link GetOutput()
        {
            return output;
        }

        public void SetOutput(Link output)
        {
            this.output = output;
        }

        public int GetType()
        {
            return type;
        }

        public void SetType(int type)
        {
            this.type = type;
        }

        public string GetName()
        {
            return name;
        }
        public abstract void Start();

        // Get the current items in the element

        public abstract int GetQueueLength();

        // Get the free slots for receiving items, -1 if infinite

        public abstract int GetFreeCapacity();

        // Input connector methods

        public abstract bool CheckAvaliability(Item theItem);

        public abstract bool Receive(Item theItem);


        // Output connector methods

        public abstract bool Unblock();
    }
}
