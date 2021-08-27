using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public Face.FaceType faceType;
    public bool isInverted;

    public static Movement Translate(string movementCall)
    {
        Movement movement = new Movement();
        movement.faceType = movementsFaceCalls[movementCall[0]];
        movement.isInverted = IsInverted(movementCall);
        return movement;
    }

    public static Movement[] Translate(string[] movementCalls)
    {
        Movement[] movements = new Movement[movementCalls.Length];
        for (int i = 0; i < movementCalls.Length; i++)
        {
            movements[i] = Translate(movementCalls[i]);
        }
        return movements;
    }

    static Dictionary<char, Face.FaceType> movementsFaceCalls = new Dictionary<char, Face.FaceType>()
    {
        { 'F', Face.FaceType.FRONT },
        { 'B', Face.FaceType.REAR },
        { 'L', Face.FaceType.LEFT },
        { 'R', Face.FaceType.RIGHT },
        { 'D', Face.FaceType.BOTTOM },
        { 'U', Face.FaceType.UP }
    };

    static bool IsInverted(string movement)
    {
        return movement.Contains("i");
    }
}
