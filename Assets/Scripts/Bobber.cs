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
#pragma warning restore 0649

    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;
    private float initialDrag;


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

    }


    private void OnTriggerEnter(Collider other)
    {
        EventManager.TriggerEvent(EventsName.InWater);
    }

    private void OnTriggerExit(Collider other)
    {
        EventManager.TriggerEvent(EventsName.OutWater);
    }

}
