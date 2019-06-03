using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageSwitch : MonoBehaviour
{

    [SerializeField] Sprite[] images;
    Color currColor;
    SpriteRenderer sRenderer;

    [SerializeField] float chronoMax;
    float chrono;

    private int currentImageIndex = 1;


    // Start is called before the first frame update
    void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        sRenderer.sprite = images[0];
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
            else
            {
                if(currentImageIndex < images.Length)
                {
                    sRenderer.sprite = images[currentImageIndex];
                    currentImageIndex++;
                    chrono = 0;
                }
                else
                {
                    SceneSwitcher.instance.LoadMainScene();
                    this.enabled = false;
                }
            }
        }
    }
}
