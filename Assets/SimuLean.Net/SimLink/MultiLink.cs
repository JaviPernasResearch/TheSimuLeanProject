using System.Collections.Generic;

namespace SimuLean
{
    /// <summary>
    /// Class that simulates a multiple connection between elements.
    /// </summary>
    public class MultiLink : Link
    {
        List<Element> origins = new List<Element>();
        List<Element> destinations = new List<Element>();

        List<Element> blockedTransfers = new List<Element>();

        Link thislink;

        /// <summary>
        /// Defines routing strategy:
        /// <list>
        /// <item>0 - First Available</item>
        /// <item>1 - Shortest Queue</item>
        ///</list>
        /// </summary>
        int mode = 0;

        public MultiLink(int mode)
        {
            thislink = this;
            this.mode = mode;
        }

        /// <summary>
        /// Adds <paramref name="theInput"/>  to multilink origins and sets multilink as output of <paramref name="theInput"/>.
        /// </summary>
        /// <param name="theInput"></param>
        public void ConnectInput(Element theInput)
        {
            origins.Add(theInput);
            theInput.SetOutput(this);
        }

        /// <summary>
        /// Adds <paramref name="theOutput"/> element to multilink destinations and sets multilink as input of <paramref name="theOutput"/>.
        /// </summary>
        /// <param name="theOutput"></param>
        public void ConnectOutput(Element theOutput)
        {
            destinations.Add(theOutput);
            theOutput.SetInput(this);
        }

        /// <summary>
        /// Resets multilink inputs and outputs.
        /// </summary>
        public void Reset()
        {
            origins.Clear();
            destinations.Clear();
            blockedTransfers.Clear();
        }

        public void Start()
        {
            blockedTransfers.Clear();
        }

        bool Link.SendItem(Item theItem, Element source)
        {
            int iDestiny = SelectOutput(theItem);
            if (iDestiny < 0)
            {
                blockedTransfers.Add(source);
                return false;
            }
            else if (destinations[iDestiny].Receive(theItem))
            {
                return true;
            }
            else
            {
                blockedTransfers.Add(source);
                return false;
            }
        }

        bool Link.NotifyAvaliable(Element source)
        {
            foreach (Element orig in blockedTransfers)
            {
                blockedTransfers.Remove(orig);
                if (orig.Unblock())
                {
                    return true;
                }
                else
                {
                    blockedTransfers.Add(orig);
                }
            }
            foreach (Element input in origins)
            {
                if (input.Unblock())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns port number according to MultiLink routing strategy (mode).
        /// </summary>
        /// <param name="theItem"></param>
        /// <returns></returns>
        int SelectOutput(Item theItem)
        {
            switch (mode)
            {
                case 0:
                    return FirstAvaliable(theItem);
                default:
                    return FirstAvaliable(theItem);
            }

        }

        /// <summary>
        /// Returns element by FIFO dispatch rule.
        /// </summary>
        /// <param name="theItem"></param>
        /// <returns></returns>
        int FirstAvaliable(Item theItem)
        {
            int i = 0;
            foreach (Element output in destinations)
            {
                if (output.CheckAvaliability(theItem))
                    return i;
                else
                    i++;
            }
            return -1;
        }

        //Template for new routing strategies
        //Set up and add to "SelectOutput" switch.
        //int MethodName(Item theItem)
        //{
        //    Decision Strategy.
        //      return port number;
        //    else
        //      return -1;
        //}
    }
}