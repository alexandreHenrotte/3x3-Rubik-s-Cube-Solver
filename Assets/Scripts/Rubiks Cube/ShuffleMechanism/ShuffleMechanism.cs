using System.Collections;
using UnityEngine;

public static class ShuffleMechanism
{
    const float SHUFFLING_SPEED = 1500f;
    const int NB_RANDOM_MOVEMENTS = 25;

    public static IEnumerator Shuffle(RubiksCube rubiksCube)
    {
        SpeedManager.Change(SHUFFLING_SPEED);

        string[] possibleMovements = { "R", "Ri", "L", "Li", "B", "Bi", "D", "Di", "F", "Fi", "U", "Ui" };
        for (int i = 0; i < NB_RANDOM_MOVEMENTS; i++)
        {
            int randomMovementIndex = new System.Random().Next(possibleMovements.Length);
            string randomMovement = possibleMovements[randomMovementIndex];
            rubiksCube.Manipulate(randomMovement);
            yield return new WaitUntil(() => rubiksCube.readyToManipulate);
        }

        SpeedManager.Reset();
    }
}
