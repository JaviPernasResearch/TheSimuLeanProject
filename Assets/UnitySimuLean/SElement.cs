using SimuLean;
using UnityEngine;

namespace UnitySimuLean
{
    public abstract class SElement : MonoBehaviour
    {

        public SElement nextElement;

        public UnityMultiLink myPreviousLink;
        public UnityMultiLink myNextLink;

        /// <summary>
        /// Initializes element. Instantiation of SimuLean Element and assgination of VElement must be done here.
        /// It is called after Unity Start() method and before StartSim() method.
        /// </summary>
        public abstract void InitializeSim();
        /// <summary>
        /// Base method for Element.Start() method (requires previous initialization).
        /// </summary>
        public abstract void StartSim();

        /// <summary>
        /// Base method to reset and start the element
        /// It is called after StartSim() method.
        /// </summary>
        public abstract void RestartSim();

        /// <summary>
        /// Default base method to initialize "simple link" connections.
        /// </summary>
        public virtual void ConnectSim()
        {
            if (nextElement != null)
                SimpleLink.CreateLink(GetElement(), nextElement.GetElement());

            if (myPreviousLink != null)
                myPreviousLink.outputs.Add(this);
            if (myNextLink != null)
                myNextLink.inputs.Add(this);
        }

        /// <summary>
        /// Returns the Element element
        /// </summary>
        /// <returns></returns>
        public abstract Element GetElement();
    }
}

