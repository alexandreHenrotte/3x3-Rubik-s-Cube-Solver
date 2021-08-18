using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceUpdater : MonoBehaviour
{
    public Face.FaceType selectedfaceType = new Face.FaceType();
    private Face face;
    private bool readyToUpdate = false;

    public void Init(Face face)
    {
        this.face = face;
        this.readyToUpdate = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (readyToUpdate && other.CompareTag("Cube"))
        {
            if (!face.cubes.Contains(other.gameObject))
            {
                face.cubes.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (readyToUpdate && other.CompareTag("Cube"))
        {
            face.cubes.Remove(other.gameObject);
        }  
    }
}
