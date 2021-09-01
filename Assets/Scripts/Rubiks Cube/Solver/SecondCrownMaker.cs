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
        Debug.Log("Here");
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            rubiksCube.StartCoroutine(FirstPass(faceType));
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
        finished = true;
    }

    IEnumerator FirstPass(Face.FaceType relativeFrontFaceType)
    {
        Cube cube = rubiksCube.GetCube(relativeFrontFaceType, 3, 2);
        bool frontColorCubeIsGood = cube.GetColor(relativeFrontFaceType) == (Face.Color)relativeFrontFaceType;
        bool upColorCubeMatchWithLeft = cube.GetColor(Face.FaceType.UP) == (Face.Color)RelativeFaceTypeGetter.GetRelativeLeft(relativeFrontFaceType);
        bool upColorCubeMatchWithRight = cube.GetColor(Face.FaceType.UP) == (Face.Color)RelativeFaceTypeGetter.GetRelativeRight(relativeFrontFaceType);

        if (frontColorCubeIsGood)
        {
            string[] movements = new string[8];
            if (upColorCubeMatchWithLeft)
            {
                movements = new string[] { "Ui", "Li", "U", "L", "U", "F", "Ui", "Fi" };
            }
            else if (upColorCubeMatchWithRight)
            {
                movements = new string[] { "U", "R", "Ui", "Ri", "Ui", "Fi", "U", "F" };
            }

            rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType, true);
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
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
                    if (rubiksCube.GetCube(faceType, i_row, i_column).GetColor(faceType) != (Face.Color)faceType)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}