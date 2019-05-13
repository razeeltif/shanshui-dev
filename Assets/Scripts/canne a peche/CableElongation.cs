using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CableElongation : MonoBehaviour, IUseSettings
{

    private const float PLANE_DEFAULT_LENGTH = 5;

    [SerializeField]
    FishRodSettings settings;

    [SerializeField]
    Transform waterPlane;

    [SerializeField]
    Transform bobber;

    [SerializeField]
    GameObject bendyRod;

    [SerializeField]
    Transform tipFishrod;

    public bool inWater = false;


    private void OnEnable()
    {
        settings.AddGameObjectListening(this);
        EventManager.StartListening(EventsName.InWater, OnInWater);
        EventManager.StartListening(EventsName.OutWater, OnOutWater);

    }

    private void OnDisable()
    {
        settings.RemoveGameObjectListening(this);
        EventManager.StopListening(EventsName.InWater, OnInWater);
        EventManager.StopListening(EventsName.OutWater, OnOutWater);

    }

    private void Awake()
    {
    }



    // Start is called before the first frame update
    void Start()
    {
        bendyRod.GetComponent<SpringJoint>().maxDistance = settings.lengthNormalState;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(bobber.transform.position, tipFishrod.transform.position));

        if (!inWater)
        {
            if(getDistanceBerge(bobber.position.z) > 0)
            {
                bendyRod.GetComponent<SpringJoint>().maxDistance = settings.maximumLength;
            }
            else
            {
                bendyRod.GetComponent<SpringJoint>().maxDistance = settings.lengthNormalState;
            }

        }
        else
        {
            float distance = Vector3.Distance(bobber.transform.position, tipFishrod.transform.position);
            if(distance > settings.lengthNormalState && bendyRod.GetComponent<SpringJoint>().maxDistance > distance)
            {
                bendyRod.GetComponent<SpringJoint>().maxDistance = distance;
            }


        }


    }


    private float getDistanceBerge(float Zpos)
    {
        float valZ = PLANE_DEFAULT_LENGTH * waterPlane.localScale.z;
        return Zpos - waterPlane.position.z + valZ;
    }



    void OnInWater()
    {
        inWater = true;
    }

    void OnOutWater()
    {
        inWater = false;
    }

    public void OnModifySettings()
    {
        
    }
}
