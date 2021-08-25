using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WhiteCross
{
    static RubiksCube rubiksCube;

    public static void Make(RubiksCube rubiksCube)
    {
        // Set rubiksCube
        WhiteCross.rubiksCube = rubiksCube;

        // Do algorithm
        while (!WhiteCrossDone())
        {
            foreach (Face.FaceType faceType in rubiksCube.faces.Keys)
            {
                // Loop on every face except the UP face (white)
                if (faceType != Face.FaceType.UP)
                {
                    ManipulateWhitePlatesOnRow(faceType);
                    //SearchForCenteredWhitePlatesOnColumn(faceType);
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


        }
        // Place white plate under his desired position
    }

    static void ManipulateWhitePlatesOnRow(Face.FaceType faceType)
    {
        for (int i_row = 1; i_row <= 3; i_row += 2) // we don't want to search on second row
        {
            Cube cube = rubiksCube.GetCube(faceType, i_row, 2).GetComponent<Cube>();
            Face.Color color = cube.GetColor(faceType);

            if (color == Face.Color.WHITE && i_row == 1)
            {
                ManipulateCubeOnRowOne(faceType);
            }

            else if (color == Face.Color.WHITE && i_row == 3)
            {
                ManipulateCubeOnRowThree(faceType);
            }
        }
    }

    static void ManipulateCubeOnRowOne(Face.FaceType faceType)
    {
        rubiksCube.Manipulate("Fi");
        while (rubiksCube.GetCube(faceType, 1, 2).GetComponent<Cube>().GetColor(Face.FaceType.UP) == Face.Color.WHITE)
        {
            rubiksCube.Manipulate("U");
        }
        rubiksCube.Manipulate("Li");
    }

    static void ManipulateCubeOnRowThree(Face.FaceType faceType)
    {
        rubiksCube.Manipulate("F");
        while (rubiksCube.GetCube(faceType, 1, 2).GetComponent<Cube>().GetColor(Face.FaceType.UP) == Face.Color.WHITE)
        {
            rubiksCube.Manipulate("U");
        }
        rubiksCube.Manipulate("R");
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

    static bool WhiteCrossDone()
    {
        return rubiksCube.GetCubeColor(Face.FaceType.UP, 1, 2) == Face.Color.WHITE &&
               rubiksCube.GetCubeColor(Face.FaceType.UP, 2, 1) == Face.Color.WHITE &&
               rubiksCube.GetCubeColor(Face.FaceType.UP, 2, 3) == Face.Color.WHITE &&
               rubiksCube.GetCubeColor(Face.FaceType.UP, 3, 2) == Face.Color.WHITE;
    }
}
