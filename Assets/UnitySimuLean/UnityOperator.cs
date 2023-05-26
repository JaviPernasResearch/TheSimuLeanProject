using SimuLean;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace UnitySimuLean
{
    /// <summary>
    /// Unity Component for Operator Element. UnityOperator uses Unity NavMesh and Animator to perform laoding and unloading animations.
    /// </summary>
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnityOperator : SElement, VElement
    {
        public Animator anim;
        NavMeshAgent agent;
        Operator theOperator;

        GameObject carryingItem;

        public bool wayBack;
        bool start;
        bool isOff;


        public int capacity = 0;

        int station;
        public float loadTime = 3.0f;

        public Transform origin, destination, wayBackStation;
        public Transform itemCarriedPosition;

        void Start()
        {
            UnitySimClock.Instance.Elements.Add(this);
        }

        override public void InitializeSim()
        {
            isOff = true;
            start = false;
            station = 0;
            agent = GetComponent<NavMeshAgent>();

            agent.isStopped = true;

            theOperator = new Operator(name, UnitySimClock.Instance.clock, capacity);

            theOperator.vElement = this;
        }

        override public void StartSim()
        {
            theOperator.Start();
        }

        void Update()
        {
            if (start && isOff)
            {
                StartCoroutine(CompleteRoute(origin));
            }

            if (carryingItem != null)
            {
                carryingItem.transform.position = itemCarriedPosition.position;
            }
        }

        public IEnumerator CompleteRoute(Transform newPosition)
        {
            start = false;
            isOff = false;

            anim.SetBool("load", false);
            anim.SetBool("idle", false);

            agent.SetDestination(newPosition.position);
            yield return new WaitForSeconds(0.1f); //Necessary to calculate the route (IA)

            agent.isStopped = false;
            anim.SetBool("move", true);

            do
            {
                yield return null;

            } while (agent.remainingDistance > anim.speed * loadTime * 1.5);

            agent.isStopped = true;
            anim.SetBool("move", false);

            if (station == 0)
            {
                anim.CrossFade("LoadObject", loadTime);
            }
            else if (station == 1)
            {
                anim.CrossFade("UnloadObject", loadTime);
            }
            do
            {
                yield return null;

            } while (anim.IsInTransition(0));

            station++;

            if (station == 1)
            {
                theOperator.atPickPoint = true;
                theOperator.PickItem(); //Once the operator is at pick point, we pick the item(s)

                do
                {
                    yield return null;

                } while (theOperator.atPickPoint == true);

                StartCoroutine(CompleteRoute(destination));

                yield break;
            }
            else
            {
                if (start != true && (!wayBack || station != 2)) //No more items to transport and no wayback
                {
                    anim.SetBool("idle", true);
                }

                theOperator.LeaveItem();

                carryingItem = null;

                if (station == 2 && wayBack)
                {
                    StartCoroutine(CompleteRoute(wayBackStation));
                    yield break;
                }
            }
            station = 0;

            isOff = true;

            start = false;

            yield return null;
        }

        override public Element GetElement()
        {
            return theOperator;
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
                start = true;
            }
        }

        void VElement.UnloadItem(Item vItem)
        {
            if (theOperator.atPickPoint == true)
            {
                GameObject gItem;

                foreach (Item it in vItem.GetSubItems())
                {
                    gItem = (GameObject)it.vItem;
                    if (gItem != null)
                        gItem.transform.position = itemCarriedPosition.transform.position;
                }

                carryingItem = (GameObject)vItem.vItem;

                theOperator.atPickPoint = false;
            }
        }

        public override void RestartSim()
        {
            StartSim();
        }
    }
}