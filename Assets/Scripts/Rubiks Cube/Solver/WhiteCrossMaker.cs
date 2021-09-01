using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteCrossMaker : IMaker
{
    RubiksCube rubiksCube;
    bool faceFreeToRotate;
    public bool finished = false;

    public WhiteCrossMaker(RubiksCube rubiksCube)
    {
        this.rubiksCube = rubiksCube;
    }

    public IEnumerator Work()
    {
        Debug.Log("White cross");
        int nbCubesWellPlaced = NbCubesWellPlaced();
        while (!HasFinished())
        {
            foreach (Face.FaceType faceType in rubiksCube.faces.Keys)
            {
                if (faceType == Face.FaceType.BOTTOM)
                {
                    yield return ElevateBottomWhitePlatesIfPossible();
                    nbCubesWellPlaced++;
                }
                else if (faceType != Face.FaceType.BOTTOM && faceType != Face.FaceType.UP)
                {
                    yield return ManipulateWhitePlates(faceType);
                    nbCubesWellPlaced++;
                }

                if (nbCubesWellPlaced == 4) // We don't want to iterate more faces if white cross is made
                {
                    break;
                }
            }
        }
        this.finished = true;
    }

    IEnumerator ElevateBottomWhitePlatesIfPossible()
    {
        Cube cube = GetOneMiddleWhiteCubeOnBottomFaceIfPossible();
        if (cube != null)
        {
            // Find relative face
            Face.FaceType destinationFace = cube.GetFaceTypes().Single(f => f != Face.FaceType.BOTTOM);

            // Make the relative face be able to rotate without destroying the existing parts of the white cross
            yield return MakeFaceFreeToRotate(destinationFace);

            // Rotate the relative face
            yield return rubiksCube.ManipulateRoutine("Fi", destinationFace);
        }
    }

    Cube GetOneMiddleWhiteCubeOnBottomFaceIfPossible()
    {
        List<Cube> potentialMiddleWhiteCubes = new List<Cube>()
        {
            rubiksCube.GetCube(Face.FaceType.BOTTOM, 1, 2),
            rubiksCube.GetCube(Face.FaceType.BOTTOM, 3, 2),
            rubiksCube.GetCube(Face.FaceType.BOTTOM, 2, 1),
            rubiksCube.GetCube(Face.FaceType.BOTTOM, 2, 3)
        };

        foreach (Cube potentialMiddleWhiteCube in potentialMiddleWhiteCubes)
        {
            if (potentialMiddleWhiteCube.GetColor(Face.FaceType.BOTTOM) == Face.Color.WHITE)
            {
                return potentialMiddleWhiteCube;
            }
        }
        return null;
    }

    IEnumerator ManipulateWhitePlates(Face.FaceType faceType)
    {
        Cube middleUpCube = rubiksCube.GetCube(faceType, 1, 2);
        Cube middleBottomCube = rubiksCube.GetCube(faceType, 3, 2);
        Cube middleLeftCube = rubiksCube.GetCube(faceType, 2, 1);
        Cube middleRightCube = rubiksCube.GetCube(faceType, 2, 3);

        if (middleUpCube.GetColor(faceType) == Face.Color.WHITE)
        {
            yield return ManipulateRow(faceType, 1);
        }
        else if (middleBottomCube.GetColor(faceType) == Face.Color.WHITE)
        {
            yield return ManipulateRow(faceType, 3);
        }
        else if (middleLeftCube.GetColor(faceType) == Face.Color.WHITE)
        {
            yield return ManipulateColumn(faceType, 1);
        }
        else if (middleRightCube.GetColor(faceType) == Face.Color.WHITE)
        {
            yield return ManipulateColumn(faceType, 3);
        }
    }

    IEnumerator ManipulateRow(Face.FaceType faceType, int row)
    {
        // Make the relative front face able to rotate without destroying the existing parts of the white cross
        yield return MakeFaceFreeToRotate(faceType);

        // Select movements and faces to check
        string mov_rowToColumn = "";
        switch (row)
        {
            case 1:
                mov_rowToColumn = "Fi";
                break;
            case 3:
                mov_rowToColumn = "F";
                break;
        }

        // Make the relative front face rotate (clockwork or anticlockwork)
        yield return rubiksCube.ManipulateRoutine(mov_rowToColumn, faceType);

        // The middle white plate in the row has now been converted in column so we can manipulate it as a column
        yield return ManipulateColumn(faceType, row);
    }

    IEnumerator ManipulateColumn(Face.FaceType faceType, int column)
    {
        // Select movements and faces to check
        Face.FaceType destinationFace = new Face.FaceType();
        string mov_elevator = "";
        switch (column)
        {
            case 1:
                destinationFace = RelativeFaceTypeGetter.GetRelativeLeft(faceType);
                mov_elevator = "Li";
                break;
            case 3:
                destinationFace = RelativeFaceTypeGetter.GetRelativeRight(faceType);
                mov_elevator = "R";
                break;
        }

        // Make the relative face (left or right) be able to rotate without destroying the existing parts of the white cross
        yield return MakeFaceFreeToRotate(destinationFace);

        // Make the relative face (left or right) rotate (clockwork or anticlockwork)
        yield return rubiksCube.ManipulateRoutine(mov_elevator, faceType);
    }

    IEnumerator MakeFaceFreeToRotate(Face.FaceType faceTypeToMakeFree)
    {
        faceFreeToRotate = false;

        while (rubiksCube.GetCube(faceTypeToMakeFree, 1, 2).GetColor(Face.FaceType.UP) == Face.Color.WHITE)
        {
            yield return rubiksCube.ManipulateRoutine("U");

            if (HasFinished())
            {
                break;
            }
        }

        faceFreeToRotate = true;
    }

    public bool HasFinished()
    {
        return NbCubesWellPlaced() == 4;
    }

    int NbCubesWellPlaced()
    {
        List<Tuple<int, int>> whiteCrossCubesIndexes = new List<Tuple<int, int>>()
        {
            { new Tuple<int, int>(1, 2) },
            { new Tuple<int, int>(2, 1) },
            { new Tuple<int, int>(2, 3) },
            { new Tuple<int, int>(3, 2) },
        };

        int nbCubesWellPlaced = 0;
        foreach (Tuple<int, int> cubeIndex in whiteCrossCubesIndexes)
        {
            Cube cube = rubiksCube.GetCube(Face.FaceType.UP, cubeIndex.Item1, cubeIndex.Item2);
            bool cubeWellPlaced = cube.GetColor(Face.FaceType.UP) == Face.Color.WHITE;
            if (cubeWellPlaced)
            {
                nbCubesWellPlaced++;
            }
        }
        return nbCubesWellPlaced;
    }
}
