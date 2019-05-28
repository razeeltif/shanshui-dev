using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(FishingManagement))]
[RequireComponent(typeof(RepresentationPositionFishing))]
public class PoissonFishing : MonoBehaviour
{
    [HideInInspector]
    public static PoissonFishing instance;

    [HideInInspector]
    public FishingManagement fishingManagement;
    HapticDistance hapticDistance;
    [HideInInspector]
    public RepresentationPositionFishing poseFishing;
    
    public Transform bobber;

    public Transform playerPosition;

    private int indexOfFishHooked;
#pragma warning disable 0649
    [SerializeField]
    GameObject fishInWaterPrefab;
#pragma warning restore 0649

    // variables for fish comportement
    [Range(0, 180)]
    public float AngleEscapingFish;
    public float distance;

    public float fishSpeed = 2f;
    public float tractionSpeedMultiplicator = 1f;
    public float fishDepth = 1;

    private UTimer fishEscapingTimer;
    public float fishEscapingFrequence;
    //

    private GameObject fishInWater;


    private bool onCatch = false;

    private int currentStep = 0;

    private IEnumerator fishGoToThePointCoroutine;

    private float initialDistanceBetweenBobberAndBerge;



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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        fishingManagement = GetComponent<FishingManagement>();
        poseFishing = GetComponent<RepresentationPositionFishing>();
        hapticDistance = GetComponent<HapticDistance>();
        hapticDistance.enabled = false;

        fishEscapingTimer = UTimer.Initialize(5, this, moveEscapingFish);
    }

    // Update is called once per frame
    void Update()
    {

        if (onCatch)
        {
            // update of the pose position relative to the player's head
            poseFishing.updatePosePosition();

            // get distance2D between the berge and the fish, relative to the water plane
            Vector3 fishInWater2D = new Vector3(fishInWater.transform.position.x, fishingManagement.waterPlane.position.y, fishInWater.transform.position.z);
            Vector3 player2D = fishingManagement.getPlayerPositionFromBerge(playerPosition.position);
            float distancePlayerFish = fishingManagement.getDistanceOnProjectionDirection(fishInWater2D, player2D);

            if( currentStep == fishingManagement.difficulty - 1)
            {
                moveFishTowardPlayerOrBackwardIfNeeded(player2D, distancePlayerFish);

                if (distancePlayerFish < fishingManagement.distanceStep / 2)
                {
                    EventManager.TriggerEvent(EventsName.CatchFish);
                }
            }
            //check if we have to change step
            else if(distancePlayerFish < fishingManagement.distanceStep * (fishingManagement.difficulty - (currentStep + 1)) )
            {
                currentStep++;
                initCurrentStep();
            }
            else
            {
                moveFishTowardPlayerOrBackwardIfNeeded(player2D, distancePlayerFish);
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

        // initialyze fishManagement values based on infos in appat.fish
        Appat appat = bobber.GetComponent<Bobber>().actualAppat;
        indexOfFishHooked = appat.getRandomFishIndex();
        fishingManagement.difficulty = appat.typesPoisson[indexOfFishHooked].GetComponent<Poisson>().difficulty;
        tractionSpeedMultiplicator = appat.typesPoisson[indexOfFishHooked].GetComponent<Poisson>().tractionForce;



        // generate new fish points
        fishingManagement.generateFishPoints(playerPosition.position, bobber.position);

        // initialyze the fish position
        Vector3 initialPosition = new Vector3(bobber.position.x, bobber.position.y - fishDepth, bobber.position.z);
        fishInWater = initFishInWater(initialPosition);

        // get distance between the bobber and the berge
        Vector3 fishInWater2D = new Vector3(fishInWater.transform.position.x, fishingManagement.waterPlane.position.y, fishInWater.transform.position.z);
        Vector3 player2D = fishingManagement.getPlayerPositionFromBerge(playerPosition.position);
        initialDistanceBetweenBobberAndBerge = fishingManagement.getDistanceOnProjectionDirection(fishInWater2D, player2D);

        // init the first pose
        initCurrentStep();

        fishEscapingTimer.start(fishEscapingFrequence);

        hapticDistance.enabled = true;
        hapticDistance.hand = poseFishing.getHoldingHand();

    }

    private void OnCatchFish()
    {
        bobber.GetComponent<Bobber>().attachFishToBobber(bobber.GetComponent<Bobber>().actualAppat.typesPoisson[indexOfFishHooked]);
        Destroy(fishInWater);
        onCatch = false;
        hapticDistance.enabled = false;
        currentStep = 0;
        fishEscapingTimer.Stop();
    }

    private void OnReleaseFish()
    {
        releaseFish();
        onCatch = false;
        hapticDistance.enabled = false;
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

    private void moveFishTowardPlayerOrBackwardIfNeeded(Vector3 player2DPosition, float distancePlayerFish)
    {
        float dist = poseFishing.distanceBetweenFishRodAndPose();

        // the fish move forward the player
        if (dist < poseFishing.tolerance)
        {

            // the fish go toward to the player
            Vector3 targetPosition = player2DPosition;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y - fishDepth, targetPosition.z);

            float tractionSpeed = (initialDistanceBetweenBobberAndBerge / fishingManagement.difficulty) * (tractionSpeedMultiplicator/100);

            fishInWater.transform.position = Vector3.MoveTowards(fishInWater.transform.position, targetPosition, tractionSpeed);

            hapticDistance.enabled = false;
            Haptic.InPose(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
        }
        else
        {
            // the fish try to return to its ititial position
            Vector3 targetPosition = fishingManagement.fishPoint[currentStep].point;
            targetPosition = new Vector3(targetPosition.x, targetPosition.y - fishDepth, targetPosition.z);

            hapticDistance.enabled = true;

            if (Vector3.Distance(fishInWater.transform.position, targetPosition) > 0.1f)
                fishInWater.transform.position = Vector3.MoveTowards(fishInWater.transform.position, targetPosition, fishSpeed / 100);

        }
    }

    private void initCurrentStep()
    {
        spwanNewPose(fishingManagement.fishPoint[currentStep].direction);
        fishGoToThePointCoroutine = TravelToNextPoint(fishingManagement.fishPoint[currentStep].point);
        StartCoroutine(fishGoToThePointCoroutine);
        EventManager.TriggerEvent(EventsName.ChangePose);
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

            Vector3 fishInWaterOnWater = new Vector3(fishInWater.transform.position.x, fishingManagement.waterPlane.position.y, fishInWater.transform.position.z);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(fishInWaterOnWater, 0.1f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(fishingManagement.getVectorOnProjectionDirection(fishInWaterOnWater, fishingManagement.getPlayerPositionFromBerge(playerPosition.position)), 0.11f);

            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(fishingManagement.getPlayerPositionFromBerge(playerPosition.position), 0.1f);
        }

       // Gizmos.DrawLine(Vector3.zero, getAngleFishEscaping() * 10);

    }

    public bool isFishing()
    {
        return onCatch;
    }
}
