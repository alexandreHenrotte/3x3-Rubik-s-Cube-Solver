using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgesMaker
{
    public RubiksCube rubiksCube;
    public bool finished = false;

    public EdgesMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work(RubiksCube rubiksCube)
    {
        // Do algorithm

        // SI AUCUNE DE BONNE
        // --> on tourne la face du dessus jusqu'à arriver jusqu'à un des trois cas possibles ci dessous

        // SI DEUX ARRETES SUR FACES QUI SE TOUCHENT
        // --> on met la face relative à gauche des deux faces qui se touchent et on fait l'algorithme
        // --> les quatres sont maintenant bonnes

        // SI DEUX ARRETES SUR FACES OPPOSEES
        // --> on met la face relative directement sur une des deux faces et on fait l'algorithme mais sans le dernier mouvement (pas le U)
        // --> on obtient le cas ou les deux arretes sont sur des faces qui se touchent

        // SI TOUTES LES ARRETES SONT PLACES
        // --> génial

        while (!AllEdgesArePlaced())
        {
            Face.FaceType[] horizontalFaceTypes = { Face.FaceType.FRONT, Face.FaceType.LEFT, Face.FaceType.REAR, Face.FaceType.RIGHT };
            foreach (Face.FaceType faceType in horizontalFaceTypes)
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
        this.finished = true;
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

    bool AllEdgesArePlaced()
    {
        Debug.Log(EdgeOnFaceIsDone(Face.FaceType.FRONT) &&
               EdgeOnFaceIsDone(Face.FaceType.LEFT) &&
               EdgeOnFaceIsDone(Face.FaceType.REAR) &&
               EdgeOnFaceIsDone(Face.FaceType.RIGHT));

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
