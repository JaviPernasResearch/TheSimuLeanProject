using SimuLean;
using UnityEngine;

namespace UnitySimuLean
{
    public abstract class SElement : MonoBehaviour
    {

        public SElement nextElement;

        public UnityMultiLink myPreviousLink;
        public UnityMultiLink myNextLink;

        public abstract void InitializeSim();
        public abstract void StartSim();

        public abstract void RestartSim();

        public virtual void ConnectSim()
        {
            if (nextElement != null)
                SimpleLink.createLink(GetElement(), nextElement.GetElement());

            if (myPreviousLink != null)
                myPreviousLink.outputs.Add(this);
            if (myNextLink != null)
                myNextLink.inputs.Add(this);
        }

        public abstract Element GetElement();

        //UI
        public abstract string GetReport();
    }
}

