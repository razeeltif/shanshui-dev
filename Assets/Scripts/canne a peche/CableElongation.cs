using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class CableElongation : MonoBehaviour, IUseSettings
{

    private const float PLANE_DEFAULT_LENGTH = 5;

    [SerializeField]
    FishRodSettings fishrodSettings;

    [SerializeField]
    Transform waterPlane;

    [SerializeField]
    Transform bobber;

    [SerializeField]
    GameObject bendyRod;

    [SerializeField]
    Transform tipFishrod;

    public enum State { lance, inWater, outWater };

    State actualState;

    private void OnEnable()
    {
        fishrodSettings.AddGameObjectListening(this);
        EventManager.StartListening(EventsName.InWater, OnInWater);
        EventManager.StartListening(EventsName.OutWater, OnOutWater);

    }

    private void OnDisable()
    {
        fishrodSettings.RemoveGameObjectListening(this);
        EventManager.StopListening(EventsName.InWater, OnInWater);
        EventManager.StopListening(EventsName.OutWater, OnOutWater);

    }

    private void Awake()
    {
        actualState = State.outWater;
    }



    // Start is called before the first frame update
    void Start()
    {
        waterPlane = GameManager.instance.GetComponent<FishingManagement>().waterPlane;
        bendyRod.GetComponent<SpringJoint>().maxDistance = fishrodSettings.lengthNormalState;
    }

    // Update is called once per frame
    void Update()
    {
        if (getDistanceBerge(bobber.position.z) < 0)
        {
            bendyRod.GetComponent<SpringJoint>().maxDistance = fishrodSettings.lengthNormalState;
            actualState = State.outWater;
        }

        if(bendyRod.GetComponent<SpringJoint>().maxDistance == fishrodSettings.maximumLength)
        {
            Haptic.launchLine(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
        }

        switch (actualState)
        {

            case State.outWater:

                // le bobber est au dessus de l'eau ?
                if (getDistanceBerge(bobber.position.z) > 0)
                {
                    bendyRod.GetComponent<SpringJoint>().maxDistance = fishrodSettings.maximumLength;
                    actualState = State.lance;
                }
                break;


            case State.lance:

                // la canne a peche effectue un mouvement de retrait ?
                if (bendyRod.GetComponent<Rigidbody>().velocity.z < -fishrodSettings.pullbackForce)
                {
                    bendyRod.GetComponent<SpringJoint>().maxDistance = fishrodSettings.lengthNormalState;
                }
                break;


            case State.inWater:

                float distance = Vector3.Distance(bobber.transform.position, tipFishrod.transform.position);
                if (distance > fishrodSettings.lengthNormalState && bendyRod.GetComponent<SpringJoint>().maxDistance > distance)
                {
                    bendyRod.GetComponent<SpringJoint>().maxDistance = distance;
                }
                break;



        }


    }


    private float getDistanceBerge(float Zpos)
    {
        float valZ = PLANE_DEFAULT_LENGTH * waterPlane.localScale.z;
        return Zpos - waterPlane.position.z + valZ;
    }



    void OnInWater()
    {
        actualState = State.inWater;
    }

    void OnOutWater()
    {
        actualState = State.outWater;
    }

    public void OnModifySettings()
    {
        
    }
}
