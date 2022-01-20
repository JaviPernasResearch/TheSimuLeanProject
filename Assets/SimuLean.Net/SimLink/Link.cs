namespace SimuLean
{
    public interface Link
    {
        bool sendItem(Item theItem, Element source);

        bool notifyAvaliable(Element source);
    }
}
