using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBrouillard : MonoBehaviour
{
    public GameObject[] brouillardD;
    public GameObject[] brouillardG;
    private float[] speed;

    private void Awake()
    {
        speed = new float[brouillardD.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < brouillardD.Length; i++)
        {
            speed[i] = Random.Range(0.2f, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < brouillardD.Length; i++)
        {
            brouillardD[i].transform.Rotate(0, 0, 3 * speed[i] * Time.deltaTime);
        }
        for (int i = 0; i < brouillardG.Length; i++)
        {
            brouillardG[i].transform.Rotate(0, 0, 3 * -speed[i] * Time.deltaTime);
        }
    }
}
