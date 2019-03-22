using UnityEngine;

public class Rope : MonoBehaviour
{

    public Rigidbody hook;

    public GameObject linkPrefab;

    public GameObject lineEndPrefab;

    public int links = 7;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
    }



    void GenerateRope()
    {

        Rigidbody previousRB = hook;

        for (int i = 0; i < links; i++)
        {
            GameObject link;
            if (i < links - 1)
            {
                link = Instantiate(linkPrefab);
            }
            else
            {
                link = Instantiate(lineEndPrefab);
            }
            HingeJoint joint = link.GetComponent<HingeJoint>();
            joint.connectedBody = previousRB;

            previousRB = link.GetComponent<Rigidbody>();

        }
    }

}
