using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PassThroughTipFishrod : MonoBehaviour
{


    private void OnCollisionEnter(Collision collision)
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), collision.collider);
    }

}