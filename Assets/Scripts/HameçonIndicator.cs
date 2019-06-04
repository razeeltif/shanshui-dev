using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HameçonIndicator : MonoBehaviour
{
    public GameObject hookIndicator;
    public float frequenceIndication = 1f;

    private bool meshOn = false;

    private UTimer clignotement;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.GrabFishrod, OnGrabFishrod);
        EventManager.StartListening(EventsName.ReleaseFishrod, OnReleaseFishrod);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.GrabFishrod, OnGrabFishrod);
        EventManager.StopListening(EventsName.ReleaseFishrod, OnReleaseFishrod);
    }

    // Start is called before the first frame update
    void Start()
    {
        clignotement = UTimer.Initialize(frequenceIndication, this, startClignotement, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (PoissonFishing.instance.bobber.GetComponent<Bobber>().actualAppat != null)
        {
            EraseIndicator();
        }
    }

    void OnGrabFishrod()
    {
        if(PoissonFishing.instance.bobber.GetComponent<Bobber>().actualAppat == null)
        {
            startClignotement();
        }
    }

    void OnReleaseFishrod()
    {
        EraseIndicator();
    }

    void EraseIndicator()
    {
        clignotement.Stop();
        GetComponentInChildren<MeshRenderer>().enabled = false;
        hookIndicator.SetActive(false);
    }

    void startClignotement()
    {
        meshOn = !meshOn;
        //Haptic.launchLine(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
        GameManager.instance.firstHandHoldingThis.hapticAction.Execute(0, 0.1f, 1, 0.4f, GameManager.instance.firstHandHoldingThis.handType);
        if (meshOn)
        {
            GetComponentInChildren<MeshRenderer>().enabled = true;
            hookIndicator.SetActive(true);
        }
        else
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
            hookIndicator.SetActive(false);
        }
        clignotement.start(frequenceIndication);
    }
}
