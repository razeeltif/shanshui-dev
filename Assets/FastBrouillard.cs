using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastBrouillard : MonoBehaviour
{
    public GameObject[] brouillardD;
    public GameObject[] brouillardG;
    public GameObject[] brouillardFlat;
    private float[] speed;
    private float[] speedflat;

    private void Awake()
    {
        speed = new float[brouillardD.Length];
        speedflat = new float[brouillardFlat.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < brouillardD.Length; i++)
        {
            speed[i] = Random.Range(0.1f, 0.5f);
        }

        for (int i = 0; i < brouillardFlat.Length; i++)
        {
            speedflat[i] = Random.Range(0.5f, 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < brouillardD.Length; i++)
        {
            brouillardD[i].transform.Rotate(0, 0, 2 * speed[i] * Time.deltaTime);
        }
        for (int i = 0; i < brouillardG.Length; i++)
        {
            brouillardG[i].transform.Rotate(0, 0, 2 * -speed[i] * Time.deltaTime);
        }
        for (int i = 0; i < brouillardFlat.Length; i++)
        {
            brouillardFlat[i].transform.Translate(2 * speedflat[i] * Time.deltaTime, 0 ,0);
        }
    }
}
