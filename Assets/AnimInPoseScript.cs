using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimInPoseScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void VibrationInPose()
    {
        Haptic.InPose(GameManager.instance.firstHandHoldingThis, GameManager.instance.secondHandHoldingThis);
    }
}
