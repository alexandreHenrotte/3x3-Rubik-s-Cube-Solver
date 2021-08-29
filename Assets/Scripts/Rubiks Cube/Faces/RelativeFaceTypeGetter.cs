using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RelativeFaceTypeGetter
{
    public static Face.FaceType GetRelativeLeft(Face.FaceType faceType)
    {
        switch (faceType)
        {
            case Face.FaceType.FRONT:
                return Face.FaceType.LEFT;
            case Face.FaceType.LEFT:
                return Face.FaceType.REAR;
            case Face.FaceType.REAR:
                return Face.FaceType.RIGHT;
            case Face.FaceType.RIGHT:
                return Face.FaceType.FRONT;
            default:
                throw new Exception($"{faceType} is not a valid horizontal face type");
        }
    }

    public static Face.FaceType GetRelativeRight(Face.FaceType faceType)
    {
        switch (faceType)
        {
            case Face.FaceType.FRONT:
                return Face.FaceType.RIGHT;
            case Face.FaceType.LEFT:
                return Face.FaceType.FRONT;
            case Face.FaceType.REAR:
                return Face.FaceType.LEFT;
            case Face.FaceType.RIGHT:
                return Face.FaceType.REAR;
            default:
                throw new Exception($"{faceType} is not a valid horizontal face type");
        }
    }

    public static Face.FaceType GetRelativeRear(Face.FaceType faceType)
    {
        switch (faceType)
        {
            case Face.FaceType.FRONT:
                return Face.FaceType.REAR;
            case Face.FaceType.LEFT:
                return Face.FaceType.RIGHT;
            case Face.FaceType.REAR:
                return Face.FaceType.FRONT;
            case Face.FaceType.RIGHT:
                return Face.FaceType.LEFT;
            default:
                throw new Exception($"{faceType} is not a valid horizontal face type");
        }
    }
}
