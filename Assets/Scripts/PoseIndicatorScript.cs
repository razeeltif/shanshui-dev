using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseIndicatorScript : MonoBehaviour
{

    public GameObject poseIndicatorPrefab100;
    public GameObject poseIndicatorPrefab75;
    public GameObject poseIndicatorPrefab50;
    public GameObject poseIndicatorPrefab25;

    private GameObject actualPoseIndicator;

    private bool aFishHasBeenCatched = false;

    private int poseCounter = 0;

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

            switch(poseCounter){
                case 0:
                    actualPoseIndicator = Instantiate(poseIndicatorPrefab100);
                    //actualPoseIndicator.GetComponent<Animator>().Play("growSphereIndicator100");
                    break;

                case 1:
                    Destroy(actualPoseIndicator);
                    actualPoseIndicator = Instantiate(poseIndicatorPrefab75);
                    //actualPoseIndicator.GetComponent<Animator>().Play("growSphereIndicator75");
                    break;

                case 2:
                    Destroy(actualPoseIndicator);
                    actualPoseIndicator = Instantiate(poseIndicatorPrefab50);
                    //actualPoseIndicator.GetComponent<Animator>().Play("growSphereIndicator50");
                    break;

                case 3:
                    Destroy(actualPoseIndicator);
                    actualPoseIndicator = Instantiate(poseIndicatorPrefab25);
                    //actualPoseIndicator.GetComponent<Animator>().Play("growSphereIndicator25");
                    break;

            }

        poseCounter++;

    }
}
