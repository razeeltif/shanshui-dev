using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestAmbiant : MonoBehaviour
{
    public bool ChangeAxis = false;

    float length = 5;

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = PoissonFishing.instance.playerPosition;
    }

    // Update is called once per frame
    void Update()
    {



        if (ChangeAxis)
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, player.position.z);
        }
        else
        {
            this.transform.position = new Vector3(player.position.x, this.transform.position.y, this.transform.position.z);
        }

        
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;

        Vector3 from;
        Vector3 to;

        if (ChangeAxis)
        {
            from = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - length);
            to = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + length);
        }
        else
        {
            from = new Vector3(this.transform.position.x - length, this.transform.position.y, this.transform.position.z);
            to = new Vector3(this.transform.position.x + length, this.transform.position.y, this.transform.position.z);
        }


        Gizmos.DrawLine(from, to);

        Gizmos.color = Color.yellow;

        Gizmos.DrawSphere(this.transform.position, 0.2f);

    }
}
