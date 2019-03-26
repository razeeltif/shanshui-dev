using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CanneAPeche : GrablableObject
{

    private SteamVR_Behaviour_Pose handHoldingThis;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            


            //Debug.Log("rotation de la main  : " + handHoldingThis.transform.rotation + "\nrotation de la canne à peche : " + this.transform.rotation);

            this.transform.rotation = handHoldingThis.transform.rotation;
            this.transform.Rotate(new Vector3(-90, 0, 0));
            this.transform.position = handHoldingThis.transform.position;



        }
    }

    public override void Grab(SteamVR_Behaviour_Pose pose)
    {
        isGrabbed = true;

        handHoldingThis = pose;

        this.GetComponent<Rigidbody>().isKinematic = true;
        this.GetComponent<Collider>().enabled = false;

    }

    public override void Release(SteamVR_Behaviour_Pose pose)
    {
        isGrabbed = false;


        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Rigidbody>().velocity = handHoldingThis.GetVelocity();
        this.GetComponent<Rigidbody>().angularVelocity = handHoldingThis.GetAngularVelocity();
        handHoldingThis = null;
    }

}
