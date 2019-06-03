using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTheplayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 player2D = PoissonFishing.instance.fishingManagement.getPlayerPositionFromBerge(Camera.main.transform.position);
        this.transform.LookAt(player2D);
        this.transform.Rotate(0,180,0);
    }

}
