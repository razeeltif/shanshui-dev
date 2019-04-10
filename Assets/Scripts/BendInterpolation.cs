using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendInterpolation : MonoBehaviour
{
#pragma warning disable 0649

    // nombre d'éléments bendable de la canne à peche (correspondra aux bones de la canne par la suite)
    [SerializeField]
    private int nbStep = 10;

    [SerializeField]
    private GameObject[] fishrodBones;

    [SerializeField]
    private GameObject stepPrefab;
    private GameObject[] stepPrefabArray;


    [SerializeField]
    private GameObject fishRod;

    [SerializeField]
    private GameObject BendableFishRod;

    private float ratio;

    [SerializeField]
    private bool debug = false;

#pragma warning restore 0649




    // Start is called before the first frame update
    void Start()
    {

        if(fishrodBones.Length != nbStep)
        {
            Debug.LogError("the number of steps and the number of fishrodBones don't match\n" +
                            "debug mode automaticaly enabled");
            debug = true;
        }

        stepPrefabArray = new GameObject[nbStep];

        // instanciation des éléments bendables
        for (int i = 0; i < nbStep; i++)
        {
            stepPrefabArray[i] = Instantiate(stepPrefab);
        }

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 rigidRodPosition = fishRod.transform.position;
        Vector3 bendyRodPosition = BendableFishRod.transform.position;

        for (int step = 0; step < nbStep; step++)
        {

            // offset de la position de l'élément bendable par rapport au centre de la canne
            float val = fishRod.transform.localScale.y - step * (fishRod.transform.localScale.y * 2 / (nbStep-1));


            // -- utilisation des coordonnées de rigidRod et bendyRod pour calculer l'interpolation de la canne -- //

            // longueur de la canne
            float L = fishRod.transform.localScale.y * 2;
            // step du calcul du ratio
            float x = ((step) / (float)nbStep) * L;
            // calcul du ratio entre la rigidRod et la bendyRod
            ratio = (Mathf.Pow(x, 2) * (3 * L - x)) / (2 * Mathf.Pow(L, 3));

            // 
            Vector3 rigid = rigidRodPosition + fishRod.transform.up * val;
            Vector3 bend = bendyRodPosition + BendableFishRod.transform.up * val;

            // on défini la nouvelle position du prochain step en appliquant le ratio calculé ci-dessus
            Vector3 newPosition = (rigid) + ((bend) - (rigid)) * ratio;

            if (debug)
            {
                // on déplace le step vers la position calculée
                stepPrefabArray[step].transform.position = newPosition;
            }
            else
            {
                fishrodBones[step].transform.position = newPosition;
            }
        }
       

    }
}
