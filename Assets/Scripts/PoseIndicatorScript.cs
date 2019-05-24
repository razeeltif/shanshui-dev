using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseIndicatorScript : MonoBehaviour
{

    public GameObject poseIndicatorPrefab;

    private GameObject actualPoseIndicator;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.HookFish, OnHookFish);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
        EventManager.StartListening(EventsName.ChangePose, OnChangePose);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.HookFish, OnHookFish);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
        EventManager.StopListening(EventsName.ChangePose, OnChangePose);
    }

    private void Update()
    {
        if(actualPoseIndicator != null)
            actualPoseIndicator.transform.position = PoissonFishing.instance.poseFishing.PosePosition;
    }

    void OnHookFish()
    {
        actualPoseIndicator = Instantiate(poseIndicatorPrefab);

    }

    void OnCatchFish()
    {
        Destroy(actualPoseIndicator);
    }

    void OnReleaseFish()
    {
        Destroy(actualPoseIndicator);
    }

    void OnChangePose()
    {
        //actualPoseIndicator.transform.position = PoissonFishing.instance.poseFishing.PosePosition;
    }
}
