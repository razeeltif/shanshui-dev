using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFishHooked : MonoBehaviour
{
    public GameObject particleFishHookedPrefab;
    public GameObject particleFishCatchedPrefab;

    public Transform waterLevel;

    private GameObject actualParticle;

    bool fishHooked = false;


    private void OnEnable()
    {
        EventManager.StartListening(EventsName.HookFish, OnHookFish);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.HookFish, OnHookFish);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
    }


    // Update is called once per frame
    void Update()
    {
        if (fishHooked)
        {
            actualParticle.transform.position = new Vector3(PoissonFishing.instance.bobber.transform.position.x,
                                                            particleFishHookedPrefab.transform.position.y,
                                                            PoissonFishing.instance.bobber.transform.position.z);
        }
    }

    private void OnHookFish()
    {
        fishHooked = true;
        actualParticle = Instantiate(particleFishHookedPrefab);
        actualParticle.transform.position = new Vector3(PoissonFishing.instance.bobber.transform.position.x,
                                                        particleFishHookedPrefab.transform.position.y,
                                                        PoissonFishing.instance.bobber.transform.position.z);

    }

    private void OnCatchFish()
    {
        fishHooked = false;
        Destroy(actualParticle);
        GameObject fishCatched = Instantiate(particleFishCatchedPrefab);
        fishCatched.transform.position = new Vector3(PoissonFishing.instance.bobber.transform.position.x,
                                                            fishCatched.transform.position.y,
                                                            PoissonFishing.instance.bobber.transform.position.z);
    }

    private void OnReleaseFish()
    {
        fishHooked = false;
        Destroy(actualParticle);
    }
}
