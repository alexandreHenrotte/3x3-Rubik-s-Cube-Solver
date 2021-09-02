using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowCrossMaker : IMaker
{
    RubiksCube rubiksCube;
    public bool finished = false;

    public YellowCrossMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        while (!HasFinished())
        {
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                if (HasFinished())
                {
                    break;
                }
                else if (Letter_L_Case(faceType))
                {
                    rubiksCube.StartCoroutine(ManipulationAlgorithm(faceType, 1));
                }
                else if (ThreeLineCubesCase(faceType))
                {
                    rubiksCube.StartCoroutine(ManipulationAlgorithm(faceType, 2));
                }

                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }

            rubiksCube.StartCoroutine(ManipulationAlgorithm(Face.FaceType.FRONT, 3)); // not relative to any face type
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }

        finished = true;
    }

    bool Letter_L_Case(Face.FaceType relativeFrontFaceType)
    {
        Face.FaceType relativeLeftUpsideDown = RelativeFaceTypeGetter.GetRelativeRight(relativeFrontFaceType);
        Face.FaceType relativeRearUpsideDown = RelativeFaceTypeGetter.GetRelativeRear(relativeFrontFaceType);

        return rubiksCube.GetCube(relativeLeftUpsideDown, 3, 2).HasColorOnFaceType(Face.Color.YELLOW, Face.FaceType.BOTTOM) &&
               rubiksCube.GetCube(relativeRearUpsideDown, 3, 2).HasColorOnFaceType(Face.Color.YELLOW, Face.FaceType.BOTTOM);
    }

    bool ThreeLineCubesCase(Face.FaceType relativeFrontFaceType)
    {
        Face.FaceType relativeRearUpsideDown = RelativeFaceTypeGetter.GetRelativeRear(relativeFrontFaceType);

        return rubiksCube.GetCube(relativeFrontFaceType, 3, 2).HasColorOnFaceType(Face.Color.YELLOW, Face.FaceType.BOTTOM) &&
               rubiksCube.GetCube(relativeRearUpsideDown, 3, 2).HasColorOnFaceType(Face.Color.YELLOW, Face.FaceType.BOTTOM);
    }

    bool OneMiddleCubeCase(Face.FaceType relativeFrontFaceType)
    {
        return !Letter_L_Case(relativeFrontFaceType) && !ThreeLineCubesCase(relativeFrontFaceType);
    }

    IEnumerator ManipulationAlgorithm(Face.FaceType relativeFrontFaceType, int nbRepetition)
    {
        for (int i = 0; i < nbRepetition; i++)
        {
            string[] movements = { "Ri", "Ui", "Fi", "U", "F", "R" };
            rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType, rubiksCubeUpsideDown: true);
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
    }

    public bool HasFinished()
    {
        List<Tuple<int, int>> yellowCrossCubesIndexes = new List<Tuple<int, int>>()
        {
            { new Tuple<int, int>(1, 2) },
            { new Tuple<int, int>(2, 1) },
            { new Tuple<int, int>(2, 3) },
            { new Tuple<int, int>(3, 2) },
        };

        foreach (Tuple<int, int> cubeIndex in yellowCrossCubesIndexes)
        {
            Cube cube = rubiksCube.GetCube(Face.FaceType.BOTTOM, cubeIndex.Item1, cubeIndex.Item2);
            bool cubeWellPlaced = cube.HasColorOnFaceType(Face.Color.YELLOW, Face.FaceType.BOTTOM);
            if (!cubeWellPlaced)
            {
                return false;
            }
        }

        return true;
    }
}