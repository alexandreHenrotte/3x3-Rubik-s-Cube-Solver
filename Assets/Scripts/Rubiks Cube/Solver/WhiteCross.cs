using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WhiteCross
{
    static RubiksCube rubiksCube;
    static List<string> movementCallsToDo = new List<string>();

    public static void Make(RubiksCube rubiksCube)
    {
        // Set rubiksCube
        WhiteCross.rubiksCube = rubiksCube;

        // Do algorithm
        foreach (Face.FaceType faceType in rubiksCube.faces.Keys)
        {
            if (faceType != Face.FaceType.BOTTOM && faceType != Face.FaceType.UP)
            {
                ManipulateWhitePlatesOnRow(faceType);
                //ManipulateWhitePlatesOnColumn(faceType);
                //SearchForCenteredWhitePlatesOnColumn(faceType);
            }
        }

        rubiksCube.Manipulate(movementCallsToDo.ToArray());
        movementCallsToDo.Clear();


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

    static void ManipulateWhitePlatesOnRow(Face.FaceType faceType)
    {
        for (int i_row = 1; i_row <= 3; i_row += 2) // we don't want to search on second row
        {
            Cube cube = rubiksCube.GetCube(faceType, i_row, 2);
            Face.Color color = cube.GetColor(faceType);

            if (color == Face.Color.WHITE && i_row == 1)
            {
                rubiksCube.ChangeRelativeFacePositionning(faceType);

                movementCallsToDo.Add("Fi");
                MakeFaceFreeToRotate(faceType);
                movementCallsToDo.Add("Li");

                rubiksCube.ResetFacePositionning();
            }

            else if (color == Face.Color.WHITE && i_row == 3)
            {
                rubiksCube.ChangeRelativeFacePositionning(faceType);

                movementCallsToDo.Add("F");
                MakeFaceFreeToRotate(faceType);
                movementCallsToDo.Add("R");

                rubiksCube.ResetFacePositionning();
            }
        }
    }

    /*
    static void SearchForCenteredWhitePlatesOnColumn(RubiksCube rubiksCube, Face.FaceType faceType)
    {
        for (int i_column = 1; i_column <= 3; i_column += 2) // we don't want to search on second column
        {
            Cube cube = rubiksCube.GetCube(faceType, 2, i_column).GetComponent<Cube>();
            Face.Color color = cube.GetColor(faceType);
            if (color == Face.Color.WHITE)
            {
                // ALGORITHM TO DO
            }
        }
    }
    */

    static void MakeFaceFreeToRotate(Face.FaceType faceTypeToMove)
    {
        Face.FaceType[] relativeFaceTypes = { Face.FaceType.LEFT, Face.FaceType.REAR, Face.FaceType.RIGHT, Face.FaceType.FRONT }; // UP and BOTTOM are not relatives

        // Find number of rotations needed
        int numberOfNeededRotations = 0;
        int indexOfFaceTypeToMove = Array.IndexOf(relativeFaceTypes, faceTypeToMove);
        for (int i = indexOfFaceTypeToMove; i < relativeFaceTypes.Length + indexOfFaceTypeToMove; i++)
        {
            Face.FaceType i_faceType = relativeFaceTypes[i % relativeFaceTypes.Length];
            bool upCubeIsWhite = rubiksCube.faces[i_faceType].GetCube(1, 2).GetColor(Face.FaceType.UP) == Face.Color.WHITE;
            if (!upCubeIsWhite)
            {
                numberOfNeededRotations = i;
                break;
            }
        }

        if (numberOfNeededRotations > 0)
        {
            // Create list of rotation movements
            string rotationMovementCall = "U";
            for (int i = 0; i < numberOfNeededRotations; i++)
            {
                movementCallsToDo.Add(rotationMovementCall);
            }
        }
    }

    static bool WhiteCrossDone()
    {
        return rubiksCube.GetCubeColor(Face.FaceType.UP, 1, 2) == Face.Color.WHITE &&
               rubiksCube.GetCubeColor(Face.FaceType.UP, 2, 1) == Face.Color.WHITE &&
               rubiksCube.GetCubeColor(Face.FaceType.UP, 2, 3) == Face.Color.WHITE &&
               rubiksCube.GetCubeColor(Face.FaceType.UP, 3, 2) == Face.Color.WHITE;
    }
}
