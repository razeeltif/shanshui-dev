using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDrones : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.OutWater, OnInWater);
        EventManager.StartListening(EventsName.InWater, OnOutWater);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.OutWater, OnInWater);
        EventManager.StopListening(EventsName.InWater, OnOutWater);

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

}
