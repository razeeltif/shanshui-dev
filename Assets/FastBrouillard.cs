using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBrouillard : MonoBehaviour
{
    public GameObject brouillard1, brouillard2, brouillard3;

    public float speedGlobal = 10;
    private float speed1 = 1;
    private float speed2 = 1;
    private float speed3 = 1;

    private Vector3 brouillard1BasePos, brouillard2BasePos, brouillard3BasePos;

    // Start is called before the first frame update
    void Start()
    {
        brouillard1BasePos = brouillard1.transform.position;
        brouillard2BasePos = brouillard2.transform.position;
        brouillard3BasePos = brouillard3.transform.position;

        speed3 = speedGlobal;
        speed2 = speed3 / 2;
        speed1 = speed2 / 2;
    }

    // Update is called once per frame
    void Update()
    {
        brouillard1.transform.Translate(Vector3.right * speed1 * Time.deltaTime);
        brouillard2.transform.Translate(Vector3.left * speed2 * Time.deltaTime);
        brouillard3.transform.Translate(Vector3.right * speed3 * Time.deltaTime);

        if (brouillard1.transform.position.x >= brouillard1BasePos.x + 25 || brouillard1.transform.position.x <= brouillard1BasePos.x - 25)
            speed1 *= -1;

        if (brouillard2.transform.position.x <= brouillard2BasePos.x - 15 || brouillard2.transform.position.x >= brouillard2BasePos.x + 15)
            speed2 *= -1;

        if (brouillard3.transform.position.x >= brouillard3BasePos.x + 10 || brouillard3.transform.position.x <= brouillard3BasePos.x + 10)
            speed3 *= -1;
    }
}
