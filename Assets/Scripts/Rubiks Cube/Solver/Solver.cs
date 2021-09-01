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
        yield return whiteCrossMaker.Work() ;
        yield return new WaitForSeconds(2f);

        // Step 2
        yield return edgesMaker.Work();
        yield return new WaitForSeconds(2f);

        // Step 3
        yield return whiteCornersMaker.Work();
        yield return new WaitForSeconds(2f);

        // Step 4
        yield return secondCrownMaker.Work();
        yield return new WaitForSeconds(2f);

        Face.rotatingSpeed = oldSpeed;
    }
}
