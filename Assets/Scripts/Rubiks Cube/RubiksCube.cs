using System;
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

    public Cube GetCube(Face.FaceType faceType, int rowNumber, int columnNumber)
    {
        Face face = faces[faceType];
        Cube cube = face.GetCube(rowNumber, columnNumber);
        return cube;
    }

    public void Manipulate(string movementCall, Face.FaceType relativeFrontFace = Face.FaceType.FRONT, bool rubiksCubeUpsideDown = false)
    {
        if (readyToManipulate)
        {
            StartCoroutine(ManipulateRoutine(movementCall, relativeFrontFace, rubiksCubeUpsideDown));
        }
    }

    IEnumerator ManipulateRoutine(string movementCall, Face.FaceType relativeFrontFace = Face.FaceType.FRONT, bool rubiksCubeUpsideDown = false)
    {
        readyToManipulate = false;

        // Rotation
        Movement movement = Movement.Translate(movementCall, relativeFrontFace, rubiksCubeUpsideDown);
        Face faceToManipulate = faces[movement.faceType];
        faceToManipulate.Rotate(this, movement.isInverted);
        yield return new WaitUntil(() => faceToManipulate.RotationFinished());

        // Color Mapping
        ColorMappingUpdater colorMappingUpdater = new ColorMappingUpdater();
        colorMappingUpdater.Update(faceToManipulate, movement);
        yield return new WaitUntil(() => colorMappingUpdater.finished);

        // Small delay to protect an other manipulation to start before the finish of the current manipulation
        yield return new WaitForSeconds(0.05f);

        readyToManipulate = true;
    }

    public void ManipulateMultipleTimes(string[] movementCalls, Face.FaceType relativeFrontFace = Face.FaceType.FRONT, bool rubiksCubeUpsideDown = false)
    {
        if (readyToManipulate)
        {
            StartCoroutine(ManipulateMultipleTimesRoutine(movementCalls, relativeFrontFace, rubiksCubeUpsideDown));
        }
    }

    IEnumerator ManipulateMultipleTimesRoutine(string[] movementCalls, Face.FaceType relativeFrontFace = Face.FaceType.FRONT, bool rubiksCubeUpsideDown = false)
    {
        if (readyToManipulate)
        {
            foreach (string movementCall in movementCalls)
            {
                Manipulate(movementCall, relativeFrontFace, rubiksCubeUpsideDown);
                yield return new WaitUntil(() => readyToManipulate);
            }
        }
    }

    public void Solve()
    {
        if (readyToManipulate && !IsFinished())
        {
            StartCoroutine(Solver.Start(this));
        }
    }

    public void Shuffle()
    {
        if (readyToManipulate)
        {
            StartCoroutine(ShuffleMechanism.Shuffle(this));
        }
    }

    public IEnumerator Test()
    {
        if (readyToManipulate)
        {
            //string[] movements = { "Ui", "Li", "U", "L", "U", "F", "Ui", "Fi" };
            string[] movements = new string[] { "U", "R", "Ui", "Ri", "Ui", "Fi", "U", "F" };
            ManipulateMultipleTimes(movements, Face.FaceType.RIGHT, true);
            yield return new WaitUntil(() => readyToManipulate);
        }
    }

    bool IsFinished()
    {
        foreach (Face.FaceType faceType in Enum.GetValues(typeof(Face.FaceType)))
        {
            try
            {
                foreach (Row row in faces[faceType].rows)
                {
                    foreach (GameObject cube in row.cubes)
                    {
                        if (cube.GetComponent<Cube>().GetColor(faceType) != (Face.Color)faceType)
                        {
                            return false;
                        }
                    }
                }
            }
            catch // The cube return an error if it doesn't have the asked color (which is possible in runtime)
            {
                return false;
            }
        }
        return true;
    }
}