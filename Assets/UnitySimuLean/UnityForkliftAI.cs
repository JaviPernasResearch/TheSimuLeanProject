using SimuLean;
using System.Collections.Generic;
using UnityEngine;

namespace UnitySimuLean
{
    //public class UnityForkliftAI : SElement, VElement
    //{

    //    private List<GameObject> carryingItem;

    //    private bool readyToLoad = false;
    //    private bool readyToUnload = false;

    //    public int capacity = 0;
    //    public float loadTime = 3.0f;
    //    public float height = 3f;

    //    public Transform origin, destination;
    //    public Transform itemCarriedPosition;
    //    public GameObject platform;
    //    public Transform initialPositionPlatform, finalPositionPlatform;
    //    private Vector3 platformVector;
    //    private float timeToMovePlatform = 0;

    //    private AIDestinationSetter destinationController;
    //    private AIPath forkliftController;
    //    public Forklift theForklift;

    //    void Start()
    //    {
    //        UnitySimClock.instance.elements.Add(this);
    //    }

    //    override public void InitializeSim()
    //    {
    //        theForklift = new Forklift(name, UnitySimClock.instance.clock, capacity);

    //        theForklift.vElement = this;

    //        destinationController = GetComponent<AIDestinationSetter>();
    //        forkliftController = GetComponent<AIPath>();
    //        destinationController.target = transform;

    //        platform.transform.position = initialPositionPlatform.position;
    //        platformVector = finalPositionPlatform.position - initialPositionPlatform.position;

    //        carryingItem = new List<GameObject>(capacity);
    //    }

    //    override public void StartSim()
    //    {
    //        theForklift.start();
    //    }

    //    void Update()
    //    {
    //        if (destinationController != null)
    //        {
    //            float distance = Vector3.Distance(transform.position, destinationController.target.position);

    //            if (forkliftController.reachedEndOfPath && distance < 5f && destinationController.target == origin && readyToLoad)
    //            {
    //                if (timeToMovePlatform == 0)
    //                {
    //                    timeToMovePlatform = Time.time;
    //                }

    //                float p;
    //                p = (Time.time - timeToMovePlatform) / loadTime;
    //                platform.transform.position = initialPositionPlatform.position + platformVector * p;

    //                if (p >= 1)
    //                {
    //                    theForklift.atPickPoint = true;
    //                    theForklift.PickItem();
    //                    readyToLoad = false;
    //                }

    //            }
    //            else if (forkliftController.reachedEndOfPath && distance < 5f && destinationController.target == destination && readyToUnload)
    //            {
    //                if (timeToMovePlatform == 0)
    //                {
    //                    timeToMovePlatform = Time.time;
    //                }

    //                float p;
    //                p = (Time.time - timeToMovePlatform) / loadTime;
    //                platform.transform.position = finalPositionPlatform.position + platformVector * -p;

    //                if (p >= 1)
    //                {
    //                    theForklift.readyToLeave = true;
    //                    theForklift.LeaveItem();
    //                    readyToUnload = false;
    //                    carryingItem.Clear();
    //                }
    //            }
    //        }

    //        if (carryingItem != null)
    //        {
    //            int i = 0;
    //            foreach (GameObject it in carryingItem)
    //            {
    //                if (it != null)
    //                {
    //                    it.transform.position = itemCarriedPosition.transform.position + Vector3.up * i * height;
    //                    i++;
    //                }
    //            }
    //        }
    //    }

    //    override public Element GetElement()
    //    {
    //        return theForklift;
    //    }

    //    void VElement.reportState(string msg)
    //    {
    //        int i = 0;
    //        GameObject myItem;
    //        foreach (Item it in theForklift.GetItems())
    //        {
    //            if (it != null)
    //            {
    //                myItem = (GameObject)it.vItem;
    //                myItem.transform.position = itemCarriedPosition.transform.position + Vector3.up * i * height;
    //                i++;
    //            }
    //        }
    //    }

    //    object VElement.generateItem(int type)
    //    {
    //        return null;
    //    }

    //    void VElement.loadItem(Item vItem)
    //    {
    //        if (destinationController.target != origin && theForklift.checkAvaliability(null))
    //        {
    //            timeToMovePlatform = 0;
    //            readyToLoad = true;
    //            destinationController.target = origin;
    //        }
    //        else if (theForklift.atPickPoint)
    //        {
    //            carryingItem.Add((GameObject)vItem.vItem);
    //        }
    //    }

    //    void VElement.unloadItem(Item vItem)
    //    {
    //        if (theForklift.atPickPoint == true)
    //        {
    //            theForklift.atPickPoint = false;

    //            timeToMovePlatform = 0;
    //            readyToUnload = true;
    //            destinationController.target = destination;
    //        }
    //    }

    //    public override void RestartSim()
    //    {
    //        StartSim();
    //    }

    //    //UI
    //    public override string GetReport()
    //    {
    //        return null;
    //    }
    //}
}