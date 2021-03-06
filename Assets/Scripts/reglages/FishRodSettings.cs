﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "fishrodSettings", menuName = "fishrodSettings")]
public class FishRodSettings : ScriptableObject
{

    private List<IUseSettings> objectsUsingSettings = new List<IUseSettings>();

    [Header("Fishrod Settings")]
    public float handPosition = 1.96f;
    [Range (0, 100)]
    public int nbSteps = 20;
    public float mass = 6;
    public float drag = 10;
    public float angularDrag = 0.5f;


    [Header("Line Settings")]
    [Range(0, 100)]
    public int totalSegments = 20;
    [Range(0, 100)]
    public float lengthNormalState = 4;
    [Range(0, 100)]
    public float maximumLength = 15;
    [Range(0, 50)]
    public float pullbackForce = 10;
    [Range(0, 100)]
    public float lengthWhenCatchAFish = 1;
    [Range(0, 5)]
    public float width = 0.1f;
    [Range(0, 20)]
    public int rigidity = 6;
    public float SpringNormalState = 500;
    public float DamperNormalState = 100;
    public float MassScaleNormalState = 1;
    public float SpringCatchState = 100;
    public float DamperCatchState = 100;
    public float MassScaleCatchState = 4;


    [Header("Water Settings")]
    public float waterLevel = 1.18f;
    public float viscosity = 20;
    public float bounceDamp = 1;
    public float wave = 0.05f;


    [Header("Fish Settings")]
    public float forceFish = 20;


    [Header("Bobber Settings")]
    public Vector3 BouncyCenterOffset;
    public float BobberMass = 1;
    public float BoobberDrag = 1;
    public float BobberAngularDrag = 1;


    public void AddGameObjectListening(IUseSettings g)
    {
        objectsUsingSettings.Add(g);
    }

    public void RemoveGameObjectListening(IUseSettings g)
    {
        objectsUsingSettings.Remove(g);
    }

    private void callAllIUSingSettings()
    {
        foreach(IUseSettings g in objectsUsingSettings)
        {
            g.OnModifySettings();
        }
    }


    void OnValidate()
    {

        if (mass < 0) mass = 0;
        if (drag < 0) drag = 0;
        if (angularDrag < 0) angularDrag = 0;

        if (SpringNormalState < 0) SpringNormalState = 0;
        if (DamperNormalState < 0) DamperNormalState = 0;
        if (SpringCatchState < 0) SpringCatchState = 0;
        if (DamperCatchState < 0) DamperCatchState = 0;

        if (viscosity < 0) viscosity = 0;
        if (bounceDamp < 0) bounceDamp = 0;
        if (wave < 0) wave = 0;

        if (BobberMass < 0) BobberMass = 0;
        if (BoobberDrag < 0) BoobberDrag = 0;
        if (BobberAngularDrag < 0) BobberAngularDrag = 0;

        callAllIUSingSettings();

    }
}
