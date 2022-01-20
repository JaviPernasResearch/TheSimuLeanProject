namespace SimuLean
{
    public interface ArrivalListener
    {
        void ItemReceived(Item theItem, int source);

        VElement GetVElement();
    }
}

