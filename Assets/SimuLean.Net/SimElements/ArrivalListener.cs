namespace SimuLean
{
    /// <summary>
    /// Interface to model internal queues of elements.
    /// </summary>
    public interface ArrivalListener
    {
        /// <summary>
        /// Notifies main element a new item has been received.
        /// </summary>
        /// <param name="theItem"></param>
        /// <param name="source"></param>
        void ItemReceived(Item theItem, int source);

        VElement GetVElement();
    }
}

