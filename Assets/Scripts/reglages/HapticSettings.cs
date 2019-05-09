using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HapticSettings", menuName = "HapticSettings")]
public class HapticSettings : ScriptableObject
{


    [Header("Grab Settings")]
    public float durationSeconds = 0.05f;
    public float frequency = 50;
    public float amplitude = 0.5f;



    void OnValidate()
    {
        Haptic.durationSeconds = durationSeconds;
        Haptic.frequency = frequency;
        Haptic.amplitude = amplitude;
    }

}
