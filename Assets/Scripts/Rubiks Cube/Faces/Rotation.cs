using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Face;

public class Rotation
{
    public bool finished = false;
    public Quaternion quaternionToReach;

    public Rotation(Face face, bool inverted)
    {
        setQuaternionToReach(face, inverted);
    }

    void setQuaternionToReach(Face face, bool inverted)
    {
        int rotationDirection = face.rotatingInverted ? -1 : 1;
        int currentRotationInverter = inverted ? rotationDirection * -1 : rotationDirection * 1;

        switch (face.rotatingAxe)
        {
            case RotatingAxe.X:
                quaternionToReach = face.rotatingParent.transform.rotation * Quaternion.Euler((90 * currentRotationInverter), 0, 0);
                break;
            case RotatingAxe.Y:
                quaternionToReach = face.rotatingParent.transform.rotation * Quaternion.Euler(0, (90 * currentRotationInverter), 0);
                break;
            case RotatingAxe.Z:
                quaternionToReach = face.rotatingParent.transform.rotation * Quaternion.Euler(0, 0, (90 * currentRotationInverter));
                break;
        }
    }
}
