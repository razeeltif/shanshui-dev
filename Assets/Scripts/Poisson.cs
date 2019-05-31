using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Poisson : MonoBehaviour
{

    public GameSettings settings;

    public int difficulty;
    public float tractionForce;
    public Color color;

    public Transform attachPoint;

    public GameObject splash;

    private bool hasBeenOnBerge = false;


    private void OnEnable()
    {
        // abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand += Grab;
        GetComponent<Interactable>().onDetachedFromHand += Release;

        EventManager.StartListening(EventsName.InWater, OnInWater);
    }

    private void OnDisable()
    {
        // de-abonnement aux events de Interactable pour la détection du grab et du release
        GetComponent<Interactable>().onAttachedToHand -= Grab;
        GetComponent<Interactable>().onDetachedFromHand -= Release;

        EventManager.StopListening(EventsName.InWater, OnInWater);
    }

    // Start is called before the first frame update
    void Start()
    {
        Vector3 force = new Vector3(0, 250, 0) * GetComponent<Rigidbody>().mass;
        GetComponent<Rigidbody>().AddForce(force);
    }

    // Update is called once per frame
    void Update()
    {

        if(PoissonFishing.instance.fishingManagement.getDistanceBerge(this.transform.position.z) < 0)
        {
            hasBeenOnBerge = true;
        }

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

    public void OnInWater()
    {
        // on détache le poisson dès qu'il touche l'eau mais qui soit encore attaché au bobber
        if(GetComponent<FixedJoint>() != null && hasBeenOnBerge)
        {
            Destroy(GetComponent<FixedJoint>());
        }
    }

    public void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "water")
        {
            GameObject spla = Instantiate(splash);
            //spla.GetComponentInChildren<Renderer>().material.SetColor("_Color", color);
            spla.transform.position = new Vector3(this.transform.position.x, PoissonFishing.instance.fishingManagement.waterPlane.transform.position.y, this.transform.position.z);
            spla.GetComponentInChildren<ColorPanel>().UpdateColor(ColorManager.Colors.Blue, 5);
        }
    }

}
