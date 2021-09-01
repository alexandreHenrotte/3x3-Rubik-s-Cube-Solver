using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SecondCrownMaker : IMaker
{
    RubiksCube rubiksCube;
    public bool finished = false;

    public SecondCrownMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        Debug.Log("Second crown ");
        while (!HasFinished())
        {
            while (!SituationIsBlocked())
            {
                foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
                {
                    rubiksCube.StartCoroutine(PlaceCubes(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }

                rubiksCube.Manipulate("U", rubiksCubeUpsideDown: true);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }

            rubiksCube.StartCoroutine(UnblockSituation());
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }

        finished = true;
    }

    bool SituationIsBlocked()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            if (!rubiksCube.GetCube(faceType, 3, 2).HasColor(Face.Color.YELLOW))
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator PlaceCubes(Face.FaceType relativeFrontFaceType)
    {
        Cube cube = rubiksCube.GetCube(relativeFrontFaceType, 3, 2);
        bool frontColorCubeIsGood = cube.GetColor(relativeFrontFaceType) == (Face.Color)relativeFrontFaceType;
        bool upColorCubeMatchWithLeft = cube.GetColor(Face.FaceType.BOTTOM) == (Face.Color)RelativeFaceTypeGetter.GetRelativeRight(relativeFrontFaceType);
        bool upColorCubeMatchWithRight = cube.GetColor(Face.FaceType.BOTTOM) == (Face.Color)RelativeFaceTypeGetter.GetRelativeLeft(relativeFrontFaceType);

        if (frontColorCubeIsGood && upColorCubeMatchWithLeft)
        {
            rubiksCube.StartCoroutine(MoveLeft(relativeFrontFaceType));
        }
        else if (frontColorCubeIsGood && upColorCubeMatchWithRight)
        {
            rubiksCube.StartCoroutine(MoveRight(relativeFrontFaceType));
        }
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator MoveLeft(Face.FaceType relativeFrontFaceType)
    {
        Debug.Log("Move left" + " : " + relativeFrontFaceType);
        string[] movements = { "Ui", "Li", "U", "L", "U", "F", "Ui", "Fi" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType, true);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator MoveRight(Face.FaceType relativeFrontFaceType)
    {
        Debug.Log("Move right" + " : " + relativeFrontFaceType);
        string[]movements = new string[] { "U", "R", "Ui", "Ri", "Ui", "Fi", "U", "F" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType, true);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator UnblockSituation()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            if (!rubiksCube.GetCube(faceType, 2, 3).IsPlaced())
            {
                rubiksCube.StartCoroutine(MoveLeft(faceType));
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }
        }
    }

    public bool HasFinished()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            for (int i_row = 1; i_row <= 2; i_row++)
            {
                for (int i_column = 1; i_column <= 3; i_column++)
                {
                    if (!rubiksCube.GetCube(faceType, i_row, i_column).IsPlaced())
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}