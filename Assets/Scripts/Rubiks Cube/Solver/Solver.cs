using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver
{
    public static IEnumerator Start(RubiksCube rubiksCube)
    {
        float oldSpeed = Face.rotatingSpeed;
        Face.rotatingSpeed = 1000f;

        // Step makers
        WhiteCrossMaker whiteCrossMaker = new WhiteCrossMaker(rubiksCube);
        EdgesMaker edgesMaker = new EdgesMaker(rubiksCube);
        WhiteCornersMaker whiteCornersMaker = new WhiteCornersMaker(rubiksCube);
        SecondCrownMaker secondCrownMaker = new SecondCrownMaker(rubiksCube);

        // Step 1
        Debug.Log("repet");
        rubiksCube.StartCoroutine(whiteCrossMaker.Work());
        yield return new WaitUntil(() => whiteCrossMaker.finished);
        yield return new WaitForSeconds(2f);

        // Step 2
        rubiksCube.StartCoroutine(edgesMaker.Work());
        yield return new WaitUntil(() => edgesMaker.finished);
        yield return new WaitForSeconds(2f);

        // Step 3
        rubiksCube.StartCoroutine(whiteCornersMaker.Work());
        yield return new WaitUntil(() => whiteCornersMaker.finished);
        yield return new WaitForSeconds(2f);

        // Step 4
        rubiksCube.StartCoroutine(secondCrownMaker.Work());
        //yield return new WaitUntil(() => secondCrownMaker.finished);
        yield return new WaitForSeconds(2f);

        Face.rotatingSpeed = oldSpeed;
    }
}
