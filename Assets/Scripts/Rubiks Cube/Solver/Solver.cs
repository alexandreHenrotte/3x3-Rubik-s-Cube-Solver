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
        EdgesMaker edgesMaker = new EdgesMaker(rubiksCube, false);
        WhiteCornersMaker whiteCornersMaker = new WhiteCornersMaker(rubiksCube);
        SecondCrownMaker secondCrownMaker = new SecondCrownMaker(rubiksCube);
        YellowCrossMaker yellowCrossMaker = new YellowCrossMaker(rubiksCube);
        EdgesMaker edgesMakerCubeUpsideDown = new EdgesMaker(rubiksCube, true);

        // Step 1
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
        yield return new WaitUntil(() => secondCrownMaker.finished);
        yield return new WaitForSeconds(2f);

        // Step 5
        rubiksCube.StartCoroutine(yellowCrossMaker.Work());
        yield return new WaitUntil(() => yellowCrossMaker.finished);
        yield return new WaitForSeconds(2f);

        Face.rotatingSpeed = 300f;

        // Step 6
        rubiksCube.StartCoroutine(edgesMakerCubeUpsideDown.Work());
        yield return new WaitUntil(() => edgesMakerCubeUpsideDown.finished);
        yield return new WaitForSeconds(2f);

        Face.rotatingSpeed = oldSpeed;
    }
}
