using UnityEngine;

public class Rope : MonoBehaviour
{

    public Rigidbody hook;

    public GameObject linkPrefab;

    public GameObject lineEndPrefab;

    public int links = 7;

    
    public float coefGrowLine = 2;
    // the minimum value of speed the hook needs to reach before growing the line
    public float MinHookVelocity = 40;
    // the maximum value of speed where the line reach its maximum length
    public float MaxHookVelocity = 80;



    private float originalConnectedAnchor;

    private Quaternion originalOrientationRope;

    // keep link references to manipulate their connected anchor and extend the line in the process
    private GameObject[] arrayHooks;

    private void Awake()
    {
        if(MinHookVelocity > MaxHookVelocity)
        {
            Debug.LogError("Min Hook Velocity is greater than Max Hook Velocity");
        }

        originalOrientationRope = this.transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        arrayHooks = new GameObject[links];
        GenerateRope();

        // we get the 2nd line node to set the y axis of the originalConnectedAnchor
        originalConnectedAnchor = arrayHooks[1].GetComponent<HingeJoint>().connectedAnchor.y;
        Debug.Log(originalConnectedAnchor);
    }

    private void Update()
    {
        this.transform.rotation = originalOrientationRope;

        LineLengthVariation();

    }


    private void LineLengthVariation()
    {
        //arrayHooks[links - 1].GetComponent<Rigidbody>().velocity

        float actualVelocity = 0;
        actualVelocity += Mathf.Abs(arrayHooks[links - 1].GetComponent<Rigidbody>().velocity.x);
        actualVelocity += Mathf.Abs(arrayHooks[links - 1].GetComponent<Rigidbody>().velocity.y);
        actualVelocity += Mathf.Abs(arrayHooks[links - 1].GetComponent<Rigidbody>().velocity.z);





        if(actualVelocity > MinHookVelocity)
        {
            float newConnectedAnchor = (actualVelocity - MinHookVelocity) / (MaxHookVelocity - MinHookVelocity) * coefGrowLine - originalConnectedAnchor;
            
            // i = 1 => we still need the very first joint to be set at the tip of the fishrod
            for (int i = 1; i < links; i++)
            {
                arrayHooks[i].GetComponent<HingeJoint>().connectedAnchor = new Vector3(0, newConnectedAnchor, 0);
            }

        }




        Debug.Log(actualVelocity);

    }


    /// <summary>
    /// Generate all nodes of the line
    /// </summary>
    void GenerateRope()
    {

        Rigidbody previousRB = hook;

        // we create <links> joints for the line
        for (int i = 0; i < links; i++)
        {
            GameObject link;
            if (i < links - 1)
            {
                link = Instantiate(linkPrefab);
            }
            // the last link is the fishing float
            else
            {
                link = Instantiate(lineEndPrefab);
            }
            HingeJoint joint = link.GetComponent<HingeJoint>();
            joint.connectedBody = previousRB;

            // the very first joint must be set at the tip of the fishrod
            if(i == 0)
            {
                joint.connectedAnchor = new Vector3(0, 0, 0);
            }

            previousRB = link.GetComponent<Rigidbody>();
            arrayHooks[i] = link;
        }
    }

}
