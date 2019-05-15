using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HapticSettings", menuName = "HapticSettings")]
public class HapticSettings : ScriptableObject
{


    [Header("Grab")]
    public float grabDurationSeconds = 0.05f;
    public float grabFrequency = 50;
    public float grabAmplitude = 0.5f;

    [Header("Bobber In Water")]
    public float bobberInWaterDurationSeconds = 0.05f;
    public float bobberInWaterFrequency = 50;
    public float bobberInWaterAmplitude = 0.5f;

    [Header("Bobber Out Water")]
    public float bobberOutWaterDurationSeconds = 0.05f;
    public float bobberOutWaterFrequency = 50;
    public float bobberOutWaterAmplitude = 0.5f;

    [Header("Fishrod vibration when moved")]
    public float vibrationWHenMovingDurationSeconds = 0.01f;
    public float vibrationWHenMovingFrequency = 200;
    public float minimumSpeedToVibrate = 1;
    public float maximumSpeedToFullVibration = 150;


    void OnValidate()
    {
        Haptic.grabDurationSeconds = grabDurationSeconds;
        Haptic.grabFrequency       = grabFrequency;
        Haptic.grabAmplitude       = grabAmplitude;

        Haptic.bobberInWaterDurationSeconds = bobberInWaterDurationSeconds;
        Haptic.bobberInWaterFrequency       = bobberInWaterFrequency;
        Haptic.bobberInWaterAmplitude       = bobberInWaterAmplitude;

        Haptic.bobberOutWaterDurationSeconds = bobberOutWaterDurationSeconds;
        Haptic.bobberOutWaterFrequency       = bobberOutWaterFrequency;
        Haptic.bobberOutWaterAmplitude       = bobberOutWaterAmplitude;

        Haptic.vibrationWHenMovingDurationSeconds = vibrationWHenMovingDurationSeconds;
        Haptic.vibrationWHenMovingFrequency       = vibrationWHenMovingFrequency;
    }

}
