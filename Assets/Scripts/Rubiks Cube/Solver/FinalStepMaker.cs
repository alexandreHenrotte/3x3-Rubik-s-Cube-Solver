using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class FinalStepMaker : IMaker
{
    RubiksCube rubiksCube;
    public bool finished = false;

    public FinalStepMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        Face.FaceType faceType = FindFaceTypeToWorkOn();

        // Orientate corners
        while (!HasFinished())
        {
            Cube cube = rubiksCube.GetCube(faceType, 3, 1);
            while (!cube.HasColorOnFaceType(Face.Color.YELLOW, Face.FaceType.BOTTOM))
            {
                rubiksCube.StartCoroutine(ManipulationAlgorithm(faceType));
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }

            rubiksCube.Manipulate("U", rubiksCubeUpsideDown: true);
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
    }
    
    Face.FaceType FindFaceTypeToWorkOn()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            Cube cube = rubiksCube.GetCube(faceType, 3, 1);
            if (!cube.HasColorOnFaceType(Face.Color.WHITE, Face.FaceType.BOTTOM))
            {
                return faceType;
            }
        }
        throw new Exception("There is no face type to work on because the cube are already placed and rotated");
    }

    IEnumerator ManipulationAlgorithm(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R", "D" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType, rubiksCubeUpsideDown: true);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    public bool HasFinished()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            Cube cube = rubiksCube.GetCube(faceType, 3, 1);
            if (!cube.IsPlaced())
            {
                return false;
            }
        }
        return true;
    }
}