using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoiteAppat : MonoBehaviour
{

    /// 3 listes pour la gestion du respawn des appats
    /// ces 3 listes ont la même taille
    // liste des appats présents de base dans la boite
    public Appat[] listeAppats;
    // liste de la position initial dans la boite de chaque appat
    public Transform[] listePositionInitialAppats;
    // liste pour savoir si l'appat numéro i est dans la boite ou non
    private bool[] listeInTheBox;


    private void Awake()
    {
        
        if(listeAppats.Length != listePositionInitialAppats.Length)
        {
            Debug.LogError(this.name + " : listeAppats and listePositionInitialAppats length mismatch. They must have the same length");
        }

        // initialisation de la liste isInTheBox
        listeInTheBox = new bool[listeAppats.Length];
        for (int i = 0; i < listeInTheBox.Length; i++)
        {
            listeInTheBox[i] = true;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //on affilie la boite à l'appat
        for (int i = 0; i < listeAppats.Length; i++)
        {
            listeAppats[i].boiteAppat = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnleverAppat(Appat appat)
    {
        for (int i = 0; i < listeAppats.Length; i++)
        {
            if(appat == listeAppats[i])
            {
                listeInTheBox[i] = false;
            }
        }
    }

    public void RespawnAppat(Appat appat)
    {
        for (int i = 0; i < listeAppats.Length; i++)
        {
            if (appat == listeAppats[i])
            {
                listeInTheBox[i] = true;
                appat.GetComponent<Rigidbody>().isKinematic = true;
                appat.transform.parent = this.transform;
                appat.transform.position = listePositionInitialAppats[i].position;

                // todo : URespawnAnimation
                /*Vector3 initialScale = appat.transform.localScale;
                appat.transform.localScale = Vector3.zero;*/
            }
        }
    }

    public void RespawnBoite()
    {

    }
}
