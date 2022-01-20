using SimuLean;
using UnityEngine;
using UnityEngine.UI;

namespace UnitySimuLean
{
    public class UnityProvider : SElement, VElement
    {
        public string name = "Provider";
        public bool useSocket = false;
        public double minTime = 30.0;
        public double maxTime = 60.0;

        public GameObject itemPrefab;

        public Text orderQuantity;

        public Text pendingArrivals;

        ProviderSource theProviderSource;

        //UI
        private int totalOrders;

        void Start()
        {
            UnitySimClock.instance.elements.Add(this);
        }

        // Update is called once per frame
        void Update()
        {
            if (pendingArrivals != null && theProviderSource != null)
            {
                pendingArrivals.text = theProviderSource.GetPendingArrivals().ToString();
            }
        }
        public void Order()
        {
            int q;

            q = int.Parse(orderQuantity.text);

            totalOrders += q;
            theProviderSource.Order(q);

            SimCosts.AddCost(SimCosts.orderCost + SimCosts.purchaseCost * q);
        }

        override public void InitializeSim()
        {
            theProviderSource = new ProviderSource(useSocket, name, UnitySimClock.instance.clock, minTime, maxTime);

            theProviderSource.vElement = this;
        }
        override public void StartSim()
        {
            totalOrders = 0;

            theProviderSource.Start();
        }

        override public Element GetElement()
        {
            return theProviderSource;
        }


        void VElement.ReportState(string msg)
        {
        }

        object VElement.GenerateItem(int type)
        {
            GameObject vItem;
            if (itemPrefab == null)
            {
                return null;
            }
            else
            {
                vItem = Instantiate(itemPrefab) as GameObject;
                vItem.SetActive(true);

                vItem.transform.position = gameObject.transform.position + new Vector3(0f, 0f, 0f); ;
                vItem.gameObject.name = "Item" + theProviderSource.GetNumberItems();
                return vItem;
            }
        }

        void VElement.LoadItem(Item vItem)
        {
        }

        void VElement.UnloadItem(Item vItem)
        {
        }

        public override void RestartSim()
        {
            StartSim();
        }

        //UI
        public override string GetReport()
        {
            return GetElement().GetName() + System.Environment.NewLine + "Pedidos realizados: " + totalOrders;
        }
    }
}