using SimuLean;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for Sink Element.
    /// </summary>
    public class UnitySink : SElement, VElement
    {

        Sink theSink;

        public int Input { get => theSink.GetNumberIterms();  }


        void Start()
        {
            UnitySimClock.Instance.Elements.Add(this);
        }

        override public void InitializeSim()
        {
            theSink = new Sink(name, UnitySimClock.Instance.clock);

            theSink.vElement = this;
        }
        override public void StartSim()
        {
            theSink.Start();
        }

        override public Element GetElement()
        {
            return theSink;
        }


        void VElement.ReportState(string msg)
        {
        }

        object VElement.GenerateItem(int type)
        {
            return null;
        }

        void VElement.LoadItem(Item vItem)
        {
            GameObject gItem;

            gItem = (GameObject)vItem.vItem;

            if (gItem != null)
            {
                Destroy(gItem);
            }

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