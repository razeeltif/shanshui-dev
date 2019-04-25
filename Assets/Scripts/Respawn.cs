using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    public GameObject prefab;
    //public Vector3 initialPosition;
    public float limits;

    public GameObject monitoredObject;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(monitoredObject.transform.position.y < limits)
        {
            respawn();
        }
    }

    void respawn()
    {
        GameObject obj = Instantiate(prefab);
        //obj.transform.position = initialPosition;
        Destroy(monitoredObject.gameObject.transform.parent.gameObject);
        monitoredObject = obj.GetComponentInChildren<Rigidbody>().gameObject;
    }
}
