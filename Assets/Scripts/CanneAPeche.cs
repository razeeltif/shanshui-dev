using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CanneAPeche : GrablableObject, IUseSettings
{

    public FishRodSettings settings;

    private SteamVR_Behaviour_Pose firstHandHoldingThis;
    private SteamVR_Behaviour_Pose secondHandHoldingThis;

#pragma warning disable 0649 

    [SerializeField]
    private GameObject bendyRod;
#pragma warning restore 0649

    private bool isDualWield = false;


    private float bendyMass;
    private float bendyDrag;
    private float bendyAngularDrag;

    private float bendyrod_previous_spring = 0;
    private float bendyrod_previous_damper = 0;



    private void OnEnable()
    {
        settings.AddGameObjectListening(this);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    private void OnDisable()
    {
        settings.RemoveGameObjectListening(this);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        setCableComponentValues();
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

                this.transform.position = firstHandHoldingThis.transform.position - this.transform.up * settings.handPosition;
            }
            else
            {
                this.transform.rotation = firstHandHoldingThis.transform.rotation;
                this.transform.Rotate(new Vector3(-90, 0, 0));
                this.transform.position = firstHandHoldingThis.transform.position - this.transform.up * settings.handPosition;
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

    private void OnCatchFish()
    {
        bendyrod_previous_spring = bendyRod.GetComponent<SpringJoint>().spring;
        bendyrod_previous_damper = bendyRod.GetComponent<SpringJoint>().damper;
        bendyRod.GetComponent<CableComponent>().cableLength = settings.lengthWhenCatchAFish;
        bendyRod.GetComponent<SpringJoint>().spring = settings.SpringCatchState;
        bendyRod.GetComponent<SpringJoint>().damper = settings.DamperCatchState;
    }

    private void OnReleaseFish()
    {
        bendyRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
        bendyRod.GetComponent<SpringJoint>().spring = bendyrod_previous_spring;
        bendyRod.GetComponent<SpringJoint>().damper = bendyrod_previous_damper;
    }


    private void OnDrawGizmos()
    {
        Vector3 p;

        // position de la  premiere main sur l'éditor
        Gizmos.color = Color.yellow;
        p = this.transform.position + this.transform.up * settings.handPosition;
        Gizmos.DrawSphere(p, 0.1f);
        
        // position de la  deuxième main sur l'éditor
        Gizmos.color = Color.red;
        p = this.transform.position + this.transform.up * settings.handPosition;
        Gizmos.DrawSphere(p, 0.1f);

    }

    private void setBendyPhysic()
    {
        bendyRod.GetComponent<Rigidbody>().mass = settings.mass;
        bendyRod.GetComponent<Rigidbody>().drag = settings.drag;
        bendyRod.GetComponent<Rigidbody>().angularDrag = settings.angularDrag;
    }

    private void unsetBendyPhysic()
    {

        bendyRod.GetComponent<Rigidbody>().mass = 1;
        bendyRod.GetComponent<Rigidbody>().drag = 0;
        bendyRod.GetComponent<Rigidbody>().angularDrag = 0;

    }

    private void setCableComponentValues()
    {
        bendyRod.GetComponent<CableComponent>().totalSegments = settings.totalSegments;
        bendyRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
        bendyRod.GetComponent<CableComponent>().cableWidth = settings.width;
        bendyRod.GetComponent<CableComponent>().verletIterations = settings.rigidity;
        bendyRod.GetComponent<CableComponent>().cableStartOffset = new Vector3(0, -bendyRod.transform.localScale.y, 0);
        bendyRod.GetComponent<CableComponent>().cableEndOffset = new Vector3(0, bendyRod.GetComponent<CableComponent>().cableEnd.localScale.y / 2, 0);
    }

    public void OnModifySettings()
    {
        if (isGrabbed)
        {
            setBendyPhysic();
        }
        bendyRod.GetComponent<CableComponent>().verletIterations = settings.rigidity;
        bendyRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
    }
}
