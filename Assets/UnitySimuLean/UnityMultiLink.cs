using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
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

        void Update()
        {
        }

        public void ConnectSim()
        {
            mLink = new MultiLink(mode);
            foreach (SElement sElem in inputs)
            {
                mLink.connectInput(sElem.GetElement());
            }
            foreach (SElement sElem in outputs)
            {
                mLink.connectOutput(sElem.GetElement());
            }
        }
    }
}