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
        FaceUpdater[] faceUpdaters = GameObject.FindObjectsOfType<FaceUpdater>();
        // Create logical faces
        faces.Add(Face.FaceType.FRONT, new Face(GameObject.Find("Front face collider").GetComponent<FaceUpdater>(), GameObject.Find("front-center"), Face.RotatingAxe.Y, false));
        faces.Add(Face.FaceType.REAR, new Face(GameObject.Find("Rear face collider").GetComponent<FaceUpdater>(), GameObject.Find("rear-center"), Face.RotatingAxe.Y, true));
        faces.Add(Face.FaceType.LEFT, new Face(GameObject.Find("Left face collider").GetComponent<FaceUpdater>(), GameObject.Find("left-center"), Face.RotatingAxe.Z, false));
        faces.Add(Face.FaceType.RIGHT, new Face(GameObject.Find("Right face collider").GetComponent<FaceUpdater>(), GameObject.Find("right-center"), Face.RotatingAxe.Z, true));
        faces.Add(Face.FaceType.BOTTOM, new Face(GameObject.Find("Bottom face collider").GetComponent<FaceUpdater>(), GameObject.Find("bottom-center"), Face.RotatingAxe.X, true));
        faces.Add(Face.FaceType.UP, new Face(GameObject.Find("Up face collider").GetComponent<FaceUpdater>(), GameObject.Find("up-center"), Face.RotatingAxe.X, false));
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Face face in faces.Values)
        {
            face.RotateIfNecessary();
        }
    }
    public Face.Color GetCubeColor(Face.FaceType faceType, int rowNumber, int columnNumber)
    {
        Cube cube = GetCube(faceType, rowNumber, columnNumber).GetComponent<Cube>();
        Face.Color color = cube.GetColor(faceType);
        return color;
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
        bool inverted = movement.Contains("i");

        switch (movement)
        {
            case "R":
                faceType = Face.FaceType.RIGHT;
                break;
            case "Ri":
                faceType = Face.FaceType.RIGHT;
                break;
            case "L":
                faceType = Face.FaceType.LEFT;
                break;
            case "Li":
                faceType = Face.FaceType.LEFT;
                break;
            case "B":
                faceType = Face.FaceType.REAR;
                break;
            case "Bi":
                faceType = Face.FaceType.REAR;
                break;
            case "D":
                faceType = Face.FaceType.BOTTOM;
                break;
            case "Di":
                faceType = Face.FaceType.BOTTOM;
                break;
            case "F":
                faceType = Face.FaceType.FRONT;
                break;
            case "Fi":
                faceType = Face.FaceType.FRONT;
                break;
            case "U":
                faceType = Face.FaceType.UP;
                break;
            case "Ui":
                faceType = Face.FaceType.UP;
                break;
        }

        Face faceToManipulate = faces[faceType];
        faceToManipulate.Rotate(this, inverted);
        StartCoroutine(ColorMappingUpdater.Update(faceToManipulate, movement));
    }

    public void Solve()
    {
        SolveRubiksCube.Start(this);
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

    public void ChangeRelativeFacePositionning(Face.FaceType newFrontFace)
    {
        if (newFrontFace == Face.FaceType.FRONT)
        {
            Debug.Log("Face positionning is already set as asked");
        }
        else if (newFrontFace == Face.FaceType.UP || newFrontFace == Face.FaceType.BOTTOM)
        {
            throw new Exception($"Face type \"{newFrontFace}\" face cannot be set as new front face");
        }
        else
        {
            // Calculate offset between each face type
            Face.FaceType[] relativeFaceTypes = { Face.FaceType.FRONT, Face.FaceType.LEFT, Face.FaceType.REAR, Face.FaceType.RIGHT }; // UP and BOTTOM are not relatives
            int offset = Array.IndexOf(relativeFaceTypes, newFrontFace);

            // Create new dictionnary for future faces
            Dictionary<Face.FaceType, Face> newFaces = new Dictionary<Face.FaceType, Face>();
            newFaces.Add(Face.FaceType.BOTTOM, faces[Face.FaceType.BOTTOM]);
            newFaces.Add(Face.FaceType.UP, faces[Face.FaceType.UP]);

            // Get the new faces and adapt their face updaters
            for (int i = 0; i < relativeFaceTypes.Length; i++)
            {
                Face.FaceType i_FaceType = relativeFaceTypes[i];
                Face.FaceType newFaceType = relativeFaceTypes[(i + offset) % relativeFaceTypes.Length];
                Face oldFace = faces[i_FaceType];
                Face newFace = faces[newFaceType];
                newFace.faceUpdater = oldFace.faceUpdater;
                newFaces.Add(i_FaceType, newFace);
            }

            // Set new faces
            faces = newFaces;
        }
    }
}
