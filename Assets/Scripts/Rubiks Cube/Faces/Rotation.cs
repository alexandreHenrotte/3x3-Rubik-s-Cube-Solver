using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Face;

public class Rotation
{
    public bool inversed;
    public bool finished = false;
    public Quaternion quaternionToReach;

    public Rotation(Face face, bool inversed)
    {
        this.inversed = inversed;
        setQuaternionToReach(face, inversed);
    }

    void setQuaternionToReach(Face face, bool inversed)
    {
        int rotationInverter = inversed ? -1 : 1;

        switch (face.rotatingAxe)
        {
            case RotatingAxe.X:
                quaternionToReach = face.rotatingParent.transform.rotation * Quaternion.Euler((90 * rotationInverter), 0, 0);
                break;
            case RotatingAxe.Y:
                quaternionToReach = face.rotatingParent.transform.rotation * Quaternion.Euler(0, (90 * rotationInverter), 0);
                break;
            case RotatingAxe.Z:
                quaternionToReach = face.rotatingParent.transform.rotation * Quaternion.Euler(0, 0, (90 * rotationInverter));
                break;
        }
    }
}
