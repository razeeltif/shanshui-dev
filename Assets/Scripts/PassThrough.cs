using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Rigidbody))]
public class PassThrough : MonoBehaviour
{

    Rigidbody rb;

    Vector3 velocity;

    Vector3 angularVelocity;

    bool noCollision;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        velocity = rb.velocity;
        angularVelocity = rb.angularVelocity;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (noCollision)
        {
            Physics.IgnoreCollision(this.GetComponent<Collider>(), collision.collider);
            rb.velocity = velocity;
            rb.angularVelocity = angularVelocity;
        }
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        GetComponent<Collider>().isTrigger = true;
        noCollision = false;
    }


    //-------------------------------------------------
    protected virtual void OnDetachedFromHand(Hand hand)
    {

        GetComponent<Collider>().isTrigger = false;
        noCollision = true;
    }

}
