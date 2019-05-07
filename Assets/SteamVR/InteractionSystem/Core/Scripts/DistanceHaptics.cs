//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Triggers haptic pulses based on distance between 2 positions
//
//=============================================================================

using UnityEngine;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	public class DistanceHaptics : MonoBehaviour
	{
		public Transform firstTransform;
		public Transform secondTransform;

		public AnimationCurve distanceIntensityCurve = AnimationCurve.Linear( 0.0f, 800.0f, 1.0f, 800.0f );
		public AnimationCurve pulseIntervalCurve = AnimationCurve.Linear( 0.0f, 0.01f, 1.0f, 0.0f );

        public float pulse = 800;
        public float nextPulse = 0;

		//-------------------------------------------------
		IEnumerator Start()
		{
			while ( true )
			{
				float distance = Vector3.Distance( firstTransform.position, secondTransform.position );

                Hand hand = GetComponentInParent<Hand>();
                if (hand != null)
                { 
					//pulse = distanceIntensityCurve.Evaluate( distance );
                    hand.TriggerHapticPulse((ushort)pulse);

                    //SteamVR_Controller.Input( (int)trackedObject.index ).TriggerHapticPulse( (ushort)pulse );
				}

				//nextPulse = pulseIntervalCurve.Evaluate( distance );

				yield return new WaitForSeconds( nextPulse );
			}

		}
	}
}