
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // in seconds
    public float TimeBeforeShoeFish = 5f;
    public float RandomOnTimeBeforeShoeFish = 1f;

    private Timer FishTimer;



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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }




    // the bobber comes into the water
    void OnInWater()
    {
        // we check the variables before putting them in the timer
        checkVariable();

        float time = TimeBeforeShoeFish + Random.Range(-RandomOnTimeBeforeShoeFish, RandomOnTimeBeforeShoeFish);

        // start Timer
        FishTimer = new Timer(time * 1000);
        FishTimer.Elapsed += HandleTimerElapsed;
        FishTimer.Enabled = true;
        FishTimer.AutoReset = false;
    }

    // the bobber exit the water
    void OnOutWater()
    {
        // if the timer is running, we reset it
        if (FishTimer != null && FishTimer.Enabled)
        {
            FishTimer.Dispose();
        }
    }

    // callback function called at the end of the timer 
    public void HandleTimerElapsed(object sender, ElapsedEventArgs e)
    {
        // do whatever it is that you need to do on a timer
        Debug.Log("JE FERRE LE POISSON PUTAIN");
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
