using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Camera cam;
    public float chronomax;
    float chrono;
    Vector3 velocity = Vector3.zero;
    public float smoothTime;
    bool moving;
    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray rayTorus = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        Ray rayElse = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.green);

        RaycastHit hitTorus;
        RaycastHit hitElse;

        //Si le RC ne touche pas l'UI et que le chrono n'est pas au max
        if (!Physics.Raycast(rayElse, out hitElse, Mathf.Infinity, ~LayerMask.GetMask("Torus")) && chrono < chronomax)
        {
            chrono += Time.fixedDeltaTime;
        }
        //Si le RC touche pas l'UI, que le chrono est dépassé et qu'un point du torus est touché
        else if (!Physics.Raycast(rayElse, out hitElse, Mathf.Infinity, ~LayerMask.GetMask("Torus")) && chrono >= chronomax && Physics.Raycast(rayTorus, out hitTorus, Mathf.Infinity, LayerMask.GetMask("Torus")))
        {
            target = hitTorus.point;
            moving = true;
            chrono = 0;
        }
        else
        {
            chrono = 0;
        }

        if (moving && transform.position != target)
        {
            this.transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime, 4f);
            transform.LookAt(cam.transform.position);
        }
        else if(moving && transform.position == target)
        {
            moving = false;
        }
    }
}
