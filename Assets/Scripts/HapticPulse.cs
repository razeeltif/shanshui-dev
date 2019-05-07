using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HapticPulse : MonoBehaviour
{

    public Hand hand;
    public SteamVR_Action_Vibration hapticAction;
    SteamVR_Input_Sources main;

    public float duration = 1;
    public float frequency = 150;
    public float amplitude = 75;

        [Range(0,255)]
    public ushort pulse;



    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("PAUTERF");
            hand.TriggerHapticPulse((ushort)pulse);
            Pulse(duration, frequency, amplitude, hand.handType);
        }
    }


    

    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hand.hapticAction.Execute(0, duration, frequency, amplitude, source);
    }


}
