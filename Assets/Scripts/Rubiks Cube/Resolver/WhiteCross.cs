using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WhiteCross
{
    public static void Make()
    {
        var whiteCrossMade = false;
        while (!whiteCrossMade)
        {
            // Find a middle white plate of row (only row 1 and 2) OR middle white plate of column

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

    public static void WhiteCrossDone(RubiksCube rubiksCube)
    {
        Face frontFace = rubiksCube.faces[Face.FaceType.FRONT];

        /*return frontFace.cubes["2"].color == "White" &&
               frontFace.cubes["4"].color == "White" &&
               frontFace.cubes["6"].color == "White" &&
               frontFace.cubes["8"].color == "White";*/
    }
}
