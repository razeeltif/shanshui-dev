using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEventManager : MonoBehaviour
{
    [SerializeField] ParticleSystem psJump;
    [SerializeField] ParticleSystem psDive;
    [SerializeField] GameObject koi;
    [SerializeField] float particlesYOffset;

    [SerializeField] Color color1;
    [SerializeField] Color color2;


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
        
        psJump.transform.position = new Vector3(koi.transform.position.x, PoissonFishing.instance.fishingManagement.waterPlane.position.y + particlesYOffset, koi.transform.position.z);
        psJump.Play();
    }

    public void SplashOut()
    {
        psDive.transform.position = new Vector3(koi.transform.position.x, PoissonFishing.instance.fishingManagement.waterPlane.position.y + particlesYOffset, koi.transform.position.z);
        psDive.Play();
        ColorManager.CM.CreateFishSplash(color1, color2, this.transform.position);
    }

    public void ColorSplash()
    {
        ColorManager.CM.CreateFishSplash(color1, color2, this.transform.position);
    }

    public void DestroyFish()
    {
        Destroy(gameObject);
    }
}
