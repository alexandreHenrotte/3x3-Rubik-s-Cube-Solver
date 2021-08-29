using System.Collections;
using UnityEngine;

public static class ShuffleMechanism
{
    public static IEnumerator Shuffle(RubiksCube rubiksCube)
    {
        const int NB_RANDOM_MOVEMENTS = 25;
        float oldSpeed = Face.rotatingSpeed;

        Face.rotatingSpeed = 675f;
        string[] possibleMovements = { "R", "Ri", "L", "Li", "B", "Bi", "D", "Di", "F", "Fi", "U", "Ui" };
        for (int i = 0; i < NB_RANDOM_MOVEMENTS; i++)
        {
            int randomMovementIndex = new System.Random().Next(possibleMovements.Length);
            string randomMovement = possibleMovements[randomMovementIndex];
            rubiksCube.Manipulate(randomMovement);
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }
        Face.rotatingSpeed = oldSpeed;
    }
}
