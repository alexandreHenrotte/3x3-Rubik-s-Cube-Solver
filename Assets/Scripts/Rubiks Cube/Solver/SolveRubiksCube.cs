using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolveRubiksCube
{
    public static void Start(RubiksCube rubiksCube)
    {
        WhiteCross.Make(rubiksCube);
    }
}
