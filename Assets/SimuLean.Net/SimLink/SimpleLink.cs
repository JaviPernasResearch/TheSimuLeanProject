namespace SimuLean
{
    public class SimpleLink : Link
    {
        Element origin;
        Element destination;

        Link thislink;

        static public void createLink(Element origin, Element destination)
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

        bool Link.sendItem(Item theItem, Element source)
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

        bool Link.notifyAvaliable(Element source)
        {
            return origin.Unblock();
        }
    }
}
