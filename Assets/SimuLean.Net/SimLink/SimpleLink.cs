namespace SimuLean
{
    /// <summary>
    /// Class that simulates 1 to 1 links (simple links) between elements.
    /// </summary>
    public class SimpleLink : Link
    {
        Element origin;
        Element destination;

        Link thislink;

        static public void CreateLink(Element origin, Element destination)
        {
            SimpleLink theLink = new SimpleLink(origin, destination);

            origin.SetOutput(theLink);
            destination.SetInput(theLink);
        }

        public SimpleLink(Element origin, Element destination)
        {
            thislink = this;
            this.origin = origin;
            this.destination = destination;

        }

        bool Link.SendItem(Item theItem, Element source)
        {
            if (destination.Receive(theItem))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        bool Link.NotifyAvaliable(Element source)
        {
            return origin.Unblock();
        }
    }
}
