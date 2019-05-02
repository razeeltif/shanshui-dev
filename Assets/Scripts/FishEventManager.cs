using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEventManager : MonoBehaviour
{
    [SerializeField] ParticleSystem psJump;
    [SerializeField] ParticleSystem psDive;
    [SerializeField] GameObject koi;
    [SerializeField] float particlesYOffset;


    // Start is called before the first frame update
    void Start()
    {
        psJump.Stop();
        psDive.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SplashIn()
    {
        psJump.transform.position = new Vector3(koi.transform.position.x, koi.transform.position.y + particlesYOffset, koi.transform.position.z);
        psJump.Play();
    }

    public void SplashOut()
    {
        psDive.transform.position = new Vector3(koi.transform.position.x, koi.transform.position.y + particlesYOffset, koi.transform.position.z);
        Debug.Log(particlesYOffset);
        Debug.Log(koi.transform.position.y + particlesYOffset);
        psDive.Play();
    }

    public void DestroyFish()
    {
        Destroy(gameObject);
    }
}
