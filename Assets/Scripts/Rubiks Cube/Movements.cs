using System.Collections.Generic;

public class Movement
{
    public Face.FaceType faceType;
    public bool isInverted;

    public static Movement Translate(string movementCall, Face.FaceType relativeFrontFace)
    {
        Movement movement = new Movement();
        movement.faceType = GetMovementFaceCalls(relativeFrontFace)[movementCall[0]];
        movement.isInverted = IsInverted(movementCall);
        return movement;
    }

    public static Movement[] Translate(string[] movementCalls, Face.FaceType relativeFrontFace)
    {
        Movement[] movements = new Movement[movementCalls.Length];
        for (int i = 0; i < movementCalls.Length; i++)
        {
            movements[i] = Translate(movementCalls[i], relativeFrontFace);
        }
        return movements;
    }

    static Dictionary<char, Face.FaceType> GetMovementFaceCalls(Face.FaceType relativeFrontFace)
    {
        switch (relativeFrontFace)
        {
            case Face.FaceType.FRONT:
                return new Dictionary<char, Face.FaceType>()
                {
                    { 'F', Face.FaceType.FRONT },
                    { 'B', Face.FaceType.REAR },
                    { 'L', Face.FaceType.LEFT },
                    { 'R', Face.FaceType.RIGHT },
                    { 'D', Face.FaceType.BOTTOM },
                    { 'U', Face.FaceType.UP }
                };

            case Face.FaceType.LEFT:
                return new Dictionary<char, Face.FaceType>()
                {
                    { 'F', Face.FaceType.LEFT },
                    { 'B', Face.FaceType.RIGHT },
                    { 'L', Face.FaceType.REAR },
                    { 'R', Face.FaceType.FRONT },
                    { 'D', Face.FaceType.BOTTOM },
                    { 'U', Face.FaceType.UP }
                };

            case Face.FaceType.REAR:
                return new Dictionary<char, Face.FaceType>()
                {
                    { 'F', Face.FaceType.REAR },
                    { 'B', Face.FaceType.FRONT },
                    { 'L', Face.FaceType.RIGHT },
                    { 'R', Face.FaceType.LEFT },
                    { 'D', Face.FaceType.BOTTOM },
                    { 'U', Face.FaceType.UP }
                };

            case Face.FaceType.RIGHT:
                return new Dictionary<char, Face.FaceType>()
                {
                    { 'F', Face.FaceType.RIGHT },
                    { 'B', Face.FaceType.LEFT },
                    { 'L', Face.FaceType.FRONT },
                    { 'R', Face.FaceType.REAR },
                    { 'D', Face.FaceType.BOTTOM },
                    { 'U', Face.FaceType.UP }
                };

            default:
                throw new System.Exception($"{relativeFrontFace} is not a valid face type for relative positioning");
        }
    }

    static bool IsInverted(string movement)
    {
        return movement.Contains("i");
    }
}
