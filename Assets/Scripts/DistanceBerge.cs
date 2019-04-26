using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBerge : MonoBehaviour
{

    public Transform berge;
    public Vector3 directionBerge;

    private const float PLANE_DEFAULT_LENGTH = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        float valX = PLANE_DEFAULT_LENGTH * berge.localScale.x;
        float valZ = PLANE_DEFAULT_LENGTH * berge.localScale.z;


        if(this.transform.position.x > (berge.position.x + valX) || this.transform.position.x < (berge.position.x - valX))
        {
            Debug.Log("en dehors de X");
        }

        if (this.transform.position.z > (berge.position.z + valZ) || this.transform.position.z < (berge.position.z - valZ))
        {
            Debug.Log("en dehors de Z");
        }

        if(this.transform.position.z < (berge.position.z - valZ))
        {
            Debug.Log("DANS LA BERGE");
        }

        Debug.Log("Distance de la berge : " + (berge.position.z - valZ));
    }
}
