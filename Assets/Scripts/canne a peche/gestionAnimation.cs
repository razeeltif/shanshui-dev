using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gestionAnimation : MonoBehaviour
{
    void DeactivateAnimator()
    {
        GetComponentInChildren<Rigidbody>().isKinematic = false;
        GetComponentInChildren<Collider>().enabled = true;
        GetComponent<Animator>().enabled = false;

        GetComponentInChildren<Bobber>().GetComponent<Rigidbody>().isKinematic = false;
        GetComponentInChildren<Bobber>().GetComponent<Collider>().enabled = true;
    }
}
