using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    int nbFishCatched = 0;
    bool inPose = false;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.InWater, OnInWater);
        EventManager.StartListening(EventsName.OutWater, OnOutWater);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
        EventManager.StartListening(EventsName.HookFish, OnHookFish);
        EventManager.StartListening(EventsName.ChangePose, OnChangePose);
        EventManager.StartListening(EventsName.InPose, OnInPose);
        EventManager.StartListening(EventsName.OutPose, OnOutPose);
        EventManager.StartListening(EventsName.SpawnPoseIndicator, OnSpawPoseIndicator);

    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.InWater, OnInWater);
        EventManager.StopListening(EventsName.OutWater, OnOutWater);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
        EventManager.StopListening(EventsName.HookFish, OnHookFish);
        EventManager.StopListening(EventsName.ChangePose, OnChangePose);
        EventManager.StopListening(EventsName.InPose, OnInPose);
        EventManager.StopListening(EventsName.OutPose, OnOutPose);
        EventManager.StopListening(EventsName.SpawnPoseIndicator, OnSpawPoseIndicator);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inPose)
        {
            Debug.Log(PoissonFishing.instance.completionInStep * 100);
            AkSoundEngine.SetRTPCValue("avance_poisson", PoissonFishing.instance.completionInStep * 100, Camera.main.gameObject);
        }
        
    }

    void OnChangePose()
    {
        inPose = false;
        AkSoundEngine.PostEvent("Play_fishing_feedbacks", Camera.main.gameObject);
    }

    void OnInPose()
    {
        inPose = true;
        AkSoundEngine.PostEvent("UnMute_note_luan", Camera.main.gameObject);
    }

    void OnOutPose()
    {
        inPose = false;
        AkSoundEngine.PostEvent("Mute_note_luan", Camera.main.gameObject);
    }

    void OnSpawPoseIndicator()
    {
        AkSoundEngine.PostEvent("Play_note_luan", Camera.main.gameObject);
    }

    void OnInWater()
    {
        AkSoundEngine.PostEvent("Play_drone_fishing", Camera.main.gameObject);
    }

    void OnOutWater()
    {
        AkSoundEngine.PostEvent("Stop_drone_fishing", Camera.main.gameObject);
    }

    void OnCatchFish()
    {
        inPose = false;

        AkSoundEngine.PostEvent("Play_fishing_reward", Camera.main.gameObject);

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

    void OnReleaseFish()
    {
        inPose = false;
    }

    void OnHookFish()
    {
        AkSoundEngine.PostEvent("Play_fish_catch", Camera.main.gameObject);
    }



}
