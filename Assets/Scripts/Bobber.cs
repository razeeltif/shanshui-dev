using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour, IUseSettings
{

    [SerializeField]
    FishRodSettings settings;

#pragma warning disable 0649
    [SerializeField]
    GameObject fishPrefab;
    [SerializeField]
    GameObject bendRod;
#pragma warning restore 0649

    private GameObject hookedFish;
    private float forceFactor;
    private Vector3 actionPoint;
    private Vector3 upLift;
    
    private GameObject miamPoisson;


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
            EventManager.TriggerEvent(EventsName.InWater);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "water")
        {
            EventManager.TriggerEvent(EventsName.OutWater);
        }
    }

    private void OnCatchFish()
    {

         miamPoisson = new GameObject("fixed poisson");
         miamPoisson.AddComponent<Rigidbody>();
         miamPoisson.GetComponent<Rigidbody>().isKinematic = true;
         miamPoisson.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1f, this.transform.position.z);

        this.gameObject.AddComponent<SpringJoint>();
        this.gameObject.GetComponent<SpringJoint>().connectedBody = miamPoisson.GetComponent<Rigidbody>();
        this.gameObject.GetComponent<SpringJoint>().autoConfigureConnectedAnchor = false;
        this.gameObject.GetComponent<SpringJoint>().connectedAnchor = Vector3.zero;
        this.gameObject.GetComponent<SpringJoint>().anchor = new Vector3(0, -this.transform.localScale.y, 0);
        this.gameObject.GetComponent<SpringJoint>().spring = settings.forceFish;
    }

    private void OnReleaseFish()
    {

        Destroy(this.gameObject.GetComponent<SpringJoint>());

        Destroy(miamPoisson);

        /*if(hookedFish == null)
        {*/
            hookedFish = Instantiate(fishPrefab);

            hookedFish.transform.position = transform.position + transform.TransformDirection(new Vector3(0, -0.4f, 0));

            hookedFish.AddComponent<FixedJoint>();
            hookedFish.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
            hookedFish.GetComponent<FixedJoint>().breakForce = 500;
      /*  }*/


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
}
