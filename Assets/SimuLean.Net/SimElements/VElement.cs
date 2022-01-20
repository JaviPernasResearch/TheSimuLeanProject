namespace SimuLean
{
    public interface VElement
    {

        void ReportState(string msg);

        object GenerateItem(int type);

        void LoadItem(Item vItem);

        void UnloadItem(Item vItem);
    }
}

