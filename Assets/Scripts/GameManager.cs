
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    private UTimer FishTimer;
    private UTimer UnfishTimer;



    private float TMax = 0;
    private float Tact = 0;

    public float minimumFishingTime = 10;




    [HideInInspector]
    public Hand firstHandHoldingThis;
    [HideInInspector]
    public Hand secondHandHoldingThis;

    public Text text;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.InWater, OnInWater);
        EventManager.StartListening(EventsName.OutWater, OnOutWater);
        EventManager.StartListening(EventsName.ReleaseFish, ResetTimer);
        EventManager.StartListening(EventsName.CatchFish, ResetTimer);
        EventManager.StartListening(EventsName.ChangeAppat, ResetTimer);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.InWater, OnInWater);
        EventManager.StopListening(EventsName.OutWater, OnOutWater);
        EventManager.StopListening(EventsName.ReleaseFish, ResetTimer);
        EventManager.StopListening(EventsName.CatchFish, ResetTimer);
        EventManager.StopListening(EventsName.ChangeAppat, ResetTimer);
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

        if (FishTimer != null)
        {
            text.text = FishTimer.getCooldown().ToString();
        }

    }




    // the bobber comes into the water
    void OnInWater()
    {

        // we launch the timer only if there is an hook attached to the bobber
        Appat actualAppat = PoissonFishing.instance.bobber.GetComponent<Bobber>().actualAppat;
        if (actualAppat != null)
        {
            // the timer has not been launched, so we launch it
            if(FishTimer.getCooldown() == 0)
            {

                TMax = Random.Range(actualAppat.minimumWaitingTime, actualAppat.maximumWaitingTime);
                Tact = TMax;
                FishTimer.start(TMax);

            }
            // the timer has been paused, we release it
            else
            {
                float tempsAttendu = FishTimer.getCooldown();
                float tempsActuel = TMax - (tempsAttendu / Tact) * TMax;

                if (tempsActuel < Tact)
                {
                    Tact = tempsActuel;
                }

                if (Tact <= minimumFishingTime)
                {
                    Tact += minimumFishingTime;
                }

                FishTimer.start(Tact);
            }


        }

    }

    // the bobber exit the water
    void OnOutWater()
    {
        FishTimer.pause();
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

    public void ResetTimer()
    {
        FishTimer.Stop();
    }

}
