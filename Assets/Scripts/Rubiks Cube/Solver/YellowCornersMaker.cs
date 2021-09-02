using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class YellowCornersMaker : IMaker
{
    RubiksCube rubiksCube;
    public bool finished = false;

    public YellowCornersMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        while (!HasFinished())
        {
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 3, 1);
                if (CubeMatchBothFaces(cube, Face.FaceType.BOTTOM, false)) // Case where one is already placed
                {
                    rubiksCube.StartCoroutine(ManipulationAlgorithm(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);

                    if (HasFinished())
                    {
                        break;
                    }
                    else
                    {
                        rubiksCube.StartCoroutine(ManipulationAlgorithm(faceType)); // The cubes will always be place after this second manipulation
                        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                        break;
                    }
                }
            }

            if (!HasFinished())
            {
                // Case where no one is already placed (it makes appear one placed cube)
                rubiksCube.StartCoroutine(ManipulationAlgorithm(Face.FaceType.FRONT));
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }
        }

        finished = true;
    }

    IEnumerator ManipulationAlgorithm(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Li", "U", "R", "Ui", "L", "U", "Ri", "Ui" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType, true);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    bool CubeMatchBothFaces(Cube cube, Face.FaceType appropriateYellowFaceType, bool checkOrientation = false)
    {
        List<ColorFaceAssociation> colorsExceptYellow = new List<ColorFaceAssociation>();
        Face.FaceType yellowColorFaceType = new Face.FaceType();
        foreach (ColorFaceAssociation colorFaceAssociation in cube.colorFaceAssociations)
        {
            if (colorFaceAssociation.color == Face.Color.YELLOW)
            {
                yellowColorFaceType = colorFaceAssociation.faceType;
            }
            else
            {
                colorsExceptYellow.Add(colorFaceAssociation);
            }
        }

        Face.Color colorFace1 = colorsExceptYellow[0].color;
        Face.Color colorFace2 = colorsExceptYellow[1].color;
        Face.FaceType faceTypeFace1 = (colorsExceptYellow[0].faceType == appropriateYellowFaceType) ? yellowColorFaceType : colorsExceptYellow[0].faceType;
        Face.FaceType faceTypeFace2 = (colorsExceptYellow[1].faceType == appropriateYellowFaceType) ? yellowColorFaceType : colorsExceptYellow[1].faceType;

        bool CubeIsPlaced;
        if (checkOrientation)
        {
            CubeIsPlaced = (colorFace1 == (Face.Color)faceTypeFace1 &&
                            colorFace2 == (Face.Color)faceTypeFace2);
        }
        else
        {
            CubeIsPlaced = (colorFace1 == (Face.Color)faceTypeFace1 &&
                            colorFace2 == (Face.Color)faceTypeFace2) ||
                           (colorFace1 == (Face.Color)faceTypeFace2 &&
                            colorFace2 == (Face.Color)faceTypeFace1);
        }


        return CubeIsPlaced;
    }

    public bool HasFinished()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            Cube cube = rubiksCube.GetCube(faceType, 3, 1);
            if (!CubeMatchBothFaces(cube, Face.FaceType.BOTTOM, false))
            {
                return false;
            }
        }
        return true;
    }
}