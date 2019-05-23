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

    public Appat actualAppat;

    // la fonction IsTriggerEnter est appelé 2x pour une raison inconnue
    // une méthode (dégueu) de contourner ça est de ne compter qu'un appel sur 2
    private int onInWaterFix = 0;

    // la fonction IsTriggerEnter est appelé 2x pour une raison inconnue
    // une méthode (dégueu) de contourner ça est de ne compter qu'un appel sur 2
    private int onOutWaterFix = 0;

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

        if(actualAppat != null)
        {
            actualizeAppatPosition();
        }

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
            GetComponent<Rigidbody>().drag = settings.BoobberDrag;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "water")
        {
            if(onInWaterFix == 0)
            {
                inWater = true;
                Haptic.bobberInWater(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
                EventManager.TriggerEvent(EventsName.InWater);
                AkSoundEngine.PostEvent("Play_Bouchon_Water", gameObject);


                onInWaterFix++;
            }
            else
            {
                onInWaterFix = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "water")
        {
            if(onOutWaterFix == 0)
            {
                inWater = false;
                Haptic.bobberOutWater(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
                EventManager.TriggerEvent(EventsName.OutWater);

                onOutWaterFix++;
            }
            else
            {
                onOutWaterFix = 0;
            }
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
        catchedFish.transform.rotation = this.transform.rotation;
        catchedFish.transform.Rotate(-90, 0, 0);

        //float distance = Vector3.Distance(catchedFish.transform.position, catchedFish.GetComponent<Poisson>().attachPoint.position);
        //catchedFish.transform.position = transform.position + transform.TransformDirection(new Vector3(0, -distance, 0));
        catchedFish.transform.position = GetComponentInChildren<AppatSign>().transform.position;
        Vector3 pos = GetComponentInChildren<AppatSign>().transform.position - catchedFish.GetComponent<Poisson>().attachPoint.position;
        catchedFish.transform.position = GetComponentInChildren<AppatSign>().transform.position + pos;
        //catchedFish.transform.position = transform.position + transform.TransformDirection(catchedFish.GetComponent<Poisson>().attachPoint.position - catchedFish.transform.position);


        catchedFish.AddComponent<FixedJoint>();
        catchedFish.GetComponent<FixedJoint>().connectedBody = this.GetComponent<Rigidbody>();
        //catchedFish.GetComponent<FixedJoint>().connectedAnchor = catchedFish.GetComponent<Poisson>().attachPoint.localPosition;
        catchedFish.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;
        catchedFish.GetComponent<FixedJoint>().breakTorque = Mathf.Infinity;
        /*this.gameObject.AddComponent<FixedJoint>();

        this.gameObject.GetComponent<FixedJoint>().connectedBody = catchedFish.GetComponent<Rigidbody>();
        //catchedFish.GetComponent<Poisson>().attachPoint.position
        this.gameObject.GetComponent<FixedJoint>().anchor = catchedFish.transform.position - catchedFish.GetComponent<Poisson>().attachPoint.position;
        this.gameObject.GetComponent<FixedJoint>().breakForce = Mathf.Infinity;*/

    }

    public void OnModifySettings()
    {
        GetComponent<Rigidbody>().mass = settings.BobberMass;
        GetComponent<Rigidbody>().drag = settings.BoobberDrag;
        GetComponent<Rigidbody>().angularDrag = settings.BobberAngularDrag;

        if (this.gameObject.GetComponent<SpringJoint>())
        {
            this.gameObject.GetComponent<SpringJoint>().spring = settings.forceFish;
        }
    }

    public void attachAppat(Appat appat)
    {
        if(actualAppat != null)
        {
            detachAppat();
        }

        actualAppat = appat;
        actualAppat.isAttached = true;
        actualAppat.transform.position = GetComponentInChildren<AppatSign>().transform.position;

        GetComponentInChildren<AppatSign>().GetComponent<MeshRenderer>().enabled = false;
    }

    public void detachAppat()
    {
        actualAppat.releaseAppat();
        actualAppat = null;
    }

    private void actualizeAppatPosition()
    {
        actualAppat.transform.position = GetComponentInChildren<AppatSign>().transform.position;
        actualAppat.transform.rotation = GetComponentInChildren<AppatSign>().transform.rotation;
    }

    public bool isInWater()
    {
        return inWater;
    }
}
