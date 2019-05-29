using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gameSettings", menuName = "gameSettings")]
public class GameSettings : ScriptableObject
{

    private List<IUseSettings> objectsUsingSettings = new List<IUseSettings>();

    [Header("Timers for fishing (in seconds)")]
    public float timeBeforeShoeFish = 2f;
    public float RandomOnTimeBeforeShoe = 1f;
    public float TimeBeforeRelease = 30f;


    public float YLimitsBeforeRespawn = -4f;

    public float YFishrodLimitsBeforeRespawn = -2f;


    public void AddGameObjectListening(IUseSettings g)
    {
        objectsUsingSettings.Add(g);
    }

    public void RemoveGameObjectListening(IUseSettings g)
    {
        objectsUsingSettings.Remove(g);
    }

    private void callAllIUSingSettings()
    {
        foreach (IUseSettings g in objectsUsingSettings)
        {
            g.OnModifySettings();
        }
    }

    void OnValidate()
    {
        if (timeBeforeShoeFish < 0)
        {
            timeBeforeShoeFish = 0;
        }

        if (RandomOnTimeBeforeShoe < 0)
        {
            RandomOnTimeBeforeShoe = 0;
        }

        if (TimeBeforeRelease < 0)
        {
            TimeBeforeRelease = 0;
        }

        if (timeBeforeShoeFish - RandomOnTimeBeforeShoe < 0)
        {
            Debug.LogError("Time Before Shoe Fish Randomness in " + this.name + " must be smaller or egual to Time Before Shoe Fish");
        }
    }
}
