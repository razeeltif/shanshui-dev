using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseIndicatorScript : MonoBehaviour
{

    public GameObject poseIndicatorPrefab;

    private GameObject actualPoseIndicator;

    private bool aFishHasBeenCatched = false;

    private void OnEnable()
    {
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
        EventManager.StartListening(EventsName.ChangePose, OnChangePose);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
        EventManager.StopListening(EventsName.ChangePose, OnChangePose);
    }

    private void Update()
    {
        if(actualPoseIndicator != null)
            actualPoseIndicator.transform.position = PoissonFishing.instance.poseFishing.PosePosition;
    }

    void OnCatchFish()
    {
        Destroy(actualPoseIndicator);
        actualPoseIndicator = null;
    }

    void OnReleaseFish()
    {
        Destroy(actualPoseIndicator);
        actualPoseIndicator = null;
    }

    void OnChangePose()
    {
        if(actualPoseIndicator == null)
        {
            actualPoseIndicator = Instantiate(poseIndicatorPrefab);
        }
        //actualPoseIndicator.transform.position = PoissonFishing.instance.poseFishing.PosePosition;
    }
}
