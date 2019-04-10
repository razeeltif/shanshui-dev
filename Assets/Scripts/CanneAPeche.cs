﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CanneAPeche : GrablableObject
{

    private SteamVR_Behaviour_Pose firstHandHoldingThis;
    private SteamVR_Behaviour_Pose secondHandHoldingThis;

#pragma warning disable 0649 

    [SerializeField]
    private float FirstHandPosition;
    [SerializeField]
    private float SecondHandPosition;

    [SerializeField]
    private Rigidbody bendyRod;
#pragma warning restore 0649

    private bool isDualWield = false;


    private float bendyMass;
    private float bendyDrag;
    private float bendyAngularDrag;






    // Start is called before the first frame update
    void Start()
    {
        unsetBendyPhysic();
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrabbed)
        {
            if (isDualWield)
            {
                // on récupère le vecteur de direction entre la premiere main et la seconde
                Vector3 heading = secondHandHoldingThis.transform.position - firstHandHoldingThis.transform.position;
                Vector3 direction = heading.normalized;
                // on fait en sorte que la canne pointe vers le vecteur de direction
                this.transform.up = -direction;

                this.transform.position = firstHandHoldingThis.transform.position - this.transform.up * FirstHandPosition;
            }
            else
            {
                this.transform.rotation = firstHandHoldingThis.transform.rotation;
                this.transform.Rotate(new Vector3(-90, 0, 0));
                this.transform.position = firstHandHoldingThis.transform.position - this.transform.up * FirstHandPosition;
            }

        }
    }

    public override void Grab(SteamVR_Behaviour_Pose pose)
    {

        if (isGrabbed)
        {
            secondHandHoldingThis = pose;
            isDualWield = true;
        }
        else
        {
            firstHandHoldingThis = pose;

            isGrabbed = true;

            this.GetComponent<Rigidbody>().isKinematic = true;
            this.GetComponent<Collider>().isTrigger = true;

            setBendyPhysic();
        }


    }

    public override void Release(SteamVR_Behaviour_Pose pose)
    {

        // si on tient la canne à peche à 2 mains, on passe son maintient à 1 main
        if (isDualWield)
        {
            // si la main qui lâche la canne est la premiere main, on passe la seconde main en premiere main
            if(pose == firstHandHoldingThis)
            {
                firstHandHoldingThis = secondHandHoldingThis;
            }
            isDualWield = false;
        }

        // si on tient la canne à peche à 1 main, on la lâche
        else
        {
            isGrabbed = false;

            unsetBendyPhysic();

            this.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Collider>().isTrigger = false;
            this.GetComponent<Rigidbody>().velocity = firstHandHoldingThis.GetVelocity();
            this.GetComponent<Rigidbody>().angularVelocity = firstHandHoldingThis.GetAngularVelocity();
            firstHandHoldingThis = null;
        }
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

    private void setBendyPhysic()
    {
        bendyRod.mass = bendyMass;
        bendyRod.drag = bendyDrag;
        bendyRod.angularDrag = bendyAngularDrag;
    }

    private void unsetBendyPhysic()
    {
        bendyMass = bendyRod.mass;
        bendyDrag = bendyRod.drag;
        bendyAngularDrag = bendyRod.angularDrag;


        bendyRod.mass = 1;
        bendyRod.drag = 0;
        bendyRod.angularDrag = 0;

    }


}
