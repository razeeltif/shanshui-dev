using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Interacta : MonoBehaviour
{


    [HideInInspector]
    public SteamVR_Skeleton_Poser skeletonPoser;

    protected float blendToPoseTime = 0.1f;
    protected float releasePoseBlendTime = 0.2f;


    private void Awake()
    {
        skeletonPoser = GetComponent<SteamVR_Skeleton_Poser>();
    }


    public void StartDualWield(Hand hand)
    {
        if (skeletonPoser != null && hand.skeleton != null)
        {
            hand.skeleton.BlendToPoser(skeletonPoser, blendToPoseTime);
        }
    }

    public void StopDualWield(Hand hand)
    {
        if (skeletonPoser != null)
        {
            if (hand.skeleton != null)
                hand.skeleton.BlendToSkeleton(releasePoseBlendTime);
        }
    }


}
