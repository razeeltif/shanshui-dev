
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{

    public GameSettings settings;

    public static GameManager instance;

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

        float time = settings.timeBeforeShoeFish + Random.Range(-settings.RandomOnTimeBeforeShoe, settings.RandomOnTimeBeforeShoe);
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

}
