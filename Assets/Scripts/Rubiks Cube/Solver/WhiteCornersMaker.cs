using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class WhiteCornersMaker : IMaker
{
    RubiksCube rubiksCube;
    Face.FaceType newCubeFaceType;
    bool cubeReadyToBeElevated;
    List<Tuple<int, int>> cornersIndexes = new List<Tuple<int, int>>()
    {
        { new Tuple<int, int>(1, 1) },
        { new Tuple<int, int>(1, 3) },
        { new Tuple<int, int>(3, 1) },
        { new Tuple<int, int>(3, 3) },
    };
    public bool finished = false;

    public WhiteCornersMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        Debug.Log("White corners");
        while (!HasFinished())
        {
            // EVERYTHING ON THE TOP
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 1, 3);
                bool cubeHasWhite = cube.HasColor(Face.Color.WHITE);
                if (cubeHasWhite && !CubeMatchBothFacesOnTop(cube))
                {
                    yield return BringDownCube(faceType);
                    yield return PlaceCubeInHisCorner(cube, faceType);
                }
                else if (cubeHasWhite && CubeMatchBothFacesOnTop(cube) && !CubeIsOriented(cube))
                {
                    while (!CubeIsOriented(cube))
                    {
                        yield return PlaceCornerAlgorithm(faceType);
                    }
                }
            }

            // EVERYTHING ON THE BOTTOM
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 3, 3);
                bool cubeHasWhite = cube.HasColor(Face.Color.WHITE);
                if (cubeHasWhite)
                {
                    yield return PlaceCubeInHisCorner(cube, faceType);
                }
            }

        }
        finished = true;
    }

    IEnumerator PlaceCubeInHisCorner(Cube cube, Face.FaceType faceType)
    {
        yield return MoveCubeUnderIsCorner(cube, faceType);
        yield return ElevateCubeToHisCorner(cube, newCubeFaceType);
    }

    IEnumerator MoveCubeUnderIsCorner(Cube cube, Face.FaceType relativeFrontFaceType)
    {
        cubeReadyToBeElevated = false;

        int nbRotation = 0;
        while (!CubeMatchBothFacesOnBottom(cube))
        {
            yield return rubiksCube.ManipulateRoutine("D");
            nbRotation++;
        }

        newCubeFaceType = relativeFrontFaceType;
        for (int i = 0; i < nbRotation; i++)
        {
            newCubeFaceType = RelativeFaceTypeGetter.GetRelativeRight(newCubeFaceType);
        }

        cubeReadyToBeElevated = true;
    }

    IEnumerator ElevateCubeToHisCorner(Cube cube, Face.FaceType relativeFrontFaceType)
    {
        int nbManipulation = 0;
        if (cube.GetColorFaceType(Face.Color.WHITE) == RelativeFaceTypeGetter.GetRelativeRight(relativeFrontFaceType))
        {
            nbManipulation = 1;
        }
        else if (cube.GetColorFaceType(Face.Color.WHITE) == Face.FaceType.BOTTOM)
        {
            nbManipulation = 3;
        }
        if (cube.GetColorFaceType(Face.Color.WHITE) == relativeFrontFaceType)
        {
            nbManipulation = 5;
        }

        for (int i = 0; i < nbManipulation; i++)
        {
            yield return PlaceCornerAlgorithm(relativeFrontFaceType);
        }
    }

    IEnumerator PlaceCornerAlgorithm(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R", "D" };
        yield return rubiksCube.ManipulateMultipleTimesRoutine(movements, relativeFrontFaceType);
    }

    IEnumerator BringDownCube(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R" };
        yield return rubiksCube.ManipulateMultipleTimesRoutine(movements, relativeFrontFaceType);
    }

    bool AllCubesMatchTheirFaces()
    {
        foreach (Tuple<int, int> cornerIndex in cornersIndexes)
        {
            Cube cube = rubiksCube.GetCube(Face.FaceType.UP, cornerIndex.Item1, cornerIndex.Item2);
            if (!CubeMatchBothFacesOnTop(cube))
            {
                return false;
            }
        }
        return true;
    }

    bool CubeMatchBothFacesOnTop(Cube cube)
    {
        return CubeMatchBothFaces(cube, Face.FaceType.UP, false);
    }

    bool CubeMatchBothFacesOnBottom(Cube cube)
    {
        return CubeMatchBothFaces(cube, Face.FaceType.BOTTOM, false);
    }

    bool CubeMatchBothFaces(Cube cube, Face.FaceType appropriateWhiteFaceType, bool checkOrientation)
    {
        List<ColorFaceAssociation> colorsExceptWhite = new List<ColorFaceAssociation>();
        Face.FaceType whiteColorFaceType = new Face.FaceType();
        foreach (ColorFaceAssociation colorFaceAssociation in cube.colorFaceAssociations)
        {
            if (colorFaceAssociation.color == Face.Color.WHITE)
            {
                whiteColorFaceType = colorFaceAssociation.faceType;
            }
            else
            {
                colorsExceptWhite.Add(colorFaceAssociation);
            }
        }

        Face.Color colorFace1 = colorsExceptWhite[0].color;
        Face.Color colorFace2 = colorsExceptWhite[1].color;
        Face.FaceType faceTypeFace1 = (colorsExceptWhite[0].faceType == appropriateWhiteFaceType) ? whiteColorFaceType : colorsExceptWhite[0].faceType;
        Face.FaceType faceTypeFace2 = (colorsExceptWhite[1].faceType == appropriateWhiteFaceType) ? whiteColorFaceType : colorsExceptWhite[1].faceType;

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

    bool CubeIsOriented(Cube cube)
    {
        return cube.HasColorOnFaceType(Face.Color.WHITE, Face.FaceType.UP);     
    }

    public bool HasFinished()
    {
        try
        {
            Face upFace = rubiksCube.faces[Face.FaceType.UP];
            foreach (Row row in upFace.rows)
            {
                foreach (GameObject cube in row.cubes)
                {
                    if (cube.GetComponent<Cube>().GetColor(Face.FaceType.UP) != Face.Color.WHITE)
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

        return true;
    }
}
