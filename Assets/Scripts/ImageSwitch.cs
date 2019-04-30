using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSwitch : MonoBehaviour
{
    [SerializeField] Sprite image1;
    [SerializeField] Sprite image2;
    [SerializeField] Sprite image3;
    [SerializeField] Sprite image4;
    Color currColor;
    SpriteRenderer sRenderer;

    [SerializeField] float chronoMax;
    float chrono;


    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.sprite = image1;
        currColor = sRenderer.color;
        currColor.a = 0f;
        sRenderer.color = currColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (chrono < chronoMax)
        {
            //Si moins de chrono alpha, augmenter graduellement l'alpha de l'image
            if (currColor.a <= 1f)
            {
                currColor.a += 0.01f;
                sRenderer.color = currColor;
            }

            chrono++;

        }
        else if (chrono >= chronoMax)
        {
            if (currColor.a >= 0f)
            {
                currColor.a -= 0.01f;
                sRenderer.color = currColor;
                chrono++;
            }
            else if(sRenderer.sprite = image1)
            {
                sRenderer.sprite = image2;
                chrono = 0;
            }
            else if (sRenderer.sprite = image2)
            {
                sRenderer.sprite = image3;
                chrono = 0;
            }
            else if (sRenderer.sprite = image3)
            {
                sRenderer.sprite = image4;
                chrono = 0;
            }
        }
    }
}
