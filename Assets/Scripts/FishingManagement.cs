using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManagement : MonoBehaviour
{


    private bool onCath;

    [SerializeField]
    private Transform bobber;
    [SerializeField]
    private Transform waterPlane;
    [SerializeField]
    private Transform playerPosition;

    private const float PLANE_DEFAULT_LENGTH = 5;

    private Vector3 initialPositionBobber;

    public float initialLength = 5;

    public float difficulty = 4;

    public float coefReduction = 1.5f;


    private void OnEnable()
    {
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StartListening(EventsName.ReleaseFish, OnReleaseFish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
        EventManager.StopListening(EventsName.ReleaseFish, OnReleaseFish);
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnCatchFish()
    {
        onCath = true;
        initialPositionBobber = bobber.position;
    }

    private void OnReleaseFish()
    {
        Debug.Log("RELEASE §!!!");
    }


    private void OnDrawGizmos()
    {
        /*if (onCath)
        {*/
        // vecteur direction joueur - bouchon
        Vector3 player2D = new Vector3(playerPosition.position.x, waterPlane.position.y, playerPosition.position.z);
        Vector3 bobber2D = new Vector3(bobber.position.x, waterPlane.position.y, bobber.position.z);



        Vector3 dir = player2D - bobber2D;
        Vector3 left = new Vector3(dir.z, 0, -dir.x);
        left = left.normalized;
        Vector3 startLine = bobber2D +left * initialLength;
        Vector3 endLine = bobber2D - left * initialLength;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startLine, endLine);


        //}
    }

    private float getDistanceBerge()
    {
        float valZ = PLANE_DEFAULT_LENGTH * waterPlane.localScale.z;
        return waterPlane.position.z - valZ;
    }


}
