using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver
{
    const float SOLVING_SPEED = 1400f;
    const float TIME_BETWEEN_STEPS = 0f;

    public static IEnumerator Start(RubiksCube rubiksCube)
    {
        SpeedManager.Change(SOLVING_SPEED);

        // Step makers
        WhiteCrossMaker whiteCrossMaker = new WhiteCrossMaker(rubiksCube);
        EdgesMaker edgesMaker = new EdgesMaker(rubiksCube, false);
        WhiteCornersMaker whiteCornersMaker = new WhiteCornersMaker(rubiksCube);
        SecondCrownMaker secondCrownMaker = new SecondCrownMaker(rubiksCube);
        YellowCrossMaker yellowCrossMaker = new YellowCrossMaker(rubiksCube);
        EdgesMaker edgesMakerCubeUpsideDown = new EdgesMaker(rubiksCube, true);
        YellowCornersMaker yellowCornersMaker = new YellowCornersMaker(rubiksCube);
        FinalStepMaker finalStepMaker = new FinalStepMaker(rubiksCube);

        // Step 1
        Debug.Log("white cross");
        rubiksCube.StartCoroutine(whiteCrossMaker.Work());
        yield return new WaitUntil(() => whiteCrossMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 2
        Debug.Log("edges");
        rubiksCube.StartCoroutine(edgesMaker.Work());
        yield return new WaitUntil(() => edgesMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 3
        Debug.Log("white corners");
        rubiksCube.StartCoroutine(whiteCornersMaker.Work());
        yield return new WaitUntil(() => whiteCornersMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 4
        Debug.Log("second crown");
        rubiksCube.StartCoroutine(secondCrownMaker.Work());
        yield return new WaitUntil(() => secondCrownMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 5
        Debug.Log("yellow cross");
        rubiksCube.StartCoroutine(yellowCrossMaker.Work());
        yield return new WaitUntil(() => yellowCrossMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 6
        Debug.Log("edges cube upside down");
        rubiksCube.StartCoroutine(edgesMakerCubeUpsideDown.Work());
        yield return new WaitUntil(() => edgesMakerCubeUpsideDown.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 7
        Debug.Log("yellow corners");
        rubiksCube.StartCoroutine(yellowCornersMaker.Work());
        yield return new WaitUntil(() => yellowCornersMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        // Step 8
        Debug.Log("final step");
        rubiksCube.StartCoroutine(finalStepMaker.Work());
        yield return new WaitUntil(() => finalStepMaker.finished);
        yield return new WaitForSeconds(TIME_BETWEEN_STEPS);

        SpeedManager.Reset();
    }
}
