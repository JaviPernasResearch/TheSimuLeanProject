using System.Collections;

namespace SimuLean
{
    /// <summary>
    /// Base class for model element.
    /// </summary>
    public abstract class Element
    {
        private Link input, output;

        protected SimClock simClock;

        public VElement vElement;

        readonly string name;

        private int type;

        static ArrayList elements;

        /// <summary>
        /// Returns list of all elements in the model.
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetElements()
        {
            if (elements == null)
            {
                elements = new ArrayList();
            }
            return elements;
        }

        /// <summary>
        /// Return number of elements in queue.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Returns input Link of connections.
        /// </summary>
        /// <returns></returns>
        public Link GetInput()
        {
            return input;
        }

        /// <summary>
        /// Sets <paramref name="input"/> link for connections.
        /// </summary>
        /// <param name="input"></param>
        public void SetInput(Link input)
        {
            this.input = input;
        }

        /// <summary>
        /// Returns output Link of connections.
        /// </summary>
        /// <returns></returns>
        public Link GetOutput()
        {
            return output;
        }


        /// <summary>
        /// Sets <paramref name="output"/> link for connections.
        /// </summary>
        /// <param name="output"></param>
        public void SetOutput(Link output)
        {
            this.output = output;
        }

        /// <summary>
        /// Returns integer type of Element.
        /// </summary>
        /// <returns></returns>
        public int GetType()
        {
            return type;
        }

        /// <summary>
        /// Sets integer <paramref name="type"/> of Element.
        /// </summary>
        /// <param name="type"></param>
        public void SetType(int type)
        {
            this.type = type;
        }

        /// <summary>
        /// Returns Element's name.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }

        /// <summary>
        /// Base method to start element's operation.
        /// </summary>
        public abstract void Start();

          /// <summary>
        /// Gets the current items in the element
        /// </summary>
        /// <returns></returns>
        public abstract int GetQueueLength();

        /// <summary>
        /// Gets the free slots for receiving items, -1 if infinite
        /// </summary>
        /// <returns></returns>
        public abstract int GetFreeCapacity();

        // Input connector methods

        /// <summary>
        /// Checks Element's availability.
        /// </summary>
        /// <param name="theItem"></param>
        /// <returns>True if current items < element's capacity</returns>
        public abstract bool CheckAvaliability(Item theItem);

        /// <summary>
        /// Base method to receive a new <paramref name="theItem"/> from input element.
        /// </summary>
        /// <param name="theItem"></param>
        /// <returns>True if reception is performed.</returns>
        public abstract bool Receive(Item theItem);


        // Output connector methods

        /// <summary>
        /// Unblocks blocked transfer if any.
        /// </summary>
        /// <returns></returns>
        public abstract bool Unblock();
    }
}
