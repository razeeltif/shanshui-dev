using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSwitch : MonoBehaviour
{
    [SerializeField] Sprite image1;
    [SerializeField] Sprite image2;
    [SerializeField] Sprite image3;
    Sprite currImage;
    Color currColor;

    float chronoMax;
    float chronoAlpha;
    float chrono;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (chrono < chronoMax)
        {
            //Si moins de chrono alpha, augmenter graduellement l'alpha de l'image
            if (chrono < chronoAlpha)
            {

            }
        }
    }

    void ImageChange(Sprite currImage, Sprite newImage)
    {      
        currImage = newImage;
    }
}
