using SimuLean;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for MultiAssembler (infinite servers) Element.
    /// </summary>
    public class UnityMultiAssembler : SElement, VElement
    {
        public int capacity = 1;


        public SElement[] myInputs;
        public int[] requirements = { 1 };

        public double meanDelay = 60.0;

        public GameObject assembledItemPrefab;

        public Transform[] itemLoadPosition;

        public Transform itemProcessPosition;

        public float separation = 0f;

        MultiAssembler myMultiAssembler;

        //Animation
        public Animator serverAnimator;

        // Use this for initialization
        void Start()
        {
            UnitySimClock.Instance.Elements.Add(this);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (serverAnimator != null && myMultiAssembler != null)
            {
                if (myMultiAssembler.GetItems().Count > 0)
                {
                    serverAnimator.SetBool("WorkInProgress", true);
                }
                else
                    serverAnimator.SetBool("WorkInProgress", false);
            }
        }

        override public void ConnectSim()
        {
            if (myInputs.Length != requirements.Length)
            {
                Debug.LogError("Distintos requerimientos y entradas");
            }

            for (int i = 0; i < myInputs.Length; i++)
            {
                SimpleLink.CreateLink(myInputs[i].GetElement(), myMultiAssembler.GetInput(i));
            }

            base.ConnectSim();
        }

        override public void InitializeSim()
        {
            myMultiAssembler = new MultiAssembler(capacity, requirements, new PoissonProcess(meanDelay), name, UnitySimClock.Instance.clock);

            myMultiAssembler.vElement = this;
        }

        override public void StartSim()
        {
            myMultiAssembler.Start();
        }

        override public Element GetElement()
        {
            return myMultiAssembler;
        }

        void VElement.ReportState(string msg)
        {
            GameObject gItem;
            Queue<Item> items = myMultiAssembler.GetItems();
            int i = 0;

            foreach (Item it in items)
            {
                gItem = (GameObject)it.vItem;
                if (gItem != null)
                {
                    gItem.transform.position = itemProcessPosition.position + new Vector3(0f, separation * i, 0f);

                }

                i++;
            }
        }

        object VElement.GenerateItem(int type)
        {

            GameObject vItem;
            if (assembledItemPrefab == null)
            {
                return null;
            }
            else
            {
                vItem = Instantiate(assembledItemPrefab) as GameObject;
                vItem.SetActive(true);

                vItem.transform.position = gameObject.transform.position + new Vector3(0f, 0f, 0f);
                vItem.gameObject.name = "Item" + myMultiAssembler.GetCompletedItems();
                return vItem;
            }
        }

        void VElement.LoadItem(Item vItem)
        {
            GameObject gItem;

            gItem = (GameObject)vItem.vItem;

            if (itemLoadPosition.GetLength(0) == myInputs.GetLength(0) && gItem != null)
                gItem.transform.position = itemLoadPosition[vItem.GetConstrainedInput()].position;
            else
                Debug.LogError("Item positions have not been correctly configured");
        }

        void VElement.UnloadItem(Item vItem)
        {
            Destroy((GameObject)vItem.vItem);
        }

        public override void RestartSim()
        {
            Queue<Item> items = myMultiAssembler.GetItems();
            int i = 0;

            foreach (Item it in items)
            {
                Destroy((GameObject)it.vItem);
                i++;
            }

            for (int j = 0; j < myMultiAssembler.GetInputsCount(); j++)
            {
                items = myMultiAssembler.GetInput(j).GetItems();
                i = 0;

                foreach (Item it in items)
                {
                    Destroy((GameObject)it.vItem);
                    i++;
                }
            }

            StartSim();
        }
   }
}