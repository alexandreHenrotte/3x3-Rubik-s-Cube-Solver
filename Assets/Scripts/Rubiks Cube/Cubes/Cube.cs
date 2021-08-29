using System;
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

    public List<Face.FaceType> GetFaceTypes()
    {
        List<Face.FaceType> faceTypes = new List<Face.FaceType>();
        foreach (ColorFaceAssociation colorFaceAssociation in colorFaceAssociations)
        {
            faceTypes.Add(colorFaceAssociation.faceType);
        }
        return faceTypes;
    }
}

[Serializable]
public class ColorFaceAssociation
{
    public Face.FaceType faceType;
    public Face.Color color;
}