using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CanneAPeche : GrablableObject
{

    private SteamVR_Behaviour_Pose handHoldingThis;

    [SerializeField]
    private float FirstHandPosition;
    [SerializeField]
    private float SecondHandPosition;






    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            Vector3 p;
            p = this.transform.position + this.transform.up * FirstHandPosition;

            //Debug.Log("rotation de la main  : " + handHoldingThis.transform.rotation + "\nrotation de la canne à peche : " + this.transform.rotation);

            this.transform.rotation = handHoldingThis.transform.rotation;
            this.transform.Rotate(new Vector3(-90, 0, 0));
            this.transform.position = handHoldingThis.transform.position - this.transform.up * FirstHandPosition;



        }
    }

    public override void Grab(SteamVR_Behaviour_Pose pose)
    {

        handHoldingThis = pose;

        isGrabbed = true;

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


    private void OnDrawGizmos()
    {
        Vector3 p;

        // position de la  premiere main sur l'éditor
        Gizmos.color = Color.yellow;
        p = this.transform.position + this.transform.up * FirstHandPosition;
        Gizmos.DrawSphere(p, 0.1f);
        
        // position de la  deuxième main sur l'éditor
        Gizmos.color = Color.red;
        p = this.transform.position + this.transform.up * SecondHandPosition;
        Gizmos.DrawSphere(p, 0.1f);
    }

}
