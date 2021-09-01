using System;
using UnityEngine;

public class ColorMappingUpdater
{
    /*
     * Update the facetype of each color of all the cubes in the manîpulated face based on the movement
    */
    public bool finished = false;

    public void Update(Face faceManipulated, Movement movement)
    {
        foreach (GameObject cube in faceManipulated.GetAllCubes())
        {
            for (int i = 0; i < cube.GetComponent<Cube>().colorFaceAssociations.Count; i++)
            {
                ColorFaceAssociation colorFaceAssociation = cube.GetComponent<Cube>().colorFaceAssociations[i];
                colorFaceAssociation.faceType = NewFaceColorMapping(colorFaceAssociation.faceType, movement);
            }
        }
        finished = true;
    }

    Face.FaceType NewFaceColorMapping(Face.FaceType oldFaceType, Movement movement)
    {
        Face.FaceType[] mapping = new Face.FaceType[4];
        if (movement.faceType == Face.FaceType.UP)
        {
            mapping = new Face.FaceType[] { Face.FaceType.LEFT, Face.FaceType.REAR, Face.FaceType.RIGHT, Face.FaceType.FRONT };
        }
        else if (movement.faceType == Face.FaceType.BOTTOM)
        {
            mapping = new Face.FaceType[] { Face.FaceType.FRONT, Face.FaceType.RIGHT, Face.FaceType.REAR, Face.FaceType.LEFT };
        }
        else if (movement.faceType == Face.FaceType.RIGHT)
        {
            mapping = new Face.FaceType[] { Face.FaceType.BOTTOM, Face.FaceType.FRONT, Face.FaceType.UP, Face.FaceType.REAR };
        }
        else if (movement.faceType == Face.FaceType.LEFT)
        {
            mapping = new Face.FaceType[] { Face.FaceType.REAR, Face.FaceType.UP, Face.FaceType.FRONT, Face.FaceType.BOTTOM };
        }
        else if (movement.faceType == Face.FaceType.FRONT)
        {
            mapping = new Face.FaceType[] { Face.FaceType.LEFT, Face.FaceType.UP, Face.FaceType.RIGHT, Face.FaceType.BOTTOM };
        }
        else if (movement.faceType == Face.FaceType.REAR)
        {
            mapping = new Face.FaceType[] { Face.FaceType.BOTTOM, Face.FaceType.RIGHT, Face.FaceType.UP, Face.FaceType.LEFT };
        }

        int offset = movement.isInverted ? -1 : 1;
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
