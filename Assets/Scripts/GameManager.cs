
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    // in seconds
    public float TimeBeforeShoeFish = 5f;
    public float RandomOnTimeBeforeShoeFish = 1f;
    public float TimerBeforeRelease = 5f;

    private UTimer FishTimer;
    private UTimer UnfishTimer;

    [HideInInspector]
    public Hand firstHandHoldingThis;
    [HideInInspector]
    public Hand secondHandHoldingThis;



    private void OnEnable()
    {
        EventManager.StartListening(EventsName.InWater, OnInWater);
        EventManager.StartListening(EventsName.OutWater, OnOutWater);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.InWater, OnInWater);
        EventManager.StopListening(EventsName.OutWater, OnOutWater);
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FishTimer = UTimer.Initialize(0, this, StartShoeFish);
        //UnfishTimer = UTimer.Initialize(0, this, ReleaseFish);
    }

    // Update is called once per frame
    void Update()
    {
    }




    // the bobber comes into the water
    void OnInWater()
    {

        Debug.Log("PAATTE PUTATEINSFDE");

        // we check the variables before putting them in the timer
        checkVariable();

        float time = TimeBeforeShoeFish + Random.Range(-RandomOnTimeBeforeShoeFish, RandomOnTimeBeforeShoeFish);
        FishTimer.start(time);


    }

    // the bobber exit the water
    void OnOutWater()
    {
        FishTimer.Stop();
    }


    public void StartShoeFish()
    {
        EventManager.TriggerEvent(EventsName.HookFish);
        //UnfishTimer.start(TimerBeforeRelease);
    }

    public void ReleaseFish()
    {
        EventManager.TriggerEvent(EventsName.ReleaseFish);
    }

    private void checkVariable()
    {

        if(TimeBeforeShoeFish < 0)
        {
            Debug.LogError("Time Before Shoe Fish in " + this.name + " must be positive");
        }

        if(RandomOnTimeBeforeShoeFish < 0)
        {
            Debug.LogError("Time Before Shoe Fish Randomness in " + this.name + " must be positive");
        }

        if(TimeBeforeShoeFish - RandomOnTimeBeforeShoeFish < 0)
        {
            Debug.LogError("Time Before Shoe Fish Randomness in " + this.name + " must be smaller or egual to Time Before Shoe Fish");
        }
    }

}
