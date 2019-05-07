using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HapticPulse : MonoBehaviour
{

    public SteamVR_Behaviour_Pose pose;
    public SteamVR_Action_Vibration hapticAction;
    SteamVR_Input_Sources main;

    public float duration = 1;
    public float frequency = 150;
    public float amplitude = 75;

    
    // Start is called before the first frame update
    void Start()
    {
        main = pose.inputSource;
    }

    /*   public SteamVR_Action_Vibration hapticAction;

       IVRSystem device;


       void RumbleController(float duration, float strength)
       {
           StartCoroutine(RumbleControllerRoutine(duration, strength));
       }

       IEnumerator RumbleControllerRoutine(float duration, float strength)
       {
           strength = Mathf.Clamp01(strength);
           float startTime = Time.realtimeSinceStartup;

           while (Time.realtimeSinceStartup - startTime <= duration)
           {
               int valveStrength = Mathf.RoundToInt(Mathf.Lerp(0, 3999, strength));


               device.TriggerHapticPulse((ushort)valveStrength);

               yield return null;
           }
       }*/

    private void Update()
    {
        Pulse(duration, frequency, amplitude, main);
    }


    private void Pulse(float duration, float frequency, float amplitude, SteamVR_Input_Sources source)
    {
        hapticAction.Execute(0, duration, frequency, amplitude, source);
    }
}
