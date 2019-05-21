using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class Poisson : MonoBehaviour
{

    public int difficulty;
    public float tractionForce;
    public Color color;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Grab(SteamVR_Behaviour_Pose pose)
    {
        if(transform.parent != null)
        {
            transform.parent.DetachChildren();

        }
        FixedJoint fx = pose.gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.connectedBody = this.GetComponent<Rigidbody>();
    }

    public void Release(SteamVR_Behaviour_Pose pose)
    {
        pose.GetComponent<FixedJoint>().connectedBody = null;
        Destroy(pose.GetComponent<FixedJoint>());

        GetComponent<Rigidbody>().velocity = pose.GetVelocity();
        GetComponent<Rigidbody>().angularVelocity = pose.GetAngularVelocity();
    }
}
