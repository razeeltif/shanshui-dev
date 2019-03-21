﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public abstract class GrablableObject : MonoBehaviour
{

    protected bool isGrabbed = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    abstract public void Grab(SteamVR_Behaviour_Pose pose);

    abstract public void Release(SteamVR_Behaviour_Pose pose);
}
