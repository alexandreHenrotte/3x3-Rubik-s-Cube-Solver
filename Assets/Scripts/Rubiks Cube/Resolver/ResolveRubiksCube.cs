using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveRubiksCube
{
    private RubiksCube rubiksCube;

    void StartSolving()
    {
        //MakeWhiteCross();
    }

    void MakeWhiteCross()
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

    /*bool WhiteCrossDone()
    {
        Face frontFace = rubiksCube.faces[Face.FaceType.FRONT];
        return frontFace.cubes["2"].color == "White" &&
               frontFace.cubes["4"].color == "White" &&
               frontFace.cubes["6"].color == "White" &&
               frontFace.cubes["8"].color == "White";
    }*/

    /* Create the face as a dictionnary and map each cube with a number following this mapping :
       1 2 3
       4 5 6
       7 8 9
    */
    /*Dictionary<string, GameObject> createRepresentativeFace(Face face)
    {
        // X
        GameObject Xcube = face.cubes[0];
        for (int i = 1; i < face.cubes; i++)
        {
            GameObject i_cube = face.cubes[i];
            if (i_cube.transform.position.)
        }
    }*/
}
