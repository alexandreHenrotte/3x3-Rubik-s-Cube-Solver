using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Find a middle white plate of row (only row 1 and 3) OR middle white plate of column

// IF ROW
// a. if is on row 1
// --> F'
// --> turn white face until column touch the column that doesn't have white plate
// --> L'
// b. else if on row 3
// --> turn row until column first cube is not white
// --> F
// --> turn white face until column touch the column that doesn't have white plate
// --> R

// IF COLUMN
// --> turn white face until column touch the column that doesn't have white plate
// a. if is on left
// --> L'
// b. if is on right
// --> R
// Place white plate under his desired position

public static class WhiteCross
{
    static RubiksCube rubiksCube;
    static bool faceFreeToRotate;

    public static IEnumerator Make(RubiksCube rubiksCube)
    {
        // Set rubiksCube
        WhiteCross.rubiksCube = rubiksCube;

        // Do algorithm
        while (!WhiteCrossDone())
        {
            foreach (Face.FaceType faceType in rubiksCube.faces.Keys)
            {
                if (faceType == Face.FaceType.BOTTOM)
                {
                    rubiksCube.StartCoroutine(ElevateBottomWhitePlatesIfPossible());
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }

                if (faceType != Face.FaceType.BOTTOM && faceType != Face.FaceType.UP)
                {
                    rubiksCube.StartCoroutine(ManipulateWhitePlates(faceType));
                    yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                }
            }
        } 
    }

    static IEnumerator ElevateBottomWhitePlatesIfPossible()
    {
        Cube cube = GetOneMiddleWhiteCubeOnBottomFaceIfPossible();
        if (cube != null)
        {
            // Find relative face
            Face.FaceType destinationFace = cube.GetFaceTypes().Single(f => f != Face.FaceType.BOTTOM);

            // Make the relative face be able to rotate without destroying the existing parts of the white cross
            rubiksCube.StartCoroutine(MakeFaceFreeToRotate(destinationFace));
            yield return new WaitUntil(() => faceFreeToRotate);

            // Rotate the relative face
            rubiksCube.Manipulate("Fi", destinationFace);
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
    }

    static Cube GetOneMiddleWhiteCubeOnBottomFaceIfPossible()
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

    static IEnumerator ManipulateWhitePlates(Face.FaceType faceType)
    {
        Cube middleUpCube = rubiksCube.GetCube(faceType, 1, 2);
        Cube middleBottomCube = rubiksCube.GetCube(faceType, 3, 2);
        Cube middleLeftCube = rubiksCube.GetCube(faceType, 2, 1);
        Cube middleRightCube = rubiksCube.GetCube(faceType, 2, 3);

        if (middleUpCube.GetColor(faceType) == Face.Color.WHITE)
        {
            rubiksCube.StartCoroutine(ManipulateRow(faceType, 1));
        }
        else if (middleBottomCube.GetColor(faceType) == Face.Color.WHITE)
        {
            rubiksCube.StartCoroutine(ManipulateRow(faceType, 3));
        }
        else if (middleLeftCube.GetColor(faceType) == Face.Color.WHITE)
        {
            rubiksCube.StartCoroutine(ManipulateColumn(faceType, 1));
        }
        else if (middleRightCube.GetColor(faceType) == Face.Color.WHITE)
        {
            rubiksCube.StartCoroutine(ManipulateColumn(faceType, 3));
        }
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    static IEnumerator ManipulateRow(Face.FaceType faceType, int row)
    {
        // Make the relative front face able to rotate without destroying the existing parts of the white cross
        rubiksCube.StartCoroutine(MakeFaceFreeToRotate(faceType));
        yield return new WaitUntil(() => faceFreeToRotate);

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
        rubiksCube.Manipulate(mov_rowToColumn, faceType);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);

        // The middle white plate in the row has now been converted in column so we can manipulate it as a column
        rubiksCube.StartCoroutine(ManipulateColumn(faceType, row));
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    static IEnumerator ManipulateColumn(Face.FaceType faceType, int column)
    {
        // Select movements and faces to check
        Face.FaceType destinationFace = new Face.FaceType();
        string mov_elevator = "";
        switch (column)
        {
            case 1:
                destinationFace = GetRelativeLeftFaceType(faceType);
                mov_elevator = "Li";
                break;
            case 3:
                destinationFace = GetRelativeRightFaceType(faceType);
                mov_elevator = "R";
                break;
        }

        // Make the relative face (left or right) be able to rotate without destroying the existing parts of the white cross
        rubiksCube.StartCoroutine(MakeFaceFreeToRotate(destinationFace));
        yield return new WaitUntil(() => faceFreeToRotate);

        // Make the relative face (left or right) rotate (clockwork or anticlockwork)
        rubiksCube.Manipulate(mov_elevator, faceType);
        yield return new WaitUntil(() => rubiksCube.readyToManipulate);
    }

    static IEnumerator MakeFaceFreeToRotate(Face.FaceType faceTypeToMakeFree)
    {
        faceFreeToRotate = false;

        while (rubiksCube.GetCube(faceTypeToMakeFree, 1, 2).GetColor(Face.FaceType.UP) == Face.Color.WHITE)
        {
            rubiksCube.Manipulate("U");
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }

        faceFreeToRotate = true;
    }

    static Face.FaceType GetRelativeLeftFaceType(Face.FaceType faceType)
    {
        switch (faceType)
        {
            case Face.FaceType.FRONT:
                return Face.FaceType.LEFT;
            case Face.FaceType.LEFT:
                return Face.FaceType.REAR;
            case Face.FaceType.REAR:
                return Face.FaceType.RIGHT;
            case Face.FaceType.RIGHT:
                return Face.FaceType.FRONT;
            default:
                throw new Exception($"{faceType} is not a valid horizontal face type");
        }
    }

    static Face.FaceType GetRelativeRightFaceType(Face.FaceType faceType)
    {
        switch (faceType)
        {
            case Face.FaceType.FRONT:
                return Face.FaceType.RIGHT;
            case Face.FaceType.LEFT:
                return Face.FaceType.FRONT;
            case Face.FaceType.REAR:
                return Face.FaceType.LEFT;
            case Face.FaceType.RIGHT:
                return Face.FaceType.REAR;
            default:
                throw new Exception($"{faceType} is not a valid horizontal face type");
        }
    }

    static bool WhiteCrossDone()
    {
        return rubiksCube.GetCube(Face.FaceType.UP, 1, 2).GetColor(Face.FaceType.UP) == Face.Color.WHITE &&
               rubiksCube.GetCube(Face.FaceType.UP, 2, 1).GetColor(Face.FaceType.UP) == Face.Color.WHITE &&
               rubiksCube.GetCube(Face.FaceType.UP, 2, 3).GetColor(Face.FaceType.UP) == Face.Color.WHITE &&
               rubiksCube.GetCube(Face.FaceType.UP, 3, 2).GetColor(Face.FaceType.UP) == Face.Color.WHITE;
    }
}
