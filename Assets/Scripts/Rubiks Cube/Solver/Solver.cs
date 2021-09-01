using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver
{
    public static IEnumerator Start(RubiksCube rubiksCube)
    {
        float oldSpeed = Face.rotatingSpeed;
        Face.rotatingSpeed = 675f;

        // Step makers
        WhiteCrossMaker whiteCrossMaker = new WhiteCrossMaker(rubiksCube);
        EdgesMaker edgesMaker = new EdgesMaker(rubiksCube);
        WhiteCornersMaker whiteCornersMaker = new WhiteCornersMaker(rubiksCube);
        SecondCrownMaker secondCrownMaker = new SecondCrownMaker(rubiksCube);
        
        // Step 1
        rubiksCube.StartCoroutine(whiteCrossMaker.Work());
        yield return new WaitUntil(() => whiteCrossMaker.finished);

        // Step 2
        rubiksCube.StartCoroutine(edgesMaker.Work());
        yield return new WaitUntil(() => edgesMaker.finished);

        // Step 3
        rubiksCube.StartCoroutine(whiteCornersMaker.Work());
        yield return new WaitUntil(() => whiteCornersMaker.finished);
        Debug.Log("hey");
        yield return new WaitForSeconds(3f);

        // Step 4
        rubiksCube.StartCoroutine(secondCrownMaker.Work());
        yield return new WaitUntil(() => secondCrownMaker.finished);

        Face.rotatingSpeed = oldSpeed;
    }
}
