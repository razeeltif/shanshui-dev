using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWater : MonoBehaviour
{
    private Color actualColor;
    public Color baseColor = new Color(1f,1f,1f,0f);

    public float speed = 1;
    public float speedInv = 10;
    private float globalAlpha = 0;
    private float targetAlpha = 1;
    private float timeToInverse = 0.1f;

    private float red;
    private float timeStartedRedColorDiff;
    private float timeToCompleteRed = 1;

    private float blue;
    private float timeStartedBlueColorDiff;
    private float timeToCompleteBlue = 1;

    private float green;
    private float timeStartedGreenColorDiff;
    private float timeToCompleteGreen = 1;

    // Start is called before the first frame update
    void Start()
    {  
       GetComponent<Renderer>().material.color = baseColor;
       blue = red = green = 0;
       globalAlpha = 1;
    }

    // Update is called once per frame
    void Update()
    {
        red = Mathf.Lerp(1f, 0f, (Time.time - timeStartedRedColorDiff)/timeToCompleteRed);

        green = Mathf.Lerp(1f, 0f, (Time.time - timeStartedGreenColorDiff)/timeToCompleteGreen);

        blue = Mathf.Lerp(1f, 0f, ((Time.time - timeStartedBlueColorDiff)/timeToCompleteBlue));

        if(targetAlpha == 0)
        {
            globalAlpha = Mathf.MoveTowards(globalAlpha, targetAlpha, speed *   Time.deltaTime);
            
            if(globalAlpha == targetAlpha)
                Destroy(transform.parent.gameObject);
        }
        else
        {
            globalAlpha = Mathf.MoveTowards(globalAlpha, targetAlpha, speedInv * Time.deltaTime);
        }

        if(globalAlpha == targetAlpha && targetAlpha == 1) targetAlpha = 0;

        GetComponent<Renderer>().material.SetFloat("_CutOut", 1-globalAlpha);
        
        actualColor = new Color(red, green, blue);

        GetComponent<Renderer>().material.SetColor("_Color",actualColor);
    }

    internal void UpdateColor(ColorManager.Colors color, float time)
    {
        if(color == ColorManager.Colors.Red || color == ColorManager.Colors.RB ||color == ColorManager.Colors.RG)
        {
            timeStartedRedColorDiff = Time.time;
            timeToCompleteRed = time;
        }

        if(color == ColorManager.Colors.Green || color == ColorManager.Colors.GB || color == ColorManager.Colors.RG)
        {
            timeStartedGreenColorDiff = Time.time;
            timeToCompleteGreen = time;
        }

        if(color == ColorManager.Colors.Blue || color == ColorManager.Colors.GB || color == ColorManager.Colors.RB)
        {
            timeStartedBlueColorDiff = Time.time;
            timeToCompleteBlue = time;
        }

        targetAlpha = 1;
    }
}
