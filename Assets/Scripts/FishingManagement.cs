﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManagement : MonoBehaviour
{
    
    public Transform waterPlane;
    /*[SerializeField]
    private Transform playerPosition;*/

    public const float PLANE_DEFAULT_LENGTH = 5;

    public float initialLength = 5;
    public Vector3 initialDirection;

    [Range (1, 10)]
    public int difficulty = 4;

    public float coefReduction = 1.5f;

    public float minimalDistanceFromBerge = 0;

    [HideInInspector]
    public float distanceStep;

    // array stockant les positions de début et fin de chaques lignes
    // lignes du tableau : les positions d'une ligne de difficulté
    // colonne 1 : start d'une ligne
    // colonne 2 : end d'une ligne
    public Vector3[,] fishDifficultyLines;


    // array stockant la position de chaque fishPoint + si elle correspond à la gauche, le milieu au la droite
    // 0 : droite
    // 1 : milieu
    // 2 : gauche
    public DotDirection[] fishPoint;

    // permet de générer les points de déplacement du poisson lorsque celui-ci est ferré
    public void generateFishPoints(Vector3 playerPosition, Vector3 bobber)
    {

        fishDifficultyLines = new Vector3[difficulty, 2];
        fishPoint = new DotDirection[difficulty];

        // vecteur direction joueur - bouchon
        Vector3 player2D = new Vector3(playerPosition.x, waterPlane.position.y, playerPosition.z);
        Vector3 bobber2D = new Vector3(bobber.x, waterPlane.position.y, bobber.z);
        Vector3 initialDirection = player2D - bobber2D;

        // vecteur perpendiculaire vecteur joueur - bouchon
        Vector3 left = new Vector3(initialDirection.z, 0, -initialDirection.x);
        left = left.normalized;

        // le pas entre chaque ligne
        float distanceBobber_PlayerBerge = Vector3.Distance(bobber2D, getPlayerPositionFromBerge(playerPosition));
        distanceStep = distanceBobber_PlayerBerge / (difficulty);

        // on génère un point pour chaque ligne
        for (int i = 0; i < difficulty; i++)
        {
            float nexStep = (distanceStep * i);
            Vector3 posStep = (bobber2D - player2D).normalized * -nexStep;

            // génération des lignes de déplacement du poisson
            Vector3 startLine = bobber2D + posStep + left * (initialLength - (coefReduction * i));
            Vector3 endLine = bobber2D + posStep - left * (initialLength - (coefReduction * i));
            fishDifficultyLines[i, 0] = startLine;
            fishDifficultyLines[i, 1] = endLine;

            // génération d'un point
            float distance = Random.Range(-initialLength + (coefReduction * i), initialLength - (coefReduction * i));
            Vector3 point = bobber2D + posStep + left * distance;

            // si le point généré est dans la berge, on en regénere un autre
            while (getDistanceBerge(point.z) < minimalDistanceFromBerge)
            {
                distance = Random.Range(-initialLength + (coefReduction * i), initialLength - (coefReduction * i));
                point = bobber2D + posStep + left * distance;
            }

            float coef = ((initialLength - coefReduction * i) * 2) / 3;

            // récupération de la zone de la position de la main
            int direction;
            if (distance < -coef / 2) direction = 0; // droite
            else if (distance < coef / 2) direction = 1; // milieu
            else direction = 2; // gauche

            // stockage du point dans le tableau pour une ultilisation ultérieure
            fishPoint[i] = new DotDirection(point, direction);

        }
    }

    private float getDistanceBerge(float Zpos)
    {
        float valZ = PLANE_DEFAULT_LENGTH * waterPlane.localScale.z;
        return Zpos - waterPlane.position.z + valZ;
    }

    public Vector3 getPlayerPositionFromBerge(Vector3 playerPosition)
    {
        float posZ = waterPlane.position.z - PLANE_DEFAULT_LENGTH * waterPlane.localScale.z;
        Vector3 ret = new Vector3(playerPosition.x, waterPlane.position.y, posZ);
        return ret;
    }

    private void clearFishPoints()
    {
        fishDifficultyLines = null;
        fishPoint = null;
    }


    private void OnValidate()
    {

        if (minimalDistanceFromBerge < 0) minimalDistanceFromBerge = 0;

        if (initialLength < 0) initialLength = 0;

        if (coefReduction < 0) coefReduction = 0;

        if (coefReduction > (initialLength / difficulty)) coefReduction = initialLength / difficulty;


    }

    public struct DotDirection
    {
        public Vector3 point;
        public int direction;

        public DotDirection(Vector3 p, int d)
        {
            point = p;
            direction = d;
        }
    }


}
