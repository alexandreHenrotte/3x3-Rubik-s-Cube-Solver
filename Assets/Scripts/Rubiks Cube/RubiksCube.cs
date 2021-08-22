using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public Dictionary<Face.FaceType, Face> faces = new Dictionary<Face.FaceType, Face>();

    // Start is called before the first frame update
    void Start()
    {
        // Create logical faces
        faces.Add(Face.FaceType.FRONT, new Face(GameObject.Find("Front face collider").GetComponent<FaceUpdater>(), GameObject.Find("front-center"), Face.RotatingAxe.Y));
        faces.Add(Face.FaceType.REAR, new Face(GameObject.Find("Rear face collider").GetComponent<FaceUpdater>(), GameObject.Find("rear-center"), Face.RotatingAxe.Y));
        faces.Add(Face.FaceType.LEFT, new Face(GameObject.Find("Left face collider").GetComponent<FaceUpdater>(), GameObject.Find("left-center"), Face.RotatingAxe.Z));
        faces.Add(Face.FaceType.RIGHT, new Face(GameObject.Find("Right face collider").GetComponent<FaceUpdater>(), GameObject.Find("right-center"), Face.RotatingAxe.Z));
        faces.Add(Face.FaceType.BOTTOM, new Face(GameObject.Find("Bottom face collider").GetComponent<FaceUpdater>(), GameObject.Find("bottom-center"), Face.RotatingAxe.X));
        faces.Add(Face.FaceType.UP, new Face(GameObject.Find("Up face collider").GetComponent<FaceUpdater>(), GameObject.Find("up-center"), Face.RotatingAxe.X));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Face face in faces.Values)
        {
            face.RotateIfNecessary();
        }
    }

    public GameObject GetCube(Face.FaceType faceType, int rowNumber, int columnNumber)
    {
        Face face = faces[faceType];
        GameObject cube = face.GetCube(rowNumber, columnNumber);
        return cube;
    }

    public void Manipulate(string movement)
    {
        Face.FaceType faceType = new Face.FaceType();
        bool inverted = false;

        switch (movement)
        {
            case "R":
                faceType = Face.FaceType.RIGHT;
                inverted = true;
                break;
            case "Ri":
                faceType = Face.FaceType.RIGHT;
                inverted = false;
                break;
            case "L":
                faceType = Face.FaceType.LEFT;
                inverted = false;
                break;
            case "Li":
                faceType = Face.FaceType.LEFT;
                inverted = true;
                break;
            case "B":
                faceType = Face.FaceType.REAR;
                inverted = true;
                break;
            case "Bi":
                faceType = Face.FaceType.REAR;
                inverted = false;
                break;
            case "D":
                faceType = Face.FaceType.BOTTOM;
                inverted = true;
                break;
            case "Di":
                faceType = Face.FaceType.BOTTOM;
                inverted = false;
                break;
            case "F":
                faceType = Face.FaceType.FRONT;
                inverted = false;
                break;
            case "Fi":
                faceType = Face.FaceType.FRONT;
                inverted = true;
                break;
            case "U":
                faceType = Face.FaceType.UP;
                inverted = false;
                break;
            case "Ui":
                faceType = Face.FaceType.UP;
                inverted = true;
                break;
        }

        Face faceToManipulate = faces[faceType];
        faceToManipulate.Rotate(this, inverted);
        StartCoroutine(ColorMappingUpdater.Update(faceToManipulate, movement));
    }

    public void Shuffle()
    {
        StartCoroutine(ShuffleMechanism.Shuffle(this));
    }

    public bool AllFacesAreStatic()
    {
        foreach (Face face in faces.Values)
        {
            if (!face.RotationFinished())
            {
                return false;
            }
        }

        return true;
    }
}
