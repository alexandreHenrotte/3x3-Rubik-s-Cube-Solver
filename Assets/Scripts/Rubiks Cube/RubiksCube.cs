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

    public Cube GetCube(Face.FaceType faceType, int rowNumber, int columnNumber)
    {
        Face face = faces[faceType];
        Cube cube = face.GetCube(rowNumber, columnNumber);
        return cube;
    }

    public void Manipulate(string movementCall)
    {
        if (AllFacesAreStatic())
        {
            Movement movement = Movement.Translate(movementCall);
            Face faceToManipulate = faces[movement.faceType];
            faceToManipulate.Rotate(this, movement.isInverted);
            StartCoroutine(ColorMappingUpdater.Update(faceToManipulate, movement));
        }
    }

    public void Manipulate(string[] movementCalls)
    {
        if (AllFacesAreStatic())
        {
            StartCoroutine(ManipulateMany(movementCalls));
        }
    }

    IEnumerator ManipulateMany(string[] movementCalls)
    {
        foreach (string movementCall in movementCalls)
        {
            Manipulate(movementCall);
            yield return new WaitUntil(() => AllFacesAreStatic());
        }
    }

    public void Solve()
    {
        if (AllFacesAreStatic())
        {
            WhiteCross.Make(this);
        }
    }

    public void Shuffle()
    {
        if (AllFacesAreStatic())
        {
            StartCoroutine(ShuffleMechanism.Shuffle(this));
        }
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

    public void ResetFacePositionning()
    {
        Dictionary<Face.FaceType, Face> newFaces = new Dictionary<Face.FaceType, Face>();
        newFaces.Add(Face.FaceType.FRONT, faces.Single(f => f.Value.rotatingParent.name.Contains("front")).Value);
        newFaces.Add(Face.FaceType.REAR, faces.Single(f => f.Value.rotatingParent.name.Contains("rear")).Value);
        newFaces.Add(Face.FaceType.LEFT, faces.Single(f => f.Value.rotatingParent.name.Contains("left")).Value);
        newFaces.Add(Face.FaceType.RIGHT, faces.Single(f => f.Value.rotatingParent.name.Contains("right")).Value);
        newFaces.Add(Face.FaceType.BOTTOM, faces.Single(f => f.Value.rotatingParent.name.Contains("bottom")).Value);
        newFaces.Add(Face.FaceType.UP, faces.Single(f => f.Value.rotatingParent.name.Contains("up")).Value);

        faces = newFaces;
    }
}
