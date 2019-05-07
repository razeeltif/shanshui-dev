using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(FishingManagement))]
[RequireComponent(typeof(RepresentationPositionFishing))]
public class PoissonFishing : MonoBehaviour
{

    public static PoissonFishing instance;

    FishingManagement fishingManagement;
    [HideInInspector]
    public RepresentationPositionFishing poseFishing;
    
    public Transform bobber;
    [SerializeField]
    private Transform playerPosition;

#pragma warning disable 0649
    [SerializeField]
    GameObject fishInWaterPrefab;
    [SerializeField]
    GameObject fishModelPrefab;
#pragma warning restore 0649

    // variables for fish comportement
    [Range(0, 180)]
    public float AngleEscapingFish;
    public float distance;

    public float fishSpeed = 2f;
    public float tractionSpeed = 1f;
    public float fishDepth = 1;

    private UTimer fishEscapingTimer;
    public float fishEscapingFrequence;
    //

    private GameObject fishInWater;


    private bool onCatch = false;

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

        fishingManagement = GetComponent<FishingManagement>();
        poseFishing = GetComponent<RepresentationPositionFishing>();

        fishEscapingTimer = UTimer.Initialize(5, this, moveEscapingFish);
    }

    // Update is called once per frame
    void Update()
    {

        if (onCatch)
        {

            poseFishing.updatePosePosition();

            // get distance2D between the berge and the fish, relative to the water plane
            Vector3 fishInWater2D = new Vector3(fishInWater.transform.position.x, fishingManagement.waterPlane.position.y, fishInWater.transform.position.z);
            float distancePlayerFish = fishingManagement.getDistanceOnProjectionDirection(fishInWater.transform.position, fishingManagement.getPlayerPositionFromBerge(playerPosition.position));

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
                    EventManager.TriggerEvent(EventsName.CatchFish);
                    Debug.Log("END !! ------------------------------------------------------------------------");
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
            releaseFish();
        }

        // generate new fish points
        fishingManagement.generateFishPoints(playerPosition.position, bobber.position);

        // initialyze the fish position
        Vector3 initialPosition = new Vector3(bobber.position.x, bobber.position.y - fishDepth, bobber.position.z);
        fishInWater = initFishInWater(initialPosition);

        // init the first pose
        initCurrentStep();

        fishEscapingTimer.start(fishEscapingFrequence);

    }

    private void OnCatchFish()
    {
        bobber.GetComponent<Bobber>().attachFishToBobber(fishModelPrefab);
        Destroy(fishInWater);
        onCatch = false;
        currentStep = 0;
        fishEscapingTimer.Stop();
    }

    private void OnReleaseFish()
    {
        releaseFish();
        onCatch = false;
        currentStep = 0;
        fishEscapingTimer.Stop();
    }

    public GameObject initFishInWater(Vector3 initialPosition)
    {
        fishInWater = Instantiate(fishInWaterPrefab);
        fishInWater.transform.position = initialPosition;
        bobber.GetComponent<Bobber>().hookedFish(getRigidBodyFromChild());
        return fishInWater;
    }

    public void releaseFish()
    {
        bobber.GetComponent<Bobber>().detachFish();
        Destroy(fishInWater);
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
        onCatch = true;

    }

    private Vector3 getAngleFishEscaping()
    {
        // generate new angleX
        float angleX = Random.Range(-AngleEscapingFish, AngleEscapingFish);
        Vector3 posX = Quaternion.AngleAxis(angleX, Vector3.up) * Vector3.forward;
        return posX;
    }

    private Rigidbody getRigidBodyFromChild()
    {
        Rigidbody returnVal = null;
        // to get the rigidbody of the child, we need to remove the one from the parent
        Rigidbody[] ListRigibody = fishInWater.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rig in ListRigibody)
        {
            // if the rig possess a parent, it's the child we are looking for
            if (rig.transform.parent != null)
                returnVal = rig;
        }
        return returnVal;
    }

    private void moveEscapingFish()
    {
        Rigidbody rig = getRigidBodyFromChild();
        Vector3 angle = getAngleFishEscaping();
        rig.transform.position = fishInWater.transform.position + angle * distance;
        //rig.transform.position = -angle * distance;
        fishEscapingTimer.restart();

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
        if (onCatch)
        {
            for (int i = 0; i < fishingManagement.difficulty; i++)
            {
                Gizmos.DrawLine(fishingManagement.fishDifficultyLines[i, 0], fishingManagement.fishDifficultyLines[i, 1]);
                Gizmos.DrawSphere(fishingManagement.fishPoint[i].point, 0.1f);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(fishingManagement.getVectorOnProjectionDirection(fishInWater.transform.position, fishingManagement.getPlayerPositionFromBerge(playerPosition.position)), 0.11f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(fishingManagement.getPlayerPositionFromBerge(playerPosition.position), 0.1f);
        }

        Gizmos.DrawLine(Vector3.zero, getAngleFishEscaping() * 10);

    }

    public bool isFishing()
    {
        return onCatch;
    }
}
