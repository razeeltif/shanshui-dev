using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    abstract protected void Grab();

    abstract protected void Release();
}
