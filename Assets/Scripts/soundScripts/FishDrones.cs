using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDrones : MonoBehaviour
{

    int nbFishCatched = 0;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.InWater, OnInWater);
        EventManager.StartListening(EventsName.InWater, OnOutWater);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.InWater, OnInWater);
        EventManager.StopListening(EventsName.OutWater, OnOutWater);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInWater()
    {
        AkSoundEngine.PostEvent("Stop_drone_fishing", Camera.main.gameObject);
    }

    void OnOutWater()
    {
        AkSoundEngine.PostEvent("Play_drone_fishing", Camera.main.gameObject);
    }

    void OnCatchFish()
    { 
        nbFishCatched++;

        switch (nbFishCatched) {
            case 1:
                AkSoundEngine.PostEvent("Play_drone_01", Camera.main.gameObject);
                break;

            case 2:
                AkSoundEngine.PostEvent("Play_drone_02", Camera.main.gameObject);
                break;

            case 3:
                AkSoundEngine.PostEvent("Play_drone_03", Camera.main.gameObject);
                break;

            case 4:
                AkSoundEngine.PostEvent("Play_drone_04", Camera.main.gameObject);
                break;

            case 5:
                AkSoundEngine.PostEvent("Play_drone_05", Camera.main.gameObject);
                break;


        }
    }



}
