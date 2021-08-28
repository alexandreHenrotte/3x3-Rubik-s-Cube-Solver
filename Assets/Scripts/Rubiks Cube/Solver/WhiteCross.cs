using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class WhiteCross
{
    static RubiksCube rubiksCube;
    static bool faceFreeToRotate;

    public static IEnumerator Make(RubiksCube rubiksCube)
    {
        // Set rubiksCube
        WhiteCross.rubiksCube = rubiksCube;

        // Do algorithm
        foreach (Face.FaceType faceType in rubiksCube.faces.Keys)
        {
            if (faceType != Face.FaceType.BOTTOM && faceType != Face.FaceType.UP)
            {
                rubiksCube.StartCoroutine(ManipulateWhitePlatesOnRow(faceType));
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                rubiksCube.StartCoroutine(ManipulateWhitePlatesOnColumn(faceType));
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }
        }


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
    }

    static IEnumerator ManipulateWhitePlatesOnRow(Face.FaceType faceType)
    {
        for (int i_row = 1; i_row <= 3; i_row += 2) // we don't want to search on second row
        {
            Cube cube = rubiksCube.GetCube(faceType, i_row, 2);
            Face.Color color = cube.GetColor(faceType);

            if (color == Face.Color.WHITE && i_row == 1)
            {
                rubiksCube.StartCoroutine(MakeFaceFreeToRotate(faceType));
                yield return new WaitUntil(() => faceFreeToRotate);
                rubiksCube.Manipulate("Fi", faceType);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                rubiksCube.StartCoroutine(MakeFaceFreeToRotate(GetLeftFaceType(faceType)));
                yield return new WaitUntil(() => faceFreeToRotate);
                rubiksCube.Manipulate("Li", faceType);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }

            else if (color == Face.Color.WHITE && i_row == 3)
            {
                rubiksCube.StartCoroutine(MakeFaceFreeToRotate(faceType));
                yield return new WaitUntil(() => faceFreeToRotate);
                rubiksCube.Manipulate("F", faceType);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
                rubiksCube.StartCoroutine(MakeFaceFreeToRotate(GetRightFaceType(faceType)));
                yield return new WaitUntil(() => faceFreeToRotate);
                rubiksCube.Manipulate("R", faceType);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }
        }
    }
    static IEnumerator ManipulateWhitePlatesOnColumn(Face.FaceType faceType)
    {
        for (int i_column = 1; i_column <= 3; i_column += 2) // we don't want to search on second column
        {
            Cube cube = rubiksCube.GetCube(faceType, 2, i_column).GetComponent<Cube>();
            Face.Color color = cube.GetColor(faceType);

            if (color == Face.Color.WHITE && i_column == 1)
            {
                rubiksCube.StartCoroutine(MakeFaceFreeToRotate(GetLeftFaceType(faceType)));
                yield return new WaitUntil(() => faceFreeToRotate);
                rubiksCube.Manipulate("Li", faceType);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }
            else if (color == Face.Color.WHITE && i_column == 3)
            {
                rubiksCube.StartCoroutine(MakeFaceFreeToRotate(GetRightFaceType(faceType)));
                yield return new WaitUntil(() => faceFreeToRotate);
                rubiksCube.Manipulate("R", faceType);
                yield return new WaitUntil(() => rubiksCube.readyToManipulate);
            }
        }
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

    static Face.FaceType GetLeftFaceType(Face.FaceType faceType)
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

    static Face.FaceType GetRightFaceType(Face.FaceType faceType)
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
        return rubiksCube.GetCube(Face.FaceType.BOTTOM, 1, 2).GetColor(Face.FaceType.BOTTOM) == Face.Color.WHITE &&
               rubiksCube.GetCube(Face.FaceType.BOTTOM, 2, 1).GetColor(Face.FaceType.BOTTOM) == Face.Color.WHITE &&
               rubiksCube.GetCube(Face.FaceType.BOTTOM, 2, 3).GetColor(Face.FaceType.BOTTOM) == Face.Color.WHITE &&
               rubiksCube.GetCube(Face.FaceType.BOTTOM, 3, 2).GetColor(Face.FaceType.BOTTOM) == Face.Color.WHITE;
    }
}
