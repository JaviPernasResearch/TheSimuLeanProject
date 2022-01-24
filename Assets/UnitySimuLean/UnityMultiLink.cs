using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for MultiLink Element.
    /// </summary>
    public class UnityMultiLink : MonoBehaviour
    {

        public List<SElement> inputs;
        public List<SElement> outputs;
        public int mode = 0;

        MultiLink mLink;

        void Start()
        {
            UnitySimClock.instance.mLinks.Add(this);
        }

        /// <summary>
        /// Sets inputs and outputs on MultiLink connections.
        /// </summary>
        public void ConnectSim()
        {
            mLink = new MultiLink(mode);
            foreach (SElement sElem in inputs)
            {
                mLink.ConnectInput(sElem.GetElement());
            }
            foreach (SElement sElem in outputs)
            {
                mLink.ConnectOutput(sElem.GetElement());
            }
        }
    }
}