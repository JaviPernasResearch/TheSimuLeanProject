using SimuLean;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for InfiniteSource Element.
    /// </summary>
    public class UnityInfinitySource : SElement, VElement
    {

        InfiniteSource theSource;

        public GameObject itemPrefab;

        public float separation = 1f;

        public Transform itemPosition;

        override public void InitializeSim()
        {
            theSource = new InfiniteSource(name, UnitySimClock.Instance.clock);
            theSource.vElement = this;

        }
        override public void StartSim()
        {
            theSource.Start();
        }

        void Start()
        {
            UnitySimClock.Instance.Elements.Add(this);
        }

        override public Element GetElement()
        {
            return theSource;
        }

        void VElement.ReportState(string msg)
        {
        }

        object VElement.GenerateItem(int type)
        {
            GameObject newItem = Instantiate(itemPrefab) as GameObject;

            newItem.SetActive(true);

            newItem.transform.position = itemPosition.position + new Vector3(0f, separation, 0f); ;
            newItem.gameObject.name = newItem.gameObject.name + theSource.GetNumberItems();
            return newItem;
        }

        void VElement.LoadItem(Item vItem)
        {
            GameObject gItem;

            gItem = (GameObject)vItem.vItem;

            gItem.transform.position = itemPosition.position;

        }

        void VElement.UnloadItem(Item vItem)
        {
        }

        public override void RestartSim()
        {
            StartSim();
        }
    }
}