using UnityEngine;
using System;
using System.Collections;


public class CableComponent : MonoBehaviour
{
    #region Class members

#pragma warning disable 0649 
    public Transform cableEnd;
	[SerializeField] private Material cableMaterial;
#pragma warning restore 0649

    // Cable config
    [HideInInspector] public Vector3 cableStartOffset;
    [HideInInspector] public Vector3 cableEndOffset;
    [HideInInspector] public float cableLength = 0.5f;
	[HideInInspector] public int totalSegments = 5;
	[SerializeField] private float segmentsPerUnit = 2f;
	private int segments = 0;
	[HideInInspector] public float cableWidth = 0.1f;

    // Solver config
    [HideInInspector] public int verletIterations = 1;
	[SerializeField] private int solverIterations = 1;

	//[Range(0,3)]
	//[SerializeField] private float stiffness = 1f;

    private Vector3 cableEndPosition;
    private Vector3 cableStartPosition;

	private LineRenderer line;
	private CableParticle[] points;

	#endregion


	#region Initial setup

	void Start()
	{

        // init cable start and end position
        cableStartPosition = this.transform.position + this.transform.right * cableStartOffset.x + this.transform.up * cableStartOffset.y + this.transform.forward * cableStartOffset.z;
        cableEndPosition = cableEnd.position + cableEnd.right * cableEndOffset.x + cableEnd.up * cableEndOffset.y + cableEnd.forward * cableEndOffset.z;


        InitCableParticles();
		InitLineRenderer();
	}

	/**
	 * Init cable particles
	 * 
	 * Creates the cable particles along the cable length
	 * and binds the start and end tips to their respective game objects.
	 */
	void InitCableParticles()
	{
		// Calculate segments to use
		if (totalSegments > 0)
			segments = totalSegments;
		else
			segments = Mathf.CeilToInt (cableLength * segmentsPerUnit);

		Vector3 cableDirection = (cableEndPosition - cableStartPosition).normalized;
		float initialSegmentLength = cableLength / segments;
		points = new CableParticle[segments + 1];

		// Foreach point
		for (int pointIdx = 0; pointIdx <= segments; pointIdx++) {
			// Initial position
			Vector3 initialPosition = cableStartPosition + (cableDirection * (initialSegmentLength * pointIdx));
			points[pointIdx] = new CableParticle(initialPosition);
		}

		// Bind start and end particles with their respective gameobjects
		CableParticle start = points[0];
		CableParticle end = points[segments];
		start.Bind(this.transform, cableStartOffset);
        end.Bind(cableEnd.transform, cableEndOffset);
	}

	/**
	 * Initialized the line renderer
	 */
	void InitLineRenderer()
	{
		line = this.gameObject.AddComponent<LineRenderer>();
        line.startWidth = cableWidth;
        line.endWidth = cableWidth;
        line.positionCount = segments + 1;
		line.material = cableMaterial;
		line.GetComponent<Renderer>().enabled = true;
	}

	#endregion


	#region Render Pass

	void Update()
	{
		RenderCable();
	}

	/**
	 * Render Cable
	 * 
	 * Update every particle position in the line renderer.
	 */
	void RenderCable()
	{
		for (int pointIdx = 0; pointIdx < segments + 1; pointIdx++) 
		{
			line.SetPosition(pointIdx, points [pointIdx].Position);
		}
	}

	#endregion


	#region Verlet integration & solver pass

	void FixedUpdate()
	{
		for (int verletIdx = 0; verletIdx < verletIterations; verletIdx++) 
		{
			VerletIntegrate();
			SolveConstraints();
		}
	}

	/**
	 * Verler integration pass
	 * 
	 * In this step every particle updates its position and speed.
	 */
	void VerletIntegrate()
	{
		Vector3 gravityDisplacement = Time.fixedDeltaTime * Time.fixedDeltaTime * Physics.gravity;
		foreach (CableParticle particle in points) 
		{
			particle.UpdateVerlet(gravityDisplacement);
		}
	}

	/**
	 * Constrains solver pass
	 * 
	 * In this step every constraint is addressed in sequence
	 */
	void SolveConstraints()
	{
		// For each solver iteration..
		for (int iterationIdx = 0; iterationIdx < solverIterations; iterationIdx++) 
		{
			SolveDistanceConstraint();
			SolveStiffnessConstraint();
		}
	}

	#endregion



	#region Solver Constraints

	/**
	 * Distance constraint for each segment / pair of particles
	 **/
	void SolveDistanceConstraint()
	{
		float segmentLength = cableLength / segments;
		for (int SegIdx = 0; SegIdx < segments; SegIdx++) 
		{
			CableParticle particleA = points[SegIdx];
			CableParticle particleB = points[SegIdx + 1];

			// Solve for this pair of particles
			SolveDistanceConstraint(particleA, particleB, segmentLength);
		}
	}
		
	/**
	 * Distance Constraint 
	 * 
	 * This is the main constrains that keeps the cable particles "tied" together.
	 */
	void SolveDistanceConstraint(CableParticle particleA, CableParticle particleB, float segmentLength)
	{
		// Find current vector between particles
		Vector3 delta = particleB.Position - particleA.Position;
		// 
		float currentDistance = delta.magnitude;
		float errorFactor = (currentDistance - segmentLength) / currentDistance;
		
		// Only move free particles to satisfy constraints
		if (particleA.IsFree() && particleB.IsFree()) 
		{
			particleA.Position += errorFactor * 0.5f * delta;
			particleB.Position -= errorFactor * 0.5f * delta;
		} 
		else if (particleA.IsFree()) 
		{
			particleA.Position += errorFactor * delta;
		} 
		else if (particleB.IsFree()) 
		{
			particleB.Position -= errorFactor * delta;
		}
	}

	/**
	 * Stiffness constraint
	 **/
	void SolveStiffnessConstraint()
	{
		float distance = (points[0].Position - points[segments].Position).magnitude;
		if (distance > cableLength) 
		{
			foreach (CableParticle particle in points) 
			{
				SolveStiffnessConstraint(particle, distance);
			}
		}	
	}

	/**
	 * TODO: I'll implement this constraint to reinforce cable stiffness 
	 * 
	 * As the system has more particles, the verlet integration aproach 
	 * may get way too loose cable simulation. This constraint is intended 
	 * to reinforce the cable stiffness.
	 * // throw new System.NotImplementedException ();
	 **/
	void SolveStiffnessConstraint(CableParticle cableParticle, float distance)
	{
	

	}

    #endregion

    #region On Draw Gizmo

    private void OnDrawGizmos()
    {
        Vector3 p;


        // anchor position for the beginning of the line
        Gizmos.color = Color.yellow;
        p = this.transform.position + this.transform.right * cableStartOffset.x + this.transform.up * cableStartOffset.y + this.transform.forward * cableStartOffset.z;
        Gizmos.DrawSphere(p, 0.1f);

        // panchor position for the end of the line
        Gizmos.color = Color.red;
        p = cableEnd.position + cableEnd.right * cableEndOffset.x + cableEnd.up * cableEndOffset.y + cableEnd.forward * cableEndOffset.z;
       Gizmos.DrawSphere(p, 0.1f);

    }


    #endregion

}
