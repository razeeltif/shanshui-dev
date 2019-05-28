using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverAmbiant : MonoBehaviour
{

    Transform leftChild;
    Transform rightChild;

    Transform player;

    public float distance = 2.0f;


    // Start is called before the first frame update
    void Start()
    {
        leftChild = GetComponentsInChildren<Transform>()[1];
        rightChild = GetComponentsInChildren<Transform>()[2];

        player = PoissonFishing.instance.playerPosition;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(player.position.x, this.transform.position.y, this.transform.position.z);
        setLeftRightPosition();
    }

    private void setLeftRightPosition()
    {
        leftChild = GetComponentsInChildren<Transform>()[1];
        rightChild = GetComponentsInChildren<Transform>()[2];

        leftChild.position = new Vector3(this.transform.position.x - distance, this.transform.position.y, this.transform.position.z);
        rightChild.position = new Vector3(this.transform.position.x + distance, this.transform.position.y, this.transform.position.z);

    }


    private void OnDrawGizmos()
    {

        if (leftChild != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(leftChild.position, 0.2f);
        }

        if(rightChild != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(rightChild.position, 0.2f);
        }

    }
}
