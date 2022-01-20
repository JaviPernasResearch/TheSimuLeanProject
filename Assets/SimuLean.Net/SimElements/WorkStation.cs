namespace SimuLean
{
    public interface WorkStation
    {
        void CompleteServerProcess(ServerProcess theProcess);
        string GetName();
    }
}
