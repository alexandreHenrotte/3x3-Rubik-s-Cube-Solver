using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public List<ColorFaceAssociation> colorFaceAssociations = new List<ColorFaceAssociation>(); 
}

[Serializable]
public class ColorFaceAssociation
{
    public Face.FaceType faceType;
    public Face.Color color;
}