using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class RepresentationPositionFishing : MonoBehaviour
{

    [SerializeField]
    private Camera VRCamera;

    public Text debugText;

    public SteamVR_Behaviour_Pose hand;

    public float minDistance;
    public float maxDistance;

    [Range(0, 180)]
    public float maxAngleX;
    [Range(0, 180)]
    public float minAngleY;
    [Range(0, 180)]
    public float maxAngleY;
    [Range(0, 180)]
    public float middleZoneAngle;


    public float distance;
    public float angleX;
    public float angleY;


    public float tolerance;

    private Vector3 PosePosition;

    private bool onCatch;


    private void OnEnable()
    {
        EventManager.StartListening(EventsName.CatchFish, OnCatchFish);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventsName.CatchFish, OnCatchFish);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updatePosePosition();

        if (debugText != null)
        {
            if (checkControllerInZone()){
                spawnNewPoseRightSection();
            }
        }

    }




    private void OnDrawGizmos()
    {

        // zone de distance max d'apparition des poses
        Gizmos.DrawWireSphere(VRCamera.transform.position, maxDistance);

        // zone de distance min d'apparition des poses
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(VRCamera.transform.position, minDistance);

        // pose actuelle
        Gizmos.color = Color.green;

        if(PosePosition != null)
            Gizmos.DrawSphere(PosePosition, tolerance);

    }


    private void OnCatchFish()
    {
        onCatch = true;
    }

    private void angleBetweenControllerAndCamera()
    {
        Vector3 vectorHand = hand.gameObject.transform.position;
        Vector3 vectorCamera = VRCamera.gameObject.transform.position;
        Vector3 vectorDistance = vectorHand - vectorCamera;
        float angleX = Vector3.SignedAngle(vectorDistance, Vector3.forward, Vector3.up);
        float angleY = Vector3.SignedAngle(vectorDistance, Vector3.up, Vector3.forward);
        debugText.text = " X : " + angleX.ToString() + "\n"
            + " Y : " + angleY.ToString();
    }



    private void distanceBetweenHandAndCamera()
    {
        Vector3 vectorHand = hand.gameObject.transform.position;
        Vector3 vectorCamera = VRCamera.gameObject.transform.position;
        Vector3 vectorDistance = vectorHand - vectorCamera;
        debugText.text = vectorDistance.ToString();
    }



    // check si le controler du joueur est dans la zone de pose
    private bool checkControllerInZone()
    {

        bool returnvalue = false;
        // récupération de la distance entre le controller et la position de la pose
        Vector3 vecDist = hand.gameObject.transform.position - PosePosition;
        float dist = vecDist.magnitude;


        // float distance = vectorDistance.magnitude;

        string stringText = " distance : " + dist;

        if(dist < tolerance)
        {
            stringText += "\nDans la zone";
            returnvalue = true;
        }

        debugText.text = stringText;

        return returnvalue;
    }

    private void spawnNewPose()
    {
        // generate new angleX
        angleX = Random.Range(-maxAngleX, maxAngleX);

        // generate new angleY
        angleY = Random.Range(0, maxAngleY);

        distance = Random.Range(minDistance, maxDistance);

        // récupération du vecteur de la Pose
        updatePosePosition();

    }


    private void spawnNewPoseLeftSection()
    {
        angleX = Random.Range(-maxAngleX, -middleZoneAngle);

        // generate new angleY
        angleY = Random.Range(minAngleY, maxAngleY);

        distance = Random.Range(minDistance, maxDistance);

        // récupération du vecteur de la Pose
        updatePosePosition();
    }

    private void spawnNewPoseRightSection()
    {
        angleX = Random.Range(middleZoneAngle, maxAngleX);

        // generate new angleY
        angleY = Random.Range(minAngleY, maxAngleY);

        distance = Random.Range(minDistance, maxDistance);

        // récupération du vecteur de la Pose
        updatePosePosition();
    }

    private void SpawnNewPoseMiddleSection()
    {
        // generate new angleX
        angleX = Random.Range(-middleZoneAngle, middleZoneAngle);

        // generate new angleY
        angleY = Random.Range(0, maxAngleY);

        distance = Random.Range(minDistance, maxDistance);

        // récupération du vecteur de la Pose
        updatePosePosition();
    }

    // update de la position de la pose en fonction du casque
    private void updatePosePosition()
    {
        // récupération du vecteur de la Pose
        Vector3 posXv2 = Quaternion.AngleAxis(angleX + 90, Vector3.up) * Vector3.forward * distance;
        Vector3 posY = Quaternion.AngleAxis(angleY, posXv2) * Vector3.up * distance;
        PosePosition = posY + VRCamera.gameObject.transform.position;
    }


    private void checkZone()
    {
        // récupération de l'angle de l'axe des X entre le casque et le controller
        Vector3 vectorHand = hand.gameObject.transform.position;
        Vector3 vectorCamera = VRCamera.gameObject.transform.position;
        Vector3 vectorDistance = vectorHand - vectorCamera;
        float angleX = Vector3.SignedAngle(vectorDistance, Vector3.forward, Vector3.up);

        // check dans la zone du milieu
        if(angleX > -middleZoneAngle && angleX < middleZoneAngle)
        {
            debugText.text = "Middle " + angleX;
        }

        // check dans la zone de gauche
        else if (angleX < -middleZoneAngle && angleX > -maxAngleX)
        {
            debugText.text = "Left " + angleX;
        }

        // check dans la zone de droite
        else if (angleX > middleZoneAngle && angleX < maxAngleX)
        {
            debugText.text = "Right " + angleX;
        }
        else
        {
            debugText.text = "Dead " + angleX;
        }



    }

    private void OnValidate()
    {
        if (maxDistance < 0) maxDistance = 0;

        if (minDistance < 0) minDistance = 0;
        if (minDistance > maxDistance) minDistance = maxDistance;


        if (distance < 0) distance = 0;
        if (distance > maxDistance) distance = maxDistance;


        if (angleY < minAngleY) angleY = minAngleY;
        if (angleY > maxAngleY) angleY = maxAngleY;

        if (minAngleY < 0) minAngleY = 0;
        if (minAngleY > maxAngleY) minAngleY = maxAngleY;

        if (angleX < -maxAngleX) angleX = -maxAngleX;
        if (angleX > maxAngleX) angleX = maxAngleX;


        if (middleZoneAngle > maxAngleX) middleZoneAngle = maxAngleX;
        if (middleZoneAngle < 0) middleZoneAngle = 0;

        updatePosePosition();

    }

}
