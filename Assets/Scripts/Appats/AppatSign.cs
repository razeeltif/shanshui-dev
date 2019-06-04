using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppatSign : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "appat")
        {
            if (GetComponentInParent<Bobber>().actualAppat == null || 
                GetComponentInParent<Bobber>().actualAppat.gameObject != other.gameObject)
            {
                GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "appat")
        {
            GetComponentInChildren<MeshRenderer>().enabled = false;
        }
    }
}
