using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PoissonFishing : MonoBehaviour
{

    public static PoissonFishing instance;


    [SerializeField]
    FishingManagement fishingManagement;

    public RepresentationPositionFishing poseFishing;
    
    public Transform bobber;
    [SerializeField]
    private Transform playerPosition;

#pragma warning disable 0649
    [SerializeField]
    GameObject fishPrefab;
#pragma warning restore 0649


    public float fishSpeed = 2f;
    public float tractionSpeed = 1f;
    public float fishDepth = 1;


    private GameObject fishInWater;


    private bool onCath = false;

    private int currentStep = 0;

    private IEnumerator fishGoToThePointCoroutine;



    private void OnEnable()
    {
        EventManager.StartListening(EventsName.HookFish, OnHookFish);
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.HookFish, OnHookFish);
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (onCath)
        {

            poseFishing.updatePosePosition();

            // get distance2D between the berge and the fish, relative to the water plane
            Vector3 fishInWater2D = new Vector3(fishInWater.transform.position.x, fishingManagement.waterPlane.position.y, fishInWater.transform.position.z);
            float distancePlayerFish = Vector3.Distance(fishingManagement.getPlayerPositionFromBerge(playerPosition.position), fishInWater2D);

            //check if we have to change step
            if (distancePlayerFish <= fishingManagement.distanceStep * (fishingManagement.difficulty - currentStep) )
            {

                // next step
                if(currentStep < fishingManagement.difficulty - 1)
                {
                    currentStep++;
                    initCurrentStep();
                }
                else
                {
                    Debug.Log("END !!");
                }
            }
            else
            {
                // check distance between fishrod and pose
                float dist = poseFishing.distanceBetweenFishRodAndPose();

                if(dist < poseFishing.tolerance)
                {

                    // the fish go toward to the player
                    // Vector3 targetPosition = new Vector3(poseFishing.canneAPeche.GetComponent<CanneAPeche>().getTipOfFishRod().x, fishingManagement.waterPlane.position.y - fishDepth, poseFishing.canneAPeche.GetComponent<CanneAPeche>().getTipOfFishRod().z);
                    Vector3 targetPosition = fishingManagement.getPlayerPositionFromBerge(playerPosition.position);
                    fishInWater.transform.position = Vector3.MoveTowards(fishInWater.transform.position, targetPosition, tractionSpeed / 100);
                }
                else
                {
                    // the fish try to return to its ititial position
                    Vector3 targetPosition = fishingManagement.fishPoint[currentStep].point;
                    targetPosition = new Vector3(targetPosition.x, targetPosition.y - fishDepth, targetPosition.z);

                    if(Vector3.Distance(fishInWater.transform.position, targetPosition) > 0.1f)
                        fishInWater.transform.position = Vector3.MoveTowards(fishInWater.transform.position, targetPosition, fishSpeed / 100);

                }
            }
        }
    }


    private void OnHookFish()
    {

        // if there is already an fish, we remove before adding a new one
        if (fishInWater != null)
        {
            Destroy(fishInWater);
            bobber.GetComponent<Bobber>().detachFish();
        }

        // generate new fish points
        fishingManagement.generateFishPoints(playerPosition.position, bobber.position);

        // initialyze the fish position
        Vector3 initialPosition = new Vector3(bobber.position.x, bobber.position.y - fishDepth, bobber.position.z);
        fishInWater = initFish(initialPosition);
        bobber.GetComponent<Bobber>().hookedFish(fishInWater);

        // init the first pose
        currentStep = 0;
        initCurrentStep();

    }

    private void OnCatchFish()
    {
        bobber.GetComponent<Bobber>().attachFishToBobber(fishPrefab);
    }

    private void OnReleaseFish()
    {
        onCath = false;
    }

    public GameObject initFish(Vector3 initialPosition)
    {
        GameObject fish = new GameObject("fishInWater");
        fish.AddComponent<Rigidbody>();
        fish.GetComponent<Rigidbody>().isKinematic = true;
        fish.transform.position = initialPosition;

        return fish;

    }

    private void spwanNewPose(int direction)
    {
        switch (direction)
        {
            // right
            case 2:
                poseFishing.spawnNewPoseRightSection();
                break;
                
            // middle
            case 1:
                poseFishing.SpawnNewPoseMiddleSection();
                break;            
            
            // left
            case 0:
                poseFishing.spawnNewPoseLeftSection();
                break;

        }
    }


    private IEnumerator TravelToNextPoint(Vector3 targetPosition)
    {

        targetPosition = new Vector3(targetPosition.x, targetPosition.y - fishDepth, targetPosition.z);

        while (Vector3.Distance(fishInWater.transform.position, targetPosition) >0.1f)
        {
            fishInWater.transform.position = Vector3.MoveTowards(fishInWater.transform.position, targetPosition, fishSpeed / 100);
            yield return new WaitForSeconds(0.01f);
        }
        onCath = true;

    }

    private void initCurrentStep()
    {
        spwanNewPose(fishingManagement.fishPoint[currentStep].direction);
        fishGoToThePointCoroutine = TravelToNextPoint(fishingManagement.fishPoint[currentStep].point);
        StartCoroutine(fishGoToThePointCoroutine);
    }




    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        if (onCath)
        {
            for (int i = 0; i < fishingManagement.difficulty; i++)
            {
                Gizmos.DrawLine(fishingManagement.fishDifficultyLines[i, 0], fishingManagement.fishDifficultyLines[i, 1]);
                Gizmos.DrawSphere(fishingManagement.fishPoint[i].point, 0.1f);
            }
        }

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(fishingManagement.getPlayerPositionFromBerge(playerPosition.position), 0.11f);
    }
}
