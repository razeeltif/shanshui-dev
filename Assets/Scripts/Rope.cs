using UnityEngine;

public class Rope : MonoBehaviour
{

    public Rigidbody hook;

    public GameObject linkPrefab;

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
            GameObject link = Instantiate(linkPrefab, transform);
            HingeJoint joint = link.GetComponent<HingeJoint>();
            joint.connectedBody = hook;

            previousRB = link.GetComponent<Rigidbody>();
        }
    }

}
