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
        EventManager.TriggerEvent(EventsName.ReleaseFish);
        EventManager.TriggerEvent(EventsName.OutWater);

        if (PoissonFishing.instance.bobber.GetComponent<Bobber>().actualAppat != null)
            PoissonFishing.instance.bobber.GetComponent<Bobber>().detachAppat();

        GameObject obj = Instantiate(prefab);
        Destroy(monitoredObject.gameObject.transform.parent.gameObject);
        monitoredObject = obj.GetComponentInChildren<Rigidbody>().gameObject;

        obj.GetComponentInChildren<Rigidbody>().isKinematic = true;
        obj.GetComponentInChildren<Collider>().enabled = false;
        obj.GetComponent<Animator>().enabled = true;
        obj.GetComponent<Animator>().Play("growFishrod", -1, 0f);

        obj.GetComponentInChildren<Bobber>().GetComponent<Rigidbody>().isKinematic = true;
        obj.GetComponentInChildren<Bobber>().GetComponent<Collider>().enabled = false;

        PoissonFishing.instance.bobber = obj.GetComponentInChildren<Bobber>().transform;
        PoissonFishing.instance.poseFishing.canneAPeche = obj.GetComponentInChildren<Rigidbody>().transform;
    }

}
