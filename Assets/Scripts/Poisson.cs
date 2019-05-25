using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Poisson : MonoBehaviour
{

    GameSettings settings;

    public int difficulty;
    public float tractionForce;
    public Color color;

    public Transform attachPoint;


    private void OnEnable()
    {
        // abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand += Grab;
        GetComponent<Interactable>().onDetachedFromHand += Release;
    }

    private void OnDisable()
    {
        // de-abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand -= Grab;
        GetComponent<Interactable>().onDetachedFromHand -= Release;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.y < settings.YLimitsBeforeRespawn){
            Destroy(gameObject);
        }
    }

    public void Grab(Hand hand)
    {

        Destroy(GetComponent<FixedJoint>());

    }

    public void Release(Hand hand)
    {
    }

    public Vector3 getPositionAttachPoisson()
    {
        return this.transform.position - attachPoint.position;
    }

}
