using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildlifeManager : MonoBehaviour
{
    float chanceJumpFish, chanceJumpBird, chanceSpawnBoat;
    float tirageFish, tirageBird, tirageBoat;
    float xMinF, xMaxF, xMinBoat, xMaxBoat;
    float yMinB, yMaxB;
    float zMinF, zMaxF, zMinB, zMaxB;
    float rotationRand;

    [SerializeField] GameObject fishPrefab;
    [SerializeField] GameObject birdPrefab;
    [SerializeField] GameObject boatPrefab;
    [SerializeField] GameObject waterPlane;
    GameObject newFish;
    GameObject newBirdFlock;
    GameObject newBoat;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = fishPrefab.GetComponent<Animator>();
        chanceJumpFish = 2000;
        chanceJumpBird = 2000;
        chanceSpawnBoat = 2000;
        xMinBoat = 40;
        xMaxBoat = 100;
        xMinF = waterPlane.transform.position.x;
        xMaxF = waterPlane.transform.position.x + 10;
        zMinF = waterPlane.transform.position.z - 10;
        zMaxF = waterPlane.transform.position.z + 10;
        yMinB = 20;
        yMaxB = 50;
        zMinB = -60;
        zMaxB = 60;
    }

    // Update is called once per frame
    void Update()
    {
        tirageFish = Random.Range(0, chanceJumpFish);
        tirageBird = Random.Range(0, chanceJumpBird);
        tirageBoat = Random.Range(0, chanceSpawnBoat);

        if (tirageFish <= 1)
        {
            SpawnFish();
        }
        if (tirageBird <= 1)
        {
            SpawnBirdFlock();
        }
        if (tirageBoat <= 0.5f)
        {
            SpawnBoat();
        }
        if (tirageBoat > 0.5f && tirageBoat <= 1)
        {
            SpawnBoat2();
        }
    }

    void SpawnFish()
    {
        newFish = Instantiate(fishPrefab, transform);
        newFish.transform.rotation = Quaternion.Euler(newFish.transform.rotation.x, Random.Range(-180, 180), newFish.transform.rotation.z);
        newFish.transform.position = new Vector3(Random.Range(xMinF, xMaxF), waterPlane.transform.position.y -0.5f, Random.Range(zMinF, zMaxF));
    }

    void SpawnBirdFlock()
    {
        newBirdFlock = Instantiate(birdPrefab, transform);
        newBirdFlock.transform.position = new Vector3(20, Random.Range(yMinB, yMaxB), Random.Range(zMinB, zMaxB));
    }

    void SpawnBoat()
    {
        newBoat = Instantiate(boatPrefab, transform);
        newBoat.transform.position = new Vector3(Random.Range(xMinBoat, xMaxBoat), waterPlane.transform.position.y - 0.5f, -150);
    }

    void SpawnBoat2()
    {
        newBoat = Instantiate(boatPrefab, transform);
        newBoat.transform.position = new Vector3(Random.Range(xMinBoat, xMaxBoat), waterPlane.transform.position.y - 0.5f, 150);
        newBoat.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}