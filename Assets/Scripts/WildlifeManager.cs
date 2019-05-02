using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildlifeManager : MonoBehaviour
{
    float chanceJump, tirage;
    float xMin, xMax;
    float zMin, zMax;
    float rotationRand;

    bool fishExist;

    [SerializeField] GameObject fishPrefab;
    [SerializeField] GameObject waterPlane;
    GameObject newFish;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = fishPrefab.GetComponent<Animator>();
        chanceJump = 12000;
        xMin = waterPlane.transform.position.x;
        xMax = waterPlane.transform.position.x + 10;
        zMin = waterPlane.transform.position.z - 10;
        zMax = waterPlane.transform.position.z + 10;
    }

    // Update is called once per frame
    void Update()
    {
        tirage = Random.Range(0, chanceJump);
        if (tirage <= 1)
        {
            newFish = Instantiate(fishPrefab, transform);
            newFish.transform.rotation = Quaternion.Euler(newFish.transform.rotation.x, Random.Range(-180, 180), newFish.transform.rotation.z);
            newFish.transform.position = new Vector3(Random.Range(xMin, xMax), waterPlane.transform.position.y - 0.5f, Random.Range(zMin, zMax));
        }
     }
}
