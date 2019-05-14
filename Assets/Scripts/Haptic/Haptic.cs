using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public static class Haptic
{

    static public float grabDurationSeconds = 0.05f;
    static public float grabFrequency = 50;
    static public float grabAmplitude = 0.5f;

    static public float bobberInWaterDurationSeconds = 0.05f;
    static public float bobberInWaterFrequency = 50;
    static public float bobberInWaterAmplitude = 0.5f;

    static public float bobberOutWaterDurationSeconds = 0.05f;
    static public float bobberOutWaterFrequency = 50;
    static public float bobberOutWaterAmplitude = 0.5f;

    static public float vibrationWHenMovingDurationSeconds = 0.01f;
    static public float vibrationWHenMovingFrequency = 200;



    public static void vibrationWHenMoving(Hand hand, float amplitude)
    {
        hand.hapticAction.Execute(0, vibrationWHenMovingDurationSeconds, vibrationWHenMovingFrequency, amplitude, hand.handType);
    }

    public static void GrabObject(Hand hand)
    {
        hand.hapticAction.Execute(0, grabDurationSeconds, grabFrequency, grabAmplitude, hand.handType);
    }

    public static void bobberInWater(Hand hand)
    {
        hand.hapticAction.Execute(0, bobberInWaterDurationSeconds, bobberInWaterFrequency, bobberInWaterAmplitude, hand.handType);
    }

    public static void bobberOutWater(Hand hand)
    {
        hand.hapticAction.Execute(0, bobberOutWaterDurationSeconds, bobberOutWaterFrequency, bobberOutWaterAmplitude, hand.handType);
    }

}
