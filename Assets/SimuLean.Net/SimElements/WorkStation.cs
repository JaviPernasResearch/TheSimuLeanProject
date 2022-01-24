namespace SimuLean
{
    /// <summary>
    /// Default itnerface for every workstation type.
    /// </summary>
    public interface WorkStation
    {
        /// <summary>
        /// Completes pending process and tries to send the item. If not possible, server remains blocked.
        /// </summary>
        /// <param name="theProcess"></param>
        void CompleteServerProcess(ServerProcess theProcess);
        string GetName();
    }
}
