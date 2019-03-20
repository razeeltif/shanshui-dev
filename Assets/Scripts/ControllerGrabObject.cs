using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerGrabObject : MonoBehaviour
{

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;

    public ControllerGrabObject otherHand;

    // Stores the GameObject that the trigger is currently colliding with, so you have the ability to grab the object.
    private GameObject collidingObject;

    // Serves as a reference to the GameObject that the player is currently grabbing.
    private GameObject objectInHand;


    // Start is called before the first frame update
    void Start()
    {
        if(otherHand == null)
        {
            Debug.LogError("the other hand is not referenced in " + gameObject.name);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        // we attempt to grab an object
        if (grabAction.GetLastStateDown(handType))
        {
            if (collidingObject.tag == "CanneAPeche")
            {
                GrabCanneAPeche();
            }
            else
            {
                GrabStandardObject();
            }
        }

        // we attempt to release an holded object
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand.tag == "CanneAPeche")
            {
                ReleaseCanneAPeche();
            }
            else
            {
                ReleaseObject();
            }
        }


        
    }

    private void GrabCanneAPeche()
    {
        // check if the object we want to grab is not already in the other hand
        if (collidingObject == otherHand.objectInHand)
        {
            Debug.Log("Je suis le meme ");
        }

        objectInHand = collidingObject;
        collidingObject = null;

        objectInHand.GetComponent<Collider>().enabled = false;
        objectInHand.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x + 90, this.transform.rotation.eulerAngles.y, this.transform.rotation.eulerAngles.z + 180);

        objectInHand.transform.position = this.transform.position + (objectInHand.transform.position - objectInHand.transform.GetChild(0).position);

    }

    private void GrabStandardObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        FixedJoint joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }





    private void ReleaseObject()
    {
        GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());

        objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
        objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

        objectInHand = null;
    }

    private void ReleaseCanneAPeche()
    {
        objectInHand.GetComponent<Collider>().enabled = true;
        objectInHand.GetComponent<Rigidbody>().velocity = controllerPose.GetVelocity();
        objectInHand.GetComponent<Rigidbody>().angularVelocity = controllerPose.GetAngularVelocity();

        objectInHand = null;
    }




    // create a joint between the controller and the holded object, and add some forces so the joint won't break easily
    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }


    private void SetCollidingObject (Collider col)
    {

        // if the object we want to grab has an rigidbody
        if (!collidingObject && col.GetComponent<Rigidbody>())
        {
                collidingObject = col.gameObject;
        }
    }


    private void OnTriggerEnter (Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }

    private void OnTriggerExit (Collider other)
    {
        if (collidingObject)
            collidingObject = null;
    }
}
