using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public static class Haptic
{

    static public float durationSeconds = 0.05f;
    static public float frequency = 50;
    static public float amplitude = 0.5f;





    public static void GrabObject(Hand hand)
    {
        hand.hapticAction.Execute(0, durationSeconds, frequency, amplitude, hand.handType);
    }


}
