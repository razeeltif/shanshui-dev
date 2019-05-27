using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splashInWater : MonoBehaviour
{

    public GameObject splashPrefab;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "CanneAPeche")
        {
            spawn(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "CanneAPeche")
        {
            spawn(other);
        }
    }

    private void spawn(Collider other)
    {
        GameObject obj = Instantiate(splashPrefab);
        obj.transform.position = new Vector3(other.transform.position.x, this.transform.position.y - 0.02f, other.transform.position.z);
    }
}
