using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face
{
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
    public FaceType type;
    public GameObject rotatingParent;
    public RotatingAxe rotatingAxe;
    float rotatingSpeed;
    Rotation currentRotation;

    public Face(FaceType type, FaceUpdater faceUpdater, GameObject rotatingParent, RotatingAxe rotatingAxe, float rotatingSpeed = 100f)
    {
        this.type = type;
        this.rotatingParent = rotatingParent;
        this.rotatingAxe = rotatingAxe;
        this.rotatingSpeed = rotatingSpeed;

        faceUpdater.Init(this);
    }

    public void Rotate(bool inversed=false)
    {
        if (currentRotation == null || currentRotation.finished)
        {
            this.currentRotation = new Rotation(this, inversed);
        }  
    }

    public void RotateIfNecessary()
    {
        if (currentRotation == null || currentRotation.finished)
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
}
