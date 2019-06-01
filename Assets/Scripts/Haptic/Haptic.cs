using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public static class Haptic
{

    static public float grabDurationSeconds = 0.05f;
    static public float grabFrequency = 50;
    static public float grabAmplitude = 0.5f;

    static public float inPosebDurationSeconds = 0.05f;
    static public float inPoseFrequency = 2000;
    static public float inPoseAmplitude = 1f;

    static public float bobberInWaterDurationSeconds = 0.05f;
    static public float bobberInWaterFrequency = 50;
    static public float bobberInWaterAmplitude = 0.5f;

    static public float bobberOutWaterDurationSeconds = 0.05f;
    static public float bobberOutWaterFrequency = 50;
    static public float bobberOutWaterAmplitude = 0.5f;

    static public float vibrationWHenMovingDurationSeconds = 0.01f;
    static public float vibrationWHenMovingFrequency = 200;

    static public float launchLineDurationSeconds = 0.1f;
    static public float launchLineFrequency = 450;
    static public float launchLineAmplitude = 0.2f;



    public static void vibrationWHenMoving(Hand firstHand, Hand secondHand, float amplitude)
    {
        firstHand.hapticAction.Execute(0, vibrationWHenMovingDurationSeconds, vibrationWHenMovingFrequency, amplitude, firstHand.handType);
        if(secondHand != null)
        {
            secondHand.hapticAction.Execute(0, vibrationWHenMovingDurationSeconds, vibrationWHenMovingFrequency, amplitude, secondHand.handType);
        }
    }

    public static void InPose(Hand firstHand, Hand secondHand)
    {
        firstHand.hapticAction.Execute(0, inPosebDurationSeconds, inPoseFrequency, inPoseAmplitude, firstHand.handType);
        if (secondHand != null)
        {
            secondHand.hapticAction.Execute(0, inPosebDurationSeconds, inPoseFrequency, inPoseAmplitude, secondHand.handType);
        }
    }

    public static void GrabObject(Hand firstHand)
    {
        firstHand.hapticAction.Execute(0, grabDurationSeconds, grabFrequency, grabAmplitude, firstHand.handType);
    }

    public static void bobberInWater(Hand firstHand, Hand secondHand)
    {
        firstHand.hapticAction.Execute(0, bobberInWaterDurationSeconds, bobberInWaterFrequency, bobberInWaterAmplitude, firstHand.handType);
        if (secondHand != null)
        {
            secondHand.hapticAction.Execute(0, bobberInWaterDurationSeconds, bobberInWaterFrequency, bobberInWaterAmplitude, secondHand.handType);
        }
    }

    public static void bobberOutWater(Hand firstHand, Hand secondHand)
    {
        firstHand.hapticAction.Execute(0, bobberOutWaterDurationSeconds, bobberOutWaterFrequency, bobberOutWaterAmplitude, firstHand.handType);
        if (secondHand != null)
        {
            secondHand.hapticAction.Execute(0, bobberOutWaterDurationSeconds, bobberOutWaterFrequency, bobberOutWaterAmplitude, secondHand.handType);
        }
    }

    public static void launchLine(Hand firstHand, Hand secondHand)
    {
        firstHand.hapticAction.Execute(0, launchLineDurationSeconds, launchLineFrequency, launchLineAmplitude, firstHand.handType);
        if (secondHand != null)
        {
            secondHand.hapticAction.Execute(0, launchLineDurationSeconds, launchLineFrequency, launchLineAmplitude, secondHand.handType);
        }
    }

}
