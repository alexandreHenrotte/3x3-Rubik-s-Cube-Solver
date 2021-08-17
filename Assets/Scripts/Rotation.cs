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
                quaternionToReach = Quaternion.Euler(face.rotatingParent.transform.rotation.eulerAngles.x + (90 * rotationInverter), 0, 0);
                break;
            case RotatingAxe.Y:
                quaternionToReach = Quaternion.Euler(0, face.rotatingParent.transform.rotation.eulerAngles.y + (90 * rotationInverter), 0);
                break;
            case RotatingAxe.Z:
                quaternionToReach = Quaternion.Euler(0, 0, face.rotatingParent.transform.rotation.eulerAngles.z + (90 * rotationInverter));
                break;
        }

        Debug.Log(quaternionToReach);
    }
}
