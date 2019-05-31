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
    public Color color2;

    public Transform attachPoint;

    public GameObject splash;

    private bool hasBeenOnBerge = false;

    public float forceAuBoutDuFil = 150;

    private UTimer fishEscapingTimer;

    private float moveFrequence = 0.05f;


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
        fishEscapingTimer = UTimer.Initialize(moveFrequence, this, moveEscapingFish);
        Vector3 force = new Vector3(0, 250, 0) * GetComponent<Rigidbody>().mass;
        GetComponent<Rigidbody>().AddForce(force);
        fishEscapingTimer.start();
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
        fishEscapingTimer.Stop();
    }

    public void Release(Hand hand)
    {
        fishEscapingTimer.start();
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
        if(collider.tag == "water" && hasBeenOnBerge)
        {
            ColorManager.CM.CreateSplash(color, color2, this.transform.position);
        }
    }


    private Vector3 getAngleFishEscaping()
    {
        // generate new angleX
        float angleX = Random.Range(-180, 180);
        float angleY = Random.Range(-180, 180);
        Vector3 posX = Quaternion.AngleAxis(angleX, Vector3.up) * Vector3.forward;
        posX = posX + Quaternion.AngleAxis(angleY, Vector3.right) * Vector3.forward;
        return posX;
    }

    private void moveEscapingFish()
    {
        Vector3 angle = getAngleFishEscaping();
        GetComponent<Rigidbody>().AddForce(angle * forceAuBoutDuFil);
        GetComponent<Rigidbody>().AddTorque(angle * forceAuBoutDuFil / 50);
        fishEscapingTimer.restart();

    }

}
