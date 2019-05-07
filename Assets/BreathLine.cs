using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathLine : MonoBehaviour
{
    Bobber bobber;
    [SerializeField] GameObject waterPlane;

    [SerializeField] GameObject breathLinePref;
    GameObject breathLineInst;

    bool lineSpawned;
    [SerializeField] float maxCircleSize;

    void Start()
    {
        bobber = GetComponent<Bobber>();
    }

    void Update()
    {
        if(bobber.isInWater() && !lineSpawned)
        {
            breathLineInst = Instantiate(breathLinePref);
            breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);
            lineSpawned = true;
        }
        else if(bobber.isInWater() && lineSpawned)
        {
            breathLineInst.transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
            breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);

        }
        else if(breathLineInst != null && !bobber.isInWater())
        {
            Destroy(breathLineInst);
            lineSpawned = false;
        }
    }
}
