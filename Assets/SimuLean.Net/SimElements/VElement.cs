namespace SimuLean
{
    /// <summary>
    /// Default interface that couples UnitySimuLean elemnt classes (Unity Components) with corresponding SimuLean Element classes. Every Element must implement VElement.
    /// </summary>
    public interface VElement
    {

        /// <summary>
        /// Sends <paramref name="msg"/> to VElement.
        /// </summary>
        /// <param name="msg"></param>
        void ReportState(string msg);

        /// <summary>
        /// Base method to generate gameobject representing item. Use <paramref name="type"/> if necessary to specify item type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        object GenerateItem(int type);

        /// <summary>
        /// Base method notify item <paramref name="vItem"/> has been loaded onto VElement.
        /// </summary>
        /// <param name="vItem"></param>
        void LoadItem(Item vItem);

        /// <summary>
        /// Base method notify item <paramref name="vItem"/> has been unloaded from VElement. 
        /// </summary>
        /// <param name="vItem"></param>
        void UnloadItem(Item vItem);
    }
}

