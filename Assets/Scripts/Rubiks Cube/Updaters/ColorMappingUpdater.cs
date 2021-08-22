using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorMappingUpdater
{
    /*
     * Update the facetype of each color of all the cubes in the manîpulated face based on the movement
    */
    public static IEnumerator Update(Face faceManipulated, string movement)
    {
        yield return new WaitUntil(() => faceManipulated.RotationFinished());
        foreach (GameObject cube in faceManipulated.GetCubes())
        {
            for (int i = 0; i < cube.GetComponent<Cube>().colorFaceAssociations.Count; i++)
            {
                ColorFaceAssociation colorFaceAssociation = cube.GetComponent<Cube>().colorFaceAssociations[i];
                colorFaceAssociation.faceType = NewFaceColorMapping(colorFaceAssociation.faceType, movement);
            }
        }
    }
    static Face.FaceType NewFaceColorMapping(Face.FaceType oldFaceType, string movement)
    {
        Face.FaceType[] mapping = new Face.FaceType[4];
        if (movement.Contains("U"))
        {
            mapping = new Face.FaceType[] { Face.FaceType.LEFT, Face.FaceType.REAR, Face.FaceType.RIGHT, Face.FaceType.FRONT };
        }
        else if (movement.Contains("D"))
        {
            mapping = new Face.FaceType[] { Face.FaceType.FRONT, Face.FaceType.RIGHT, Face.FaceType.REAR, Face.FaceType.LEFT };
        }
        else if (movement.Contains("R"))
        {
            mapping = new Face.FaceType[] { Face.FaceType.BOTTOM, Face.FaceType.FRONT, Face.FaceType.UP, Face.FaceType.REAR };
        }
        else if (movement.Contains("L"))
        {
            mapping = new Face.FaceType[] { Face.FaceType.REAR, Face.FaceType.UP, Face.FaceType.FRONT, Face.FaceType.BOTTOM };
        }
        else if (movement.Contains("F"))
        {
            mapping = new Face.FaceType[] { Face.FaceType.LEFT, Face.FaceType.UP, Face.FaceType.RIGHT, Face.FaceType.BOTTOM };
        }
        else if (movement.Contains("B"))
        {
            mapping = new Face.FaceType[] { Face.FaceType.BOTTOM, Face.FaceType.RIGHT, Face.FaceType.UP, Face.FaceType.LEFT };
        }

        int offset = movement.Contains("i") ? -1 : 1;
        Face.FaceType newFaceType = new Face.FaceType();
        int index_oldFace = Array.FindIndex(mapping, faceType => faceType == oldFaceType);
        if (index_oldFace != -1)
        {
            int indexNewFace = (index_oldFace + offset) % mapping.Length;
            if (indexNewFace >= 0)
            {
                newFaceType = mapping[indexNewFace];
            }
            else
            {
                newFaceType = mapping[mapping.Length + indexNewFace];
            }
        }
        else
        {
            newFaceType = oldFaceType;
        }

        return newFaceType;
    }
}
