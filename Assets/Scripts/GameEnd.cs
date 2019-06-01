using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{


    public float lastFishTimer = 10f;

    public float timeAfterLastFish = 5f;


    private UTimer lastFishCool;

    private UTimer afterLastFishCool;

    private bool lastFishOn = false;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    // Start is called before the first frame update
    void Start()
    {
        lastFishCool = UTimer.Initialize(lastFishTimer, this, OnLastFishTimer);
        afterLastFishCool = UTimer.Initialize(timeAfterLastFish, this, OnAfterLastFish);

        lastFishCool.start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCatchFish()
    {
        if (lastFishOn)
        {
            afterLastFishCool.start();
            WildlifeManager.instance.chanceJumpFish = 10;
        }
    }

    void OnReleaseFish()
    {
        if (lastFishOn)
        {
            afterLastFishCool.start();
            WildlifeManager.instance.chanceJumpFish = 10;
        }
    }


    void OnLastFishTimer()
    {
        Debug.Log("LAST FISH TIMER FINISHED");
        lastFishOn = true;
    }

    void OnAfterLastFish()
    {

        //SceneSwitcher.instance.LoadEndScene();

    }


}
