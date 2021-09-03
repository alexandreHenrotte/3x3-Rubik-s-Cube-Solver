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

    public bool HasColor(Face.Color color)
    {
        foreach (ColorFaceAssociation colorFaceAssociation in colorFaceAssociations)
        {
            if (colorFaceAssociation.color == color)
            {
                return true;
            }
        }
        return false;
    }

    public Face.FaceType GetColorFaceType(Face.Color color)
    {
        foreach (ColorFaceAssociation colorFaceAssociation in colorFaceAssociations)
        {
            if (colorFaceAssociation.color == color)
            {
                return colorFaceAssociation.faceType;
            }
        }
        throw new Exception("The cube has no " + color);
    }

    public bool HasColorOnFaceType(Face.Color color, Face.FaceType faceType)
    {
        if (HasColor(color) && GetColorFaceType(color) == faceType)
        {
            return true;
        }
        return false;
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

    public bool IsPlaced()
    {
        foreach (ColorFaceAssociation colorFaceAssociation in colorFaceAssociations)
        {
            Face.Color faceStaticColor = (Face.Color)colorFaceAssociation.faceType;
            Face.Color cubeColor = colorFaceAssociation.color;
            if (faceStaticColor != cubeColor)
            {
                return false;
            }
        }
        return true;
    }
}

[Serializable]
public class ColorFaceAssociation
{
    public Face.FaceType faceType;
    public Face.Color color;
}