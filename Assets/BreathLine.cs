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
    [SerializeField] float maxCircleRadius;
    public AnimationCurve controleSpeed, controleSpeedOut;
    float lineSpeed;
    [SerializeField] float lineMult;

    Color circleColor;
    bool breathingIn;

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
            circleColor = breathLineInst.GetComponent<SpriteRenderer>().color;
            circleColor.a = 0f;
            lineSpawned = true;
            breathingIn = true;
        }
        else if(bobber.isInWater() && lineSpawned)
        {
            if (breathLineInst.transform.localScale.x < maxCircleRadius && breathingIn)
            {
                lineSpeed = controleSpeed.Evaluate(breathLineInst.transform.localScale.x / maxCircleRadius) * lineMult;
                breathLineInst.transform.localScale += new Vector3(lineSpeed, lineSpeed, lineSpeed);
                breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);
                circleColor.a += 0.001f;
            }
            else if (breathLineInst.transform.localScale.x >= 0 && !breathingIn)
            {
                lineSpeed = controleSpeedOut.Evaluate(breathLineInst.transform.localScale.x / maxCircleRadius) * lineMult;
                breathLineInst.transform.localScale -= new Vector3(lineSpeed, lineSpeed, lineSpeed);
                breathLineInst.transform.position = new Vector3(transform.position.x, waterPlane.transform.position.y, transform.position.z);
                circleColor.a -= 0.001f;
            }
            else
            {
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
