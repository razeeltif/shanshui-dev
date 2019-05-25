using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Appat : MonoBehaviour
{

    public GameSettings gameSettings;
    [HideInInspector]
    public BoiteAppat boiteAppat;

    public GameObject[] typesPoisson;
    public float minimumWaitingTime;
    public float maximumWaitingTime;

    [HideInInspector]
    public bool isAttached = false;
    [HideInInspector]
    public bool canBeAttached = false; 

    private void OnEnable()
    {
        // abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand += Grab;
        GetComponent<Interactable>().onDetachedFromHand += Release;
    }

    private void OnDisable()
    {
        // de-abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand -= Grab;
        GetComponent<Interactable>().onDetachedFromHand -= Release;
    }

    private void Awake()
    {
        if(typesPoisson.Length <= 0)
        {
            Debug.LogError(name + " : Types Poisson not initialized");
        }
        if(minimumWaitingTime == 0)
        {
            Debug.LogError(name + " : minimumWaitingTime not initialized");
        }
        if (maximumWaitingTime == 0)
        {
            Debug.LogError(name + " : maximumWaitingTime not initialized");
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttached)
        {
            respawnAppatIfNeeded();

        }
    }


    void Grab(Hand hand)
    {
        Haptic.GrabObject(hand);
        if (PoissonFishing.instance.bobber.GetComponent<Bobber>().actualAppat == this)
        {
            PoissonFishing.instance.bobber.GetComponent<Bobber>().detachAppat();
            GetComponent<Rigidbody>().isKinematic = true;
        }

    }

    void Release(Hand hand)
    {
        if (canBeAttached)
        {
            attachAppatToBobber();
        }
        else
        {
            releaseAppat();
        }
    }

    void respawnAppatIfNeeded()
    {
        if (this.transform.position.y < gameSettings.YLimitsBeforeRespawn)
        {
            boiteAppat.RespawnAppat(this);
            GetComponent<Collider>().enabled = false;
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("grow", -1, 0f);
        }
    }

    void DeactivateAnimatorOnEndAnimation()
    {
        GetComponent<Animator>().enabled = false;
        GetComponent<Collider>().enabled = true;
    }

    void attachAppatToBobber()
    {
        PoissonFishing.instance.bobber.GetComponent<Bobber>().attachAppat(this);
    }

    public void releaseAppat()
    {
        isAttached = false;
        canBeAttached = false;
        GetComponent<Rigidbody>().isKinematic = false;
        EventManager.TriggerEvent(EventsName.ChangeAppat);
    }

    public int getRandomFishIndex()
    {
        int returnVal = Random.Range(0, typesPoisson.Length);
        return returnVal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "appatPosition")
        {
            canBeAttached = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "appatPosition")
        {
            canBeAttached = false;
        }
    }
}
