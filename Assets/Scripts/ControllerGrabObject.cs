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
    [HideInInspector]
    public GameObject objectInHand;


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
            // if there is an object near our hand
            if (collidingObject)
            {
                Debug.Log(collidingObject.gameObject.name);
                // if the object has a special action when grabbed
                if (collidingObject.GetComponent<GrablableObject>() != null)
                {
                    objectInHand = collidingObject;
                    collidingObject = null;
                    objectInHand.GetComponent<GrablableObject>().Grab(controllerPose);
                }
                // else, we grab it has a regular object
                else
                {
                    // we check if the object we want to grab is not already grabbed by the other hand
                    if(otherHand.objectInHand == collidingObject)
                    {
                        // if so, we release the object from the other hand, and we grab it with this hand
                        otherHand.ReleaseStandardObject();
                    }
                    GrabStandardObject();
                }
            }
        }

        // we attempt to release an holded object
        if (grabAction.GetLastStateUp(handType))
        {
            if (objectInHand)
            {
                if (objectInHand.GetComponent<GrablableObject>() != null)
                {
                    objectInHand.GetComponent<GrablableObject>().Release(controllerPose);
                    objectInHand = null;
                }
                else
                {
                    ReleaseStandardObject();
                }
            }
        }


        
    }


    public void GrabStandardObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        FixedJoint joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }





    public void ReleaseStandardObject()
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
