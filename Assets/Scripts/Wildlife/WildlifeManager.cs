﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildlifeManager : MonoBehaviour
{
    static public WildlifeManager instance;

    public float chanceJumpFish, chanceJumpBird, chanceSpawnBoat;
    float tirageFish, tirageBird, tirageBoat;
    float xMinF, xMaxF, xMinBoat, xMaxBoat;
    float yMinB, yMaxB;
    float zMinF, zMaxF, zMinB, zMaxB;
    float rotationRand;

    [SerializeField] GameObject fishPrefab;
    [SerializeField] GameObject spawnerFish;
    [SerializeField] GameObject[] birdPrefab;
    [SerializeField] GameObject spawnerBird;
    [SerializeField] GameObject boatPrefab;
    [SerializeField] GameObject spawnerBoat;
    [SerializeField] GameObject turtlePrefab;
    [SerializeField] GameObject spawnerTurtle;
    [SerializeField] GameObject waterPlane;
    GameObject newFish;
    GameObject newBirdFlock;
    GameObject newBoat;

    Animator anim;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = fishPrefab.GetComponent<Animator>();
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

        if (tirageBird <= 0.25f) SpawnBirdFlock(0);
        if (tirageBird <= 0.50f && tirageBird > 0.25f) SpawnBirdFlock(1);
        if (tirageBird <= 0.75f && tirageBird > 0.50f) SpawnBirdFlock(2);
        if (tirageBird <= 1 && tirageBird >= 0.75f) SpawnBirdFlock(3);

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
        newFish.transform.position = new Vector3(Random.Range(spawnerFish.transform.position.x - 40, spawnerFish.transform.position.x + 40) , waterPlane.transform.position.y -0.5f, Random.Range(spawnerFish.transform.position.z - 30, spawnerFish.transform.position.z + 30));
    }

    void SpawnBirdFlock(int birdtospawn)
    {
        newBirdFlock = Instantiate(birdPrefab[birdtospawn], transform);
        newBirdFlock.transform.position = new Vector3(Random.Range(spawnerBird.transform.position.x - 100, spawnerFish.transform.position.x + 100), Random.Range(spawnerBird.transform.position.y - 50, spawnerBird.transform.position.y + 50), spawnerBird.transform.position.z);
        newBirdFlock.transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    void SpawnBoat()
    {
        newBoat = Instantiate(boatPrefab, transform);
        newBoat.transform.position = new Vector3(150, waterPlane.transform.position.y, Random.Range(spawnerBoat.transform.position.z - 30, spawnerBoat.transform.position.z + 30));
        newBoat.transform.rotation = Quaternion.Euler(0, 90, 0);
    }

    void SpawnBoat2()
    {
        newBoat = Instantiate(boatPrefab, transform);
        newBoat.transform.position = new Vector3(-150, waterPlane.transform.position.y - 0.5f, Random.Range(spawnerBoat.transform.position.z - 40, spawnerBoat.transform.position.z + 40));
        newBoat.transform.rotation = Quaternion.Euler(0, 270, 0);
    }
}