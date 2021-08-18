using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public Dictionary<Face.FaceType, Face> faces = new Dictionary<Face.FaceType, Face>();

    // Start is called before the first frame update
    void Start()
    {
        // Setup center positions
        //GameObject.Find("right-center").transform.position = GameObject.Find("right-center-cube").transform.position;

        // Create logical faces
        faces.Add(Face.FaceType.FRONT, new Face( GameObject.Find("Front face collider").GetComponent<FaceUpdater>(), GameObject.Find("front-center"), Face.RotatingAxe.Y));
        faces.Add(Face.FaceType.REAR, new Face(GameObject.Find("Rear face collider").GetComponent<FaceUpdater>(), GameObject.Find("rear-center"), Face.RotatingAxe.Y));
        faces.Add(Face.FaceType.LEFT, new Face(GameObject.Find("Left face collider").GetComponent<FaceUpdater>(), GameObject.Find("left-center"), Face.RotatingAxe.Z));
        faces.Add(Face.FaceType.RIGHT, new Face(GameObject.Find("Right face collider").GetComponent<FaceUpdater>(), GameObject.Find("right-center"), Face.RotatingAxe.Z));
        faces.Add(Face.FaceType.BOTTOM, new Face(GameObject.Find("Bottom face collider").GetComponent<FaceUpdater>(), GameObject.Find("bottom-center"), Face.RotatingAxe.X));
        faces.Add(Face.FaceType.UP, new Face(GameObject.Find("Up face collider").GetComponent<FaceUpdater>(), GameObject.Find("up-center"), Face.RotatingAxe.X));
}

    // Update is called once per frame
    void Update()
    {
        foreach (Face face in faces.Values)
        {
            face.RotateIfNecessary();
        }
    }

    }

    {
        yield return new WaitForSeconds(3);
        Manipulate("R");
        yield return new WaitForSeconds(3);
        Manipulate("Ri");
        yield return new WaitForSeconds(3);
        Manipulate("L");
        yield return new WaitForSeconds(3);
        Manipulate("Li");
        yield return new WaitForSeconds(3);
        Manipulate("B");
        yield return new WaitForSeconds(3);
        Manipulate("Bi");

    public bool AllFacesAreStatic()
    {
        foreach (Face face in faces.Values)
        {
            if (!face.RotationFinished())
            {
                return false;
            }
        }

        return true;
    }

    public void Manipulate(string movement)
    {
        switch (movement)
        {
            case "R":
                faces[Face.FaceType.RIGHT].Rotate(this, true);
                break;
            case "Ri":
                faces[Face.FaceType.RIGHT].Rotate(this);
                break;
            case "L":
                faces[Face.FaceType.LEFT].Rotate(this);
                break;
            case "Li":
                faces[Face.FaceType.LEFT].Rotate(this, true);
                break;
            case "B":
                faces[Face.FaceType.REAR].Rotate(this, true);
                break;
            case "Bi":
                faces[Face.FaceType.REAR].Rotate(this);
                break;
            case "D":
                faces[Face.FaceType.BOTTOM].Rotate(this, true);
                break;
            case "Di":
                faces[Face.FaceType.BOTTOM].Rotate(this);
                break;
            case "F":
                faces[Face.FaceType.FRONT].Rotate(this);
                break;
            case "Fi":
                faces[Face.FaceType.FRONT].Rotate(this, true);
                break;
            case "U":
                faces[Face.FaceType.UP].Rotate(this);
                break;
            case "Ui":
                faces[Face.FaceType.UP].Rotate(this, true);
                break;
        }
    }
}
