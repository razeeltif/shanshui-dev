using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected override void Grab()
    {
        isGrabbed = true;
    }

    protected override void Release()
    {
        isGrabbed = false;
    }

}
