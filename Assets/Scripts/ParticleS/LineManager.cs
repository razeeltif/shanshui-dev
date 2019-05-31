using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ridePrefab;

    [SerializeField]
    private GameObject smallRidePrefab;

    [SerializeField]
    private GameObject waterPlane;


    public float rideSpawnFrequency = 0.2f;

    public float distanceBetweenTwoSpawn = 0.2f;

    bool inWater = false;

    private UTimer rideTimer;

    private Vector3 lastLinePosition;

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
        rideTimer = UTimer.Initialize(rideSpawnFrequency, this, SpawnRide);
    }

    // Update is called once per frame
    void Update()
    {
        if (inWater)
        {


            Vector3 actualBobberPosition = new Vector3(PoissonFishing.instance.bobber.transform.position.x, waterPlane.transform.position.y, PoissonFishing.instance.bobber.transform.position.z);

            if(Vector3.Distance(actualBobberPosition, lastLinePosition) > distanceBetweenTwoSpawn)
            {
                SpawnSmallRide();
            }





        }
    }

    void SpawnRide()
    {
        GameObject obj = Instantiate(ridePrefab);
        obj.transform.position = new Vector3(PoissonFishing.instance.bobber.transform.position.x, waterPlane.transform.position.y, PoissonFishing.instance.bobber.transform.position.z);
        rideTimer.start(rideSpawnFrequency);

        lastLinePosition = obj.transform.position;
    }

    void SpawnSmallRide()
    {
        GameObject obj = Instantiate(smallRidePrefab);
        obj.transform.position = new Vector3(PoissonFishing.instance.bobber.transform.position.x, waterPlane.transform.position.y, PoissonFishing.instance.bobber.transform.position.z);
        rideTimer.start(rideSpawnFrequency);

        lastLinePosition = obj.transform.position;
    }

    void OnInWater()
    {
        inWater = true;
        SpawnRide();
        rideTimer.start(rideSpawnFrequency);
    }

    void OnOutWater()
    {
        inWater = false;
        rideTimer.Stop();
    }
}
