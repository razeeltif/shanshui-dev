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

    [Header("In pose")]
    public float inPosebDurationSeconds = 0.05f;
    public float inPoseFrequency = 2000f;
    public float inPoseAmplitude = 1f;

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

    [Header("when line is launched")]
    public float launchLineDurationSeconds = 0.05f;
    public float launchLineFrequency = 50;
    public float launchLineAmplitude = 0.5f;


    void OnValidate()
    {
        Haptic.grabDurationSeconds = grabDurationSeconds;
        Haptic.grabFrequency       = grabFrequency;
        Haptic.grabAmplitude       = grabAmplitude;

        Haptic.inPosebDurationSeconds = inPosebDurationSeconds;
        Haptic.inPoseFrequency        = inPoseFrequency;
        Haptic.inPoseAmplitude        = inPoseAmplitude;

        Haptic.bobberInWaterDurationSeconds = bobberInWaterDurationSeconds;
        Haptic.bobberInWaterFrequency       = bobberInWaterFrequency;
        Haptic.bobberInWaterAmplitude       = bobberInWaterAmplitude;

        Haptic.bobberOutWaterDurationSeconds = bobberOutWaterDurationSeconds;
        Haptic.bobberOutWaterFrequency       = bobberOutWaterFrequency;
        Haptic.bobberOutWaterAmplitude       = bobberOutWaterAmplitude;

        Haptic.vibrationWHenMovingDurationSeconds = vibrationWHenMovingDurationSeconds;
        Haptic.vibrationWHenMovingFrequency       = vibrationWHenMovingFrequency;

        Haptic.launchLineDurationSeconds = launchLineDurationSeconds;
        Haptic.launchLineFrequency       = launchLineFrequency;
        Haptic.launchLineAmplitude       = launchLineAmplitude;

    }

}
