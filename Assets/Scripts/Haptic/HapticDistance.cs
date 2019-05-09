using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HapticDistance : MonoBehaviour
{


    //public Transform firstPosition;
    //public Vector3 secondPosition;

    // public float pulseForce = 800;

    public float maximumDistance = 6f;
    public float minTimeBetweenPulse = 0.01f;
    public float maxTimeBetweenPulse = 0.5f;

    public float durationSeconds;
    public float frequency;

    [Range (0,1)]
    public float amplitude = 1;
    

    [HideInInspector]
    public Hand hand;


    // timer 
    private float timer;
    private float currentTime = 0;

    private RepresentationPositionFishing representationPose;


    // Start is called before the first frame update
    void Start()
    {
        representationPose = GetComponent<RepresentationPositionFishing>();
    }

    // Update is called once per frame
    void Update()
    {


        // le timer peux changer au cours du temps, en fonction de la distance entre les 2 objets
        // il faut donc le checker à chaque frame
        timer = getPulseValue();

        if (currentTime > timer)
        {
            Pulse();
            currentTime = 0;
        }
        else
        {
            currentTime += Time.deltaTime;
        }
        
    }

    private void Pulse()
    {

        if (hand != null)
        {
            // float seconds = (float)pulseForce / 1000000f;
 //           hand.hapticAction.Execute(0, seconds, 1f / seconds, 1, hand.handType);
            hand.hapticAction.Execute(0, durationSeconds, frequency, amplitude, hand.handType);
        }
    }

    private float getPulseValue()
    {
        float timeBetweenPulse = maxTimeBetweenPulse;
        float longueur = maximumDistance - representationPose.tolerance;
        float position = Vector3.Distance(hand.transform.position, representationPose.PosePosition);

        if (position < representationPose.tolerance)
        {
            timeBetweenPulse = minTimeBetweenPulse;
        }
        else if (position < maximumDistance)
        {
            float ratio = (position - representationPose.tolerance) / longueur;
            ratio = 1 - ratio;

            timeBetweenPulse = Mathf.Lerp(maxTimeBetweenPulse, minTimeBetweenPulse, ratio);
            amplitude = Mathf.Lerp(0, 1, ratio);
        }
        /*Debug.Log("time : " + timeBetweenPulse);
        Debug.Log("longueur : " + longueur);
        Debug.Log("position : " + position);*/
        return timeBetweenPulse;
    }



}
