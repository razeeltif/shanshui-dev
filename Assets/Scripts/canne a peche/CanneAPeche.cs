using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CanneAPeche : MonoBehaviour, IUseSettings
{

    public FishRodSettings settings;
    public HapticSettings hapticSettings;

    /*[HideInInspector]
    public Hand firstHandHoldingThis;
    [HideInInspector]
    public Hand secondHandHoldingThis;*/

    public GameObject bendyRod;
    public GameObject tipRod;

    private bool isDualWield = false;


    private float bendyMass;
    private float bendyDrag;
    private float bendyAngularDrag;

    private bool isCatching = false;

    private bool isGrabbed = false;

    Interactable inter;

    UTimer bobberTimer;

    private void OnEnable()
    {
        settings.AddGameObjectListening(this);
        EventManager.StartListening(EventsName.HookFish, OnHookFish);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);

        // abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand += Grab;
        GetComponent<Interactable>().onDetachedFromHand += Release;
    }

    private void OnDisable()
    {
        settings.RemoveGameObjectListening(this);
        EventManager.StopListening(EventsName.HookFish, OnHookFish);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);

        // de-abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand -= Grab;
        GetComponent<Interactable>().onDetachedFromHand -= Release;
    }

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        bobberTimer = UTimer.Initialize(0.1f, this, setBobberKinematicOnFalse);

        bendyRod.GetComponent<Renderer>().enabled = false;
        setCableComponentValues();
        setBendySpringJointNormalState();
        unsetBendyPhysic();
    }

    // Update is called once per frame
    void Update()
    {

        if (isGrabbed)
        {
            // on veut faire vibrer la manette seulement quand on l'a en main et qu'on n'a pas ferré un poisson
            if(!isCatching)
                vibrationWhenMoving();

            if (isDualWield)
            {
                // on récupère le vecteur de direction entre la premiere main et la seconde
                Vector3 heading = GameManager.instance.secondHandHoldingThis.transform.position - GameManager.instance.firstHandHoldingThis.transform.position;
                Vector3 direction = heading.normalized;
                // on fait en sorte que la canne pointe vers le vecteur de direction
                this.transform.up = -direction;

                this.transform.position = GameManager.instance.firstHandHoldingThis.transform.position - this.transform.up * settings.handPosition;
            }
            else
            {
                this.transform.rotation = GameManager.instance.firstHandHoldingThis.transform.rotation;
                this.transform.Rotate(new Vector3(-90, 0, 0));
                this.transform.position = GameManager.instance.firstHandHoldingThis.transform.position - this.transform.up * settings.handPosition;
            }

        }
    }

    public void Grab(Hand hand)
    {

        Haptic.GrabObject(hand);

        if (isGrabbed)
        {
            GameManager.instance.secondHandHoldingThis = hand;
            isDualWield = true;
        }
        else
        {
            PoissonFishing.instance.bobber.GetComponent<Rigidbody>().isKinematic = true;
            bobberTimer.start(0.1f);

            setFirstHand(hand);

            isGrabbed = true;

            this.GetComponent<Rigidbody>().isKinematic = true;
            this.bendyRod.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Collider>().isTrigger = true;

            setBendyPhysic();

        }

    }

    public void Release(Hand hand)
    {

        // si on tient la canne à peche à 2 mains, on passe son maintient à 1 main
        if (isDualWield)
        {
            // si la main qui lâche la canne est la premiere main, on passe la seconde main en premiere main
            if(hand == GameManager.instance.firstHandHoldingThis)
            {
                setFirstHand(GameManager.instance.secondHandHoldingThis);
            }
            GameManager.instance.secondHandHoldingThis = null;
            isDualWield = false;
        }

        // si on tient la canne à peche à 1 main, on la lâche
        else
        {
            
            isGrabbed = false;

            unsetBendyPhysic();

            this.GetComponent<Rigidbody>().isKinematic = false;
            //this.GetComponent<Collider>().isTrigger = false;
            this.GetComponent<Rigidbody>().velocity = GameManager.instance.firstHandHoldingThis.GetComponent<SteamVR_Behaviour_Pose>().GetVelocity();
            this.GetComponent<Rigidbody>().angularVelocity = GameManager.instance.firstHandHoldingThis.GetComponent<SteamVR_Behaviour_Pose>().GetAngularVelocity();
            GameManager.instance.firstHandHoldingThis = null;
        }
        
    }

    // vibration quand la canne à pêche est trop remuée
    private void vibrationWhenMoving()
    {
        Vector3 velocity = bendyRod.GetComponent<Rigidbody>().velocity;
        float speed = velocity.magnitude;
        float difference = hapticSettings.maximumSpeedToFullVibration - hapticSettings.minimumSpeedToVibrate;
        //amplitude = 0f;

        if (speed > hapticSettings.minimumSpeedToVibrate)
        {
            //amplitude = Mathf.Clamp01(speed / difference);
            float amplitude = (speed - hapticSettings.minimumSpeedToVibrate) / difference;
            Haptic.vibrationWHenMoving(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis, amplitude);
        }
        
    }

    private void OnHookFish()
    {
        isCatching = true;
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthWhenCatchAFish;
        setBendySpringJointCatchState();
    }

    private void OnReleaseFish()
    {
        isCatching = false;
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
        setBendySpringJointNormalState();
    }

    private void OnCatchFish()
    {
        isCatching = false;
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
        setBendySpringJointNormalState();
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

    private void setBendySpringJointNormalState()
    {
        bendyRod.GetComponent<SpringJoint>().spring = settings.SpringNormalState;
        bendyRod.GetComponent<SpringJoint>().damper = settings.DamperNormalState;
        bendyRod.GetComponent<SpringJoint>().massScale = settings.MassScaleNormalState;
    }

    private void setBendySpringJointCatchState()
    {
        bendyRod.GetComponent<SpringJoint>().spring = settings.SpringCatchState;
        bendyRod.GetComponent<SpringJoint>().damper = settings.DamperCatchState;
        bendyRod.GetComponent<SpringJoint>().massScale = settings.MassScaleCatchState;
    }

    private void setCableComponentValues()
    {
        tipRod.GetComponent<CableComponent>().totalSegments = settings.totalSegments;
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
        tipRod.GetComponent<CableComponent>().cableWidth = settings.width;
        tipRod.GetComponent<CableComponent>().verletIterations = settings.rigidity;
        tipRod.GetComponent<CableComponent>().cableStartOffset = new Vector3(0, 0, 0);
        tipRod.GetComponent<CableComponent>().cableEndOffset = new Vector3(0, tipRod.GetComponent<CableComponent>().cableEnd.localScale.y / 2, 0);
    }

    public Vector3 getTipOfFishRod()
    {
        return bendyRod.transform.position + bendyRod.transform.right * bendyRod.GetComponent<CableComponent>().cableStartOffset.x + bendyRod.transform.up * bendyRod.GetComponent<CableComponent>().cableStartOffset.y + bendyRod.transform.forward * bendyRod.GetComponent<CableComponent>().cableStartOffset.z;
    }

    private void setFirstHand(Hand hand)
    {
        GameManager.instance.firstHandHoldingThis = hand;
    }

    private void setBobberKinematicOnFalse()
    {
        PoissonFishing.instance.bobber.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void OnModifySettings()
    {
        if (isGrabbed)
        {
            setBendyPhysic();
        }

        if (isCatching)
            setBendySpringJointCatchState();
        else
            setBendySpringJointNormalState();


        tipRod.GetComponent<CableComponent>().verletIterations = settings.rigidity;
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState;
    }
}
