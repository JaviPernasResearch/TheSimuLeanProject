using System.Collections.Generic;

namespace SimuLean
{
    public class MultiLink : Link
    {
        List<Element> origins = new List<Element>();
        List<Element> destinations = new List<Element>();

        List<Element> blockedTransfers = new List<Element>();

        Link thislink;

        int mode = 0;
        // Modes:
        //		0 - First avaliable
        //		1 - Shortest queue

        public MultiLink(int mode)
        {
            thislink = this;
            this.mode = mode;
        }

        public void connectInput(Element theInput)
        {
            origins.Add(theInput);
            theInput.SetOutput(this);
        }

        public void connectOutput(Element theOutput)
        {
            destinations.Add(theOutput);
            theOutput.SetInput(this);
        }

        public void reset()
        {
            origins.Clear();
            destinations.Clear();
            blockedTransfers.Clear();
        }

        public void start()
        {
            blockedTransfers.Clear();
        }

        bool Link.sendItem(Item theItem, Element source)
        {
            int iDestiny = selectOutput(theItem);
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

        bool Link.notifyAvaliable(Element source)
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

        int selectOutput(Item theItem)
        {
            switch (mode)
            {
                case 0:
                    return firstAvaliable(theItem);
                default:
                    return firstAvaliable(theItem);
            }

        }

        int firstAvaliable(Item theItem)
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
    }
}