using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgesMaker : IMaker
{
    RubiksCube rubiksCube;
    public bool finished = false;

    public EdgesMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        while (!HasFinished())
        {
            Debug.Log("Edges");
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                if (FaceIsInAdjacentCase(faceType))
                {
                    rubiksCube.StartCoroutine(AdjacentCaseAlgorithm(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                    break;
                }
                else if (FaceIsInOppositeCase(faceType))
                {
                    rubiksCube.StartCoroutine(OppositeCaseAlgorithm()); // The opposite case algorithm make the face fall into the adjacent case algorithm
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);

                    rubiksCube.StartCoroutine(AdjacentCaseAlgorithm(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                    break;
                }
            }

            rubiksCube.Manipulate("U");
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }

        finished = true;
    }

    bool FaceIsInAdjacentCase(Face.FaceType faceType)
    {
        return EdgeOnFaceIsDone(faceType) && EdgeOnFaceIsDone(RelativeFaceTypeGetter.GetRelativeRight(faceType));
    }

    IEnumerator AdjacentCaseAlgorithm(Face.FaceType faceType)
    {
        string[] movements = { "R", "U", "Ri", "U", "R", "U", "U", "Ri", "U" };
        rubiksCube.ManipulateMultipleTimes(movements, RelativeFaceTypeGetter.GetRelativeLeft(faceType));
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    bool FaceIsInOppositeCase(Face.FaceType faceType)
    {
        return EdgeOnFaceIsDone(faceType) && EdgeOnFaceIsDone(RelativeFaceTypeGetter.GetRelativeRear(faceType));
    }

    IEnumerator OppositeCaseAlgorithm()
    {
        string[] movements = { "R", "U", "Ri", "U", "R", "U", "U", "Ri" };
        rubiksCube.ManipulateMultipleTimes(movements);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    public bool HasFinished()
    {
        return EdgeOnFaceIsDone(Face.FaceType.FRONT) &&
               EdgeOnFaceIsDone(Face.FaceType.LEFT) &&
               EdgeOnFaceIsDone(Face.FaceType.REAR) &&
               EdgeOnFaceIsDone(Face.FaceType.RIGHT);
    }

    bool EdgeOnFaceIsDone(Face.FaceType faceType)
    {
        try
        {
            bool edgeOnFaceIsDone = rubiksCube.GetCube(faceType, 1, 2).GetColor(faceType) == (Face.Color)faceType &&
                                    rubiksCube.GetCube(faceType, 2, 2).GetColor(faceType) == (Face.Color)faceType;
            return edgeOnFaceIsDone;
        }
        catch // The cube return an error if it doesn't have the asked color (which is possible in runtime)
        {
            return false;
        }
    }
}
