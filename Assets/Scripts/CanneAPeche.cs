using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class CanneAPeche : GrablableObject
{




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Grab(SteamVR_Behaviour_Pose pose)
    {
        isGrabbed = true;
    }

    public override void Release(SteamVR_Behaviour_Pose pose)
    {
        isGrabbed = false;
    }

}
