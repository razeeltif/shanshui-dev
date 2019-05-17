using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathLine : MonoBehaviour
{
    Bobber bobber;
    GameObject waterPlane;

    [SerializeField] GameObject breathLinePref;
    GameObject breathLineInst;

    bool lineSpawned;
    float maxCircleRadius;
    [SerializeField] float sizeMult;
    public AnimationCurve controleSpeed, controleSpeedOut;
    float lineSpeed;
    [SerializeField] float lineMult;
    [SerializeField] float maxSizeDistance;

    Color currColor;
    SpriteRenderer circleSRenderer;
    bool breathingIn;

    float distance;

    Vector3 berge;

    void Start()
    {
        waterPlane = GameManager.instance.GetComponent<FishingManagement>().waterPlane.gameObject;
        bobber = GetComponent<Bobber>();
    }

    void Update()
    {

        if (bobber.isInWater() && !lineSpawned)
        {
            // get distance between the bobber and the berge 
            berge = new Vector3(bobber.transform.position.x, bobber.transform.position.y, waterPlane.transform.position.z - waterPlane.transform.localScale.z * 5);
            distance = Vector3.Distance(bobber.transform.position, berge);

            if(distance > maxSizeDistance)
            {
                distance = maxSizeDistance;
            }

            breathLineInst = Instantiate(breathLinePref);
            breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);
            circleSRenderer = breathLineInst.GetComponent<SpriteRenderer>();
            maxCircleRadius = distance * sizeMult;
            currColor = circleSRenderer.color;
            currColor.a = 1f;
            circleSRenderer.color = currColor;
            lineSpawned = true;
            breathingIn = true;
        }
        else if(bobber.isInWater() && lineSpawned)
        {
            if (breathLineInst.transform.localScale.x < maxCircleRadius && breathingIn)
            {
                lineSpeed = controleSpeed.Evaluate(breathLineInst.transform.localScale.x / maxCircleRadius) * (lineMult / 1000 * distance);
                breathLineInst.transform.localScale += new Vector3(lineSpeed, lineSpeed, lineSpeed);
                breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);
                circleSRenderer.color = currColor;
            }
            else if (breathLineInst.transform.localScale.x >= 0 && !breathingIn)
            {
                lineSpeed = controleSpeedOut.Evaluate(breathLineInst.transform.localScale.x / maxCircleRadius) * (lineMult / 1000 * distance);
                breathLineInst.transform.localScale -= new Vector3(lineSpeed, lineSpeed, lineSpeed);
                breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);
                circleSRenderer.color = currColor;
            }
            else
            {
                currColor.a += 0.2f;
                breathingIn = !breathingIn;
            }
        }
        else if(breathLineInst != null && !bobber.isInWater())
        {
            breathingIn = true; 
            Destroy(breathLineInst);
            lineSpawned = false;
        }
    }
}
