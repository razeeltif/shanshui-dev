using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Hook : MonoBehaviour
{

    public float levelOfWater = -0.32f;
    public float magnetInWater = 1f;

    private Rigidbody rb;
    private float originalDrag;
    private float mass;
    private bool underWater = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        mass = rb.mass;
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        checkIfUnderWater();

        if (underWater)
        {
            rb.drag = originalDrag * magnetInWater;
            rb.mass = mass * magnetInWater;
        }
        else
        {
            rb.drag = originalDrag;
            rb.mass = mass;
        }



    }

    private void checkIfUnderWater()
    {
        underWater = (this.transform.position.y < levelOfWater);
    }

}
