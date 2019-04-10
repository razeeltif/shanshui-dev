using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTest : MonoBehaviour
{


    private void OnEnable()
    {
        EventManager.StartListening(EventsName.InWater, OnInWater);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.InWater, OnInWater);
    }




    // si l'event InWater est Trigger quelque part, cette fonction est appelé
    void OnInWater()
    {
        Debug.Log("PATATE DOUCE PUTAIN");
    }
}
