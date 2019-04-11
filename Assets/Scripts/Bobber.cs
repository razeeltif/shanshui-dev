using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{

    [SerializeField]
    float waterLevel = -3.2f;
    [SerializeField]
    float waterViscosity = 10f;
    [SerializeField]
    float floatHeight = 2;
    [SerializeField]
    float bounceDamp = 0.05f;

#pragma warning disable 0649
    [SerializeField]
    Vector3 bouncyCenterOffset;

    [SerializeField]
    GameObject cable_start;
    [SerializeField]
    float cable_start_spring_when_shoe = 10f;
    [SerializeField]
    float hook_spring_when_shoe = 1000f;
    [SerializeField]
    GameObject bendRod;

#pragma warning restore 0649

    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;
    private float initialDrag;

    private float cable_start_previous_spring = 0;
    
    
    private GameObject miamPoisson;


    private void OnEnable()
    {
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    private void Start()
    {
        initialDrag = GetComponent<Rigidbody>().drag;
    }


    // Update is called once per frame
    void Update()
    {

        actionPoint = transform.position + transform.TransformDirection(bouncyCenterOffset);
        forceFactor = 1f - ((actionPoint.y - waterLevel) / floatHeight);

        if(forceFactor > 0f)
        {
            upLift = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * bounceDamp);
            GetComponent<Rigidbody>().AddForceAtPosition(upLift, actionPoint);
            GetComponent<Rigidbody>().drag = waterViscosity;
        }
        else
        {
            GetComponent<Rigidbody>().drag = initialDrag;
        }

        if (this.GetComponent<SpringJoint>())
        {
            Debug.Log(this.GetComponent<SpringJoint>().currentForce);
            bendRod.GetComponent<Rigidbody>().AddForce(this.GetComponent<SpringJoint>().currentForce *10);
        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "water")
        {
            EventManager.TriggerEvent(EventsName.InWater);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "water")
        {
            EventManager.TriggerEvent(EventsName.OutWater);
        }
    }

    private void OnCatchFish()
    {
        Debug.Log("Poisson ferré");



         miamPoisson = new GameObject("fixed poisson");
         miamPoisson.AddComponent<Rigidbody>();
         miamPoisson.GetComponent<Rigidbody>().isKinematic = true;
         miamPoisson.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.5f, this.transform.position.z);

        this.gameObject.AddComponent<SpringJoint>();
        this.gameObject.GetComponent<SpringJoint>().connectedBody = miamPoisson.GetComponent<Rigidbody>();
        this.gameObject.GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
        this.gameObject.GetComponent<SpringJoint>().connectedAnchor = Vector3.zero;
        this.gameObject.GetComponent<SpringJoint>().spring = hook_spring_when_shoe;

        cable_start_previous_spring = cable_start.GetComponent<SpringJoint>().spring;
        cable_start.GetComponent<SpringJoint>().spring = cable_start_spring_when_shoe;
        cable_start.GetComponent<CableComponent>().cableLength = 1;



    }

    private void OnReleaseFish()
    {
        cable_start.GetComponent<CableComponent>().cableLength = 4;
        cable_start.GetComponent<SpringJoint>().spring = cable_start_previous_spring;

        Destroy(this.gameObject.GetComponent<SpringJoint>());

        Destroy(miamPoisson);

    }

}
