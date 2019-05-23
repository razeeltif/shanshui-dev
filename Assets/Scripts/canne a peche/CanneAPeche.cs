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

    public float cableLengthOffset = 1f;

    // fix dégueu
    // stocker la secondHand lors du grab de la canne,
    // et l'attribuer à au GameManager uniquement dans l'Update
    // car sinon il l'attribu dans le grab, puis passe dans l'Update
    // avec toujours la touche trigger en mode GrabStarting
    // et donc passe dans le release
    private Hand tempHand = null;

    Interactable inter;

    UTimer bobberTimer;

    [Tooltip("The flags used to attach this object to the hand.")]
    public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

    [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
    public Transform attachmentOffset;




    private void OnEnable()
    {
        settings.AddGameObjectListening(this);
        EventManager.StartListening(EventsName.HookFish, OnHookFish);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);

    }

    private void OnDisable()
    {
        settings.RemoveGameObjectListening(this);
        EventManager.StopListening(EventsName.HookFish, OnHookFish);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);

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
                Vector3 handPosition;
                if(tempHand != null)
                {
                    handPosition = tempHand.transform.position;
                }
                else
                {
                    handPosition = GameManager.instance.secondHandHoldingThis.transform.position;
                }
                Vector3 heading = handPosition - GameManager.instance.firstHandHoldingThis.transform.position;
                Vector3 direction = heading.normalized;
                // on fait en sorte que la canne pointe vers le vecteur de direction
                this.transform.up = -direction;

                this.transform.position = GameManager.instance.firstHandHoldingThis.transform.position - this.transform.up * settings.handPosition;


                // detach fishrod from hand
                Hand hand = null;
                if (GameManager.instance.firstHandHoldingThis.GetGrabStarting() == GrabTypes.Pinch ||
                    GameManager.instance.firstHandHoldingThis.GetGrabStarting() == GrabTypes.Grip)
                    hand = GameManager.instance.firstHandHoldingThis;

                if (GameManager.instance.secondHandHoldingThis != null &&
                    (GameManager.instance.secondHandHoldingThis.GetGrabStarting() == GrabTypes.Pinch ||
                     GameManager.instance.secondHandHoldingThis.GetGrabStarting() == GrabTypes.Grip))
                    hand = GameManager.instance.secondHandHoldingThis;

                if(hand != null)
                {
                    Release(hand, hand.GetGrabStarting());
                }

                if(tempHand != null)
                {
                    GameManager.instance.secondHandHoldingThis = tempHand;
                    tempHand = null;
                }
            }
            else
            {
                this.transform.rotation = GameManager.instance.firstHandHoldingThis.transform.rotation;
                this.transform.Rotate(new Vector3(-90, 0, 0));
                this.transform.position = GameManager.instance.firstHandHoldingThis.transform.position - this.transform.up * settings.handPosition;

                // detach fishrod from hand
                if (GameManager.instance.firstHandHoldingThis.GetGrabStarting() == GrabTypes.Pinch ||
                    GameManager.instance.firstHandHoldingThis.GetGrabStarting() == GrabTypes.Grip)
                    Release(GameManager.instance.firstHandHoldingThis, GameManager.instance.firstHandHoldingThis.GetGrabStarting());
            }

        }
    }

    public void Grab(Hand hand, GrabTypes startingGrabType)
    {

        Haptic.GrabObject(hand);

        if (isGrabbed)
        {

            // switch to dualWield
            if (GameManager.instance.firstHandHoldingThis != null && hand != GameManager.instance.firstHandHoldingThis
                && hand != GameManager.instance.secondHandHoldingThis)
            {
                //GameManager.instance.secondHandHoldingThis = hand;
                tempHand = hand;
                GetComponentInChildren<Interacta>().StartDualWield(tempHand);
                isDualWield = true;
            }
        }
        else
        {
            PoissonFishing.instance.bobber.GetComponent<Rigidbody>().isKinematic = true;
            bobberTimer.start(0.1f);

            setFirstHand(hand);
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
            hand.HideGrabHint();

            this.GetComponent<Rigidbody>().isKinematic = true;
            this.bendyRod.GetComponent<Rigidbody>().isKinematic = false;
            this.GetComponent<Collider>().isTrigger = true;

            setBendyPhysic();

        }

    }

    public void Release(Hand hand, GrabTypes startingGrabType)
    {

        // si on tient la canne à peche à 2 mains, on passe son maintient à 1 main
        if (isDualWield)
        {
            // si la main qui lâche la canne est la premiere main, on passe la seconde main en premiere main
            if(hand == GameManager.instance.firstHandHoldingThis)
            {
                setFirstHand(GameManager.instance.secondHandHoldingThis);
                hand.DetachObject(gameObject, false);
                GameManager.instance.secondHandHoldingThis.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
            }
            else
            {
                GetComponentInChildren<Interacta>().StopDualWield(hand);
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
            hand.DetachObject(gameObject, false);
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
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState- cableLengthOffset;
        setBendySpringJointNormalState();
    }

    private void OnCatchFish()
    {
        isCatching = false;
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState- cableLengthOffset;
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

    protected virtual void OnHandHoverBegin(Hand hand)
    {
        bool showHint = false;

        // "Catch" the throwable by holding down the interaction button instead of pressing it.
        // Only do this if the throwable is moving faster than the prescribed threshold speed,
        // and if it isn't attached to another hand
       /* if (!attached && catchingSpeedThreshold != -1)
        {
            float catchingThreshold = catchingSpeedThreshold * SteamVR_Utils.GetLossyScale(Player.instance.trackingOriginTransform);

            GrabTypes bestGrabType = hand.GetBestGrabbingType();

            if (bestGrabType != GrabTypes.None)
            {
                if (rigidbody.velocity.magnitude >= catchingThreshold)
                {
                    hand.AttachObject(gameObject, bestGrabType, attachmentFlags);
                    showHint = false;
                }
            }
        }*/

        if (showHint)
        {
            hand.ShowGrabHint();
        }
    }


    //-------------------------------------------------
    protected virtual void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
    }


    //-------------------------------------------------
    protected virtual void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();

        if (startingGrabType != GrabTypes.None)
        {
            Grab(hand, startingGrabType);
        }
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
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState - cableLengthOffset;
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
        isGrabbed = true;
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
        tipRod.GetComponent<CableComponent>().cableLength = settings.lengthNormalState - cableLengthOffset;
    }
}
