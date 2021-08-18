using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{
    public static float rotatingSpeed = 400f;

    public enum FaceType
    {
        FRONT,
        REAR,
        LEFT,
        RIGHT,
        BOTTOM,
        UP
    }

    public enum RotatingAxe
    {
        X,
        Y,
        Z
    }

    public List<GameObject> cubes = new List<GameObject>();
    public GameObject rotatingParent;
    public RotatingAxe rotatingAxe;
    Rotation currentRotation;

    public Face(FaceUpdater faceUpdater, GameObject rotatingParent, RotatingAxe rotatingAxe)
    {
        this.rotatingParent = rotatingParent;
        this.rotatingAxe = rotatingAxe;

        faceUpdater.Init(this);
    }

    public void Rotate(RubiksCube rubiksCube, bool inversed=false)
    {
        if (rubiksCube.AllFacesAreStatic())
        {
            this.currentRotation = new Rotation(this, inversed);
        }  
    }

    public void RotateIfNecessary()
    {
        if (RotationFinished())
        {
            return;
        }

        SetParent();
        DoRotation();
    }

    void SetParent()
    {
        foreach (GameObject cube in cubes)
        {
            cube.transform.SetParent(rotatingParent.transform);
        }
    }

    void DoRotation()
    {
        rotatingParent.transform.rotation = Quaternion.RotateTowards(rotatingParent.transform.rotation, currentRotation.quaternionToReach, rotatingSpeed * Time.deltaTime);

        if (rotatingParent.transform.rotation == currentRotation.quaternionToReach)
        {
            currentRotation.finished = true;
        }
    }

    public bool RotationFinished()
    {
        return currentRotation == null || currentRotation.finished;
    }
}
