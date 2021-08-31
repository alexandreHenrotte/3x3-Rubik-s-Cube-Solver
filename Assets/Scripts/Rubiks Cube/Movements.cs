using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    public Face.FaceType faceType;
    public bool isInverted;

    public static Movement Translate(string movementCall, Face.FaceType relativeFrontFace, bool rubiksCubeUpsideDown)
    {
        Movement movement = new Movement();
        movement.faceType = GetMovementFaceCalls(relativeFrontFace, rubiksCubeUpsideDown)[movementCall[0]];
        movement.isInverted = IsInverted(movementCall, rubiksCubeUpsideDown);
        return movement;
    }

    public static Movement[] Translate(string[] movementCalls, Face.FaceType relativeFrontFace, bool rubiksCubeUpsideDown)
    {
        Movement[] movements = new Movement[movementCalls.Length];
        for (int i = 0; i < movementCalls.Length; i++)
        {
            movements[i] = Translate(movementCalls[i], relativeFrontFace, rubiksCubeUpsideDown);
        }
        return movements;
    }

    static Dictionary<char, Face.FaceType> GetMovementFaceCalls(Face.FaceType relativeFrontFace, bool rubiksCubeUpsideDown)
    {
        Dictionary<char, Face.FaceType> movementFaceCalls = new Dictionary<char, Face.FaceType>();

        if (rubiksCubeUpsideDown && (relativeFrontFace == Face.FaceType.LEFT || relativeFrontFace == Face.FaceType.RIGHT))
        {
            // LEFT become RIGHT and RIGHT becomes LEFT
            relativeFrontFace = RelativeFaceTypeGetter.GetRelativeLeft(RelativeFaceTypeGetter.GetRelativeLeft(relativeFrontFace));
        }

        switch (relativeFrontFace)
        {
            case Face.FaceType.FRONT:
                movementFaceCalls.Add('F', Face.FaceType.FRONT );
                movementFaceCalls.Add('B', Face.FaceType.REAR );
                movementFaceCalls.Add('L', Face.FaceType.LEFT );
                movementFaceCalls.Add('R', Face.FaceType.RIGHT);
                break;

            case Face.FaceType.LEFT:
                movementFaceCalls.Add('F', Face.FaceType.LEFT);
                movementFaceCalls.Add('B', Face.FaceType.RIGHT);
                movementFaceCalls.Add('L', Face.FaceType.REAR);
                movementFaceCalls.Add('R', Face.FaceType.FRONT);
                break;

            case Face.FaceType.REAR:
                movementFaceCalls.Add('F', Face.FaceType.REAR);
                movementFaceCalls.Add('B', Face.FaceType.FRONT);
                movementFaceCalls.Add('L', Face.FaceType.RIGHT);
                movementFaceCalls.Add('R', Face.FaceType.LEFT);
                break;

            case Face.FaceType.RIGHT:
                movementFaceCalls.Add('F', Face.FaceType.RIGHT);
                movementFaceCalls.Add('B', Face.FaceType.LEFT);
                movementFaceCalls.Add('L', Face.FaceType.FRONT);
                movementFaceCalls.Add('R', Face.FaceType.REAR);
                break;

            default:
                throw new System.Exception($"{relativeFrontFace} is not a valid face type for relative positioning");
        }

        if (rubiksCubeUpsideDown)
        {
            // UP is BOTTOM and BOTTOM is UP
            movementFaceCalls.Add('U', Face.FaceType.BOTTOM);
            movementFaceCalls.Add('D', Face.FaceType.UP);

            // LEFT becomes RIGHT and RIGHT becomes LEFT
            Face.FaceType copyOfLeftValue = movementFaceCalls['L'];
            movementFaceCalls['L'] = movementFaceCalls['R'];
            movementFaceCalls['R'] = copyOfLeftValue;
        }
        else
        {
            movementFaceCalls.Add('D', Face.FaceType.BOTTOM);
            movementFaceCalls.Add('U', Face.FaceType.UP);
        }

        return movementFaceCalls;
    }

    static bool IsInverted(string movement, bool rubiksCubeUpsideDown)
    {
        return movement.Contains("i");
    }
}
