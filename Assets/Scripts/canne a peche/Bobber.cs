using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour, IUseSettings
{

    [SerializeField]
    FishRodSettings settings;

    private GameObject catchedFish;
    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;

    private bool inWater = false;


    private void OnEnable()
    {
        settings.AddGameObjectListening(this);
    }

    private void OnDisable()
    {
        settings.RemoveGameObjectListening(this);
    }


    // Update is called once per frame
    void Update()
    {
        WaterSimulationOnBobber();
    }

    private void WaterSimulationOnBobber()
    {
        actionPoint = transform.position + transform.TransformDirection(settings.BouncyCenterOffset);
        forceFactor = 1f - ((actionPoint.y - settings.waterLevel) / settings.wave);

        if (forceFactor > 0f)
        {
            upLift = -Physics.gravity * (forceFactor - GetComponent<Rigidbody>().velocity.y * settings.bounceDamp);
            GetComponent<Rigidbody>().AddForceAtPosition(upLift, actionPoint);
            GetComponent<Rigidbody>().drag = settings.viscosity;
        }
        else
        {
            GetComponent<Rigidbody>().drag = settings.HookDrag;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "water")
        {
            inWater = true;
            Haptic.bobberInWater(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
            EventManager.TriggerEvent(EventsName.InWater);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "water")
        {
            inWater = false;
            Haptic.bobberOutWater(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
            EventManager.TriggerEvent(EventsName.OutWater);
        }
    }

    // setup the physics to simulate a fish shoeing the bobber
    public void hookedFish(Rigidbody obj) 
    {

        this.gameObject.AddComponent<SpringJoint>();
        this.gameObject.GetComponent<SpringJoint>().connectedBody = obj;
        this.gameObject.GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
        this.gameObject.GetComponent<SpringJoint>().connectedAnchor = Vector3.zero;
        this.gameObject.GetComponent<SpringJoint>().anchor = new Vector3(0, -this.transform.localScale.y, 0);
        this.gameObject.GetComponent<SpringJoint>().spring = settings.forceFish;

    }

    public void detachFish()
    {
        Destroy(this.gameObject.GetComponent<SpringJoint>());
    }

    public void attachFishToBobber(GameObject fish)
    {
        detachFish();

        catchedFish = Instantiate(fish);

        catchedFish.transform.position = transform.position + transform.TransformDirection(new Vector3(0, -0.4f, 0));

        catchedFish.AddComponent<FixedJoint>();
        catchedFish.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
        catchedFish.GetComponent<FixedJoint>().breakForce = 500;

    }

    public void OnModifySettings()
    {
        GetComponent<Rigidbody>().mass = settings.HookMass;
        GetComponent<Rigidbody>().drag = settings.HookDrag;
        GetComponent<Rigidbody>().angularDrag = settings.HookAngularDrag;

        if (this.gameObject.GetComponent<SpringJoint>())
        {
            this.gameObject.GetComponent<SpringJoint>().spring = settings.forceFish;
        }
    }

    public bool isInWater()
    {
        return inWater;
    }
}
