using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public List<ColorFaceAssociation> colorFaceAssociations = new List<ColorFaceAssociation>();

    public Face.Color GetColor(Face.FaceType faceType)
    {
        foreach (ColorFaceAssociation colorFaceAssociation in colorFaceAssociations)
        {
            if (colorFaceAssociation.faceType == faceType)
            {
                return colorFaceAssociation.color;
            }
        }
        throw new Exception("The cube has no color for that face");
    }
}

[Serializable]
public class ColorFaceAssociation
{
    public Face.FaceType faceType;
    public Face.Color color;
}