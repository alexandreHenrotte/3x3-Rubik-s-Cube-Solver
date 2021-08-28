using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public Dictionary<Face.FaceType, Face> faces = new Dictionary<Face.FaceType, Face>();
    public bool readyToManipulate = true;

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

    public void Manipulate(string movementCall, Face.FaceType relativeFrontFace = Face.FaceType.FRONT)
    {
        StartCoroutine(ManipulateRoutine(movementCall, relativeFrontFace));
    }

    public IEnumerator ManipulateRoutine(string movementCall, Face.FaceType relativeFrontFace)
    {
        if (readyToManipulate)
        {
            readyToManipulate = false;

            Movement movement = Movement.Translate(movementCall, relativeFrontFace);
            Face faceToManipulate = faces[movement.faceType];
            faceToManipulate.Rotate(this, movement.isInverted);
            yield return new WaitUntil(() => faceToManipulate.RotationFinished());
            ColorMappingUpdater.Update(faceToManipulate, movement);

            readyToManipulate = true;
        }
    }

    public void Manipulate(string[] movementCalls)
    {
        if (readyToManipulate)
        {
            StartCoroutine(ManipulateMany(movementCalls));
        }
    }

    IEnumerator ManipulateMany(string[] movementCalls)
    {
        foreach (string movementCall in movementCalls)
        {
            Manipulate(movementCall);
            yield return new WaitUntil(() => readyToManipulate);
        }
    }

    public void Solve()
    {
        if (readyToManipulate)
        {
            StartCoroutine(WhiteCross.Make(this));
        }
    }

    public void Shuffle()
    {
        if (readyToManipulate)
        {
            StartCoroutine(ShuffleMechanism.Shuffle(this));
        }
    }
}