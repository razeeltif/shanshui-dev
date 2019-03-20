using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ActionsTest : MonoBehaviour
{

    // The type of hand(s) to poll for input. These are either All, Left or Right.
    public SteamVR_Input_Sources handType;

    // Reference to the Teleport action.
    public SteamVR_Action_Boolean teleportAction;

    // Reference to the Grab action.
    public SteamVR_Action_Boolean grabAction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetTeleportDown())
        {
            print("Teleport " + handType);
        }

        if (GetGrab())
        {
            print("Grab " + handType);
        }
    }


    public bool GetTeleportDown()
    {
        return teleportAction.GetStateDown(handType);
    }

    public bool GetGrab()
    {
        return grabAction.GetState(handType);
    }


}
