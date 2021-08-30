using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver
{
    public static IEnumerator Start(RubiksCube rubiksCube)
    {
        WhiteCrossMaker whiteCrossMaker = new WhiteCrossMaker(rubiksCube);
        EdgesMaker edgesMaker = new EdgesMaker(rubiksCube);

        rubiksCube.StartCoroutine(whiteCrossMaker.Work(rubiksCube));
        yield return new WaitUntil(() => whiteCrossMaker.finished);

        rubiksCube.StartCoroutine(edgesMaker.Work(rubiksCube));
        yield return new WaitUntil(() => edgesMaker.finished);
    }
}
