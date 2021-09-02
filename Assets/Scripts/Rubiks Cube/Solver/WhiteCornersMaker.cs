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
        while (!HasFinished())
        {
            // EVERYTHING ON THE BOTTOM
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 3, 3);
                bool cubeHasWhite = cube.HasColor(Face.Color.WHITE);
                if (cubeHasWhite)
                {
                    rubiksCube.StartCoroutine(PlaceCubeInHisCorner(cube, faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }
            }

            // EVERYTHING ON THE TOP
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 1, 3);
                bool cubeHasWhite = cube.HasColor(Face.Color.WHITE);
                if (cubeHasWhite && !CubeMatchBothFacesOnTop(cube))
                {
                    rubiksCube.StartCoroutine(BringDownCube(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                    rubiksCube.StartCoroutine(PlaceCubeInHisCorner(cube, faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }
                else if (cubeHasWhite && CubeMatchBothFacesOnTop(cube) && !CubeIsOriented(cube))
                {
                    while (!CubeIsOriented(cube))
                    {
                        rubiksCube.StartCoroutine(PlaceCornerAlgorithm(faceType));
                        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                    }
                }
            }
        }

        finished = true;
    }

    IEnumerator PlaceCubeInHisCorner(Cube cube, Face.FaceType faceType)
    {
        rubiksCube.StartCoroutine(MoveCubeUnderIsCorner(cube, faceType));
        yield return new WaitUntil(() => cubeReadyToBeElevated);
        rubiksCube.StartCoroutine(ElevateCubeToHisCorner(cube, newCubeFaceType));
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator MoveCubeUnderIsCorner(Cube cube, Face.FaceType relativeFrontFaceType)
    {
        cubeReadyToBeElevated = false;

        int nbRotation = 0;
        while (!CubeMatchBothFacesOnBottom(cube))
        {
            if (HasFinished())
            {
                break;
            }

            rubiksCube.Manipulate("D");
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
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
            rubiksCube.StartCoroutine(PlaceCornerAlgorithm(relativeFrontFaceType));
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
    }

    IEnumerator PlaceCornerAlgorithm(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R", "D" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator BringDownCube(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
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


    bool CubesAreOriented()
    {
        foreach (Tuple<int, int> cornerIndex in cornersIndexes)
        {
            Cube cube = rubiksCube.GetCube(Face.FaceType.UP, cornerIndex.Item1, cornerIndex.Item2);
            if (!cube.HasColorOnFaceType(Face.Color.WHITE, Face.FaceType.UP))
            {
                return false;
            }
        }
        return true;
    }
    bool CubeIsOriented(Cube cube)
    {
        return cube.HasColorOnFaceType(Face.Color.WHITE, Face.FaceType.UP);
    }

    public bool HasFinished()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            Cube cube = rubiksCube.GetCube(faceType, 1, 3);
            if (!CubeMatchBothFaces(cube, Face.FaceType.UP, true))
            {
                return false;
            }
        }
        return true;
    }
}


/*using System;
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
    public bool finished = false;

    public WhiteCornersMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        while (!FourCornersAreOnTop())
        {
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 3, 3);
                bool cubeHasWhite = cube.HasColor(Face.Color.WHITE);
                if (cubeHasWhite)
                {
                    rubiksCube.StartCoroutine(PlaceCubeInHisCorner(cube, faceType, Face.FaceType.BOTTOM));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }
            }
        }

        // BUG ?
        while (!HasFinished())
        {
            foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
            {
                Cube cube = rubiksCube.GetCube(faceType, 1, 3);
                if (!CubeMatchBothFaces(cube, Face.FaceType.UP))
                {
                    rubiksCube.StartCoroutine(BringDownCube(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);

                    rubiksCube.StartCoroutine(PlaceCubeInHisCorner(cube, faceType, Face.FaceType.BOTTOM));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }
                else if (CubeMatchBothFaces(cube, Face.FaceType.UP, true) && !CubeIsOriented(cube))
                {
                    while (!CubeIsOriented(cube))
                    {
                        rubiksCube.StartCoroutine(PlaceCornerAlgorithm(faceType));
                        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                    }
                }
            }
        }

        finished = true;
    }

    IEnumerator PlaceCubeInHisCorner(Cube cube, Face.FaceType faceType, Face.FaceType appropriateWhiteFaceType)
    {
        rubiksCube.StartCoroutine(MoveCubeUnderIsCorner(cube, faceType, appropriateWhiteFaceType));
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);

        rubiksCube.StartCoroutine(ElevateCubeToHisCorner(cube, newCubeFaceType));
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator MoveCubeUnderIsCorner(Cube cube, Face.FaceType relativeFrontFaceType, Face.FaceType appropriateWhiteFaceType)
    {
        int nbRotation = 0;
        while (!CubeMatchBothFaces(cube, appropriateWhiteFaceType, false))
        {
            rubiksCube.Manipulate("D");
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            nbRotation++;
        }

        newCubeFaceType = relativeFrontFaceType;
        for (int i = 0; i < nbRotation; i++)
        {
            newCubeFaceType = RelativeFaceTypeGetter.GetRelativeRight(newCubeFaceType);
        }
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
            rubiksCube.StartCoroutine(PlaceCornerAlgorithm(relativeFrontFaceType));
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
    }

    IEnumerator PlaceCornerAlgorithm(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R", "D" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    IEnumerator BringDownCube(Face.FaceType relativeFrontFaceType)
    {
        string[] movements = { "Ri", "Di", "R" };
        rubiksCube.ManipulateMultipleTimes(movements, relativeFrontFaceType);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    bool CubeMatchBothFaces(Cube cube, Face.FaceType appropriateWhiteFaceType, bool checkOrientation = false)
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
        cube.GetColorFaceType(Face.Color.WHITE);
        return cube.HasColorOnFaceType(Face.Color.WHITE, Face.FaceType.UP);     
    }

    bool FourCornersAreOnTop()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            Cube cube = rubiksCube.GetCube(faceType, 1, 3);
            if (!cube.HasColor(Face.Color.WHITE))
            {
                return false;
            }
        }
        return true;
    }

    public bool HasFinished()
    {
        foreach (Face.FaceType faceType in RelativeFaceTypeGetter.GetHorizontalFaceTypes())
        {
            Cube cube = rubiksCube.GetCube(faceType, 1, 3);
            if (!CubeMatchBothFaces(cube, Face.FaceType.UP, true))
            {
                return false;
            }
        }
        return true;
    }
}
*/