using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendInterpolation : MonoBehaviour
{

    [SerializeField]
    private int nbStep = 10;


    [SerializeField]
    private GameObject gamePrefab;


    [SerializeField]
    private GameObject fishRod;

    [SerializeField]
    private GameObject BendableFishRod;

    // the reference to the instantiated prefab above
    private GameObject testObject;

    private float ratio;

    public float val = 0;


    

    // Start is called before the first frame update
    void Start()
    {

        testObject = Instantiate(gamePrefab);

    }

    // Update is called once per frame
    void Update()
    {

        testObject.transform.position = new Vector3(fishRod.transform.position.x, fishRod.transform.position.y, fishRod.transform.position.z);
        testObject.transform.position += fishRod.transform.up * val;

        /*
         * ratio = ( (step / nbStep)² * (3 * fishRodScale - step / nbStep) ) / ( 2 * fishRodScale^3 )
         * 
         * position de la canne a peche :
         * 
         * newPosition = 
         * */

    }
}
