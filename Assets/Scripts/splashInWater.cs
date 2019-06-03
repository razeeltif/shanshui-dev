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
            if (other.gameObject.GetComponent<Appat>())
            {
                ColorManager.CM.CreateHookSplash(other.gameObject.GetComponent<Appat>().color, other.transform.position);
            }
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
        obj.transform.position = new Vector3(other.transform.position.x, this.transform.position.y - 0.04f, other.transform.position.z);
    }
}
