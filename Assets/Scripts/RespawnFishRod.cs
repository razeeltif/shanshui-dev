using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnFishRod : MonoBehaviour
{
    public GameSettings settings;

    public GameObject prefab;

    public GameObject monitoredObject;

    private float scale = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(monitoredObject.transform.position.y < settings.YLimitsBeforeRespawn)
        {
            respawn();
        }
    }

    void respawn()
    {
        Debug.Log("RESPAWN");
        EventManager.TriggerEvent(EventsName.ReleaseFish);
        GameObject obj = Instantiate(prefab);
        Destroy(monitoredObject.gameObject.transform.parent.gameObject);
        monitoredObject = obj.GetComponentInChildren<Rigidbody>().gameObject;
        PoissonFishing.instance.bobber = obj.GetComponentInChildren<Bobber>().transform;
        PoissonFishing.instance.poseFishing.canneAPeche = obj.GetComponentInChildren<Rigidbody>().transform;


    }
    
}
