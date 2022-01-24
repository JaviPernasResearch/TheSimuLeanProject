namespace SimuLean
{
    /// <summary>
    /// Base class for links between elements.
    /// </summary>
    public interface Link
    {
        /// <summary>
        /// Sends <paramref name="theItem"/> from <paramref name="source"/>  to available output as to multilink mode.
        /// </summary>
        /// <param name="theItem"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        bool SendItem(Item theItem, Element source);

        /// <summary>
        /// Notifies the <paramref name="source"/> that there is one output available.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        bool NotifyAvaliable(Element source);
    }
}
