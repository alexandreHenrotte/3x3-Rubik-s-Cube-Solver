using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCube : MonoBehaviour
{
    public Face frontFace;
    public Face rearFace;
    public Face leftFace;
    public Face rightFace;
    public Face bottomFace;
    public Face upFace;

    // Start is called before the first frame update
    void Start()
    {
        // Create faces
        /*
        this.frontFace = new Face<FrontCube>(Object.FindObjectsOfType<FrontCube>(), GameObject.Find("front-center"));
        this.rearFace = new Face<RearCube>(Object.FindObjectsOfType<RearCube>(), GameObject.Find("rear-center"));
        this.leftFace = new Face<LeftCube>(Object.FindObjectsOfType<LeftCube>(), GameObject.Find("left-center"));
        this.rightFace = new Face<RightCube>(Object.FindObjectsOfType<RightCube>(), GameObject.Find("right-center"));
        this.bottomFace = new Face<BottomCube>(Object.FindObjectsOfType<BottomCube>(), GameObject.Find("bottom-center"));
        this.upFace = new Face<UpCube>(Object.FindObjectsOfType<UpCube>(), GameObject.Find("up-center"));
        */

        const float FACES_ROTATING_SPEED = 100f;

        frontFace = new Face(Face.FaceType.FRONT, GameObject.Find("Front face collider").GetComponent<FaceUpdater>(), GameObject.Find("front-center"), Face.RotatingAxe.Z, FACES_ROTATING_SPEED);
        rearFace = new Face(Face.FaceType.REAR, GameObject.Find("Rear face collider").GetComponent<FaceUpdater>(), GameObject.Find("rear-center"), Face.RotatingAxe.Z, FACES_ROTATING_SPEED);
        leftFace = new Face(Face.FaceType.LEFT, GameObject.Find("Left face collider").GetComponent<FaceUpdater>(), GameObject.Find("left-center"), Face.RotatingAxe.X, FACES_ROTATING_SPEED);
        rightFace = new Face(Face.FaceType.RIGHT, GameObject.Find("Right face collider").GetComponent<FaceUpdater>(), GameObject.Find("right-center"), Face.RotatingAxe.X, FACES_ROTATING_SPEED);
        bottomFace = new Face(Face.FaceType.BOTTOM, GameObject.Find("Bottom face collider").GetComponent<FaceUpdater>(), GameObject.Find("bottom-center"), Face.RotatingAxe.Y, FACES_ROTATING_SPEED);
        upFace = new Face(Face.FaceType.UP, GameObject.Find("Up face collider").GetComponent<FaceUpdater>(), GameObject.Find("up-center"), Face.RotatingAxe.Y, FACES_ROTATING_SPEED);
}

    // Update is called once per frame
    void Update()
    {
        frontFace.RotateIfNecessary();
        rearFace.RotateIfNecessary();
        leftFace.RotateIfNecessary();
        rightFace.RotateIfNecessary();
        bottomFace.RotateIfNecessary();
        upFace.RotateIfNecessary();
    }

    IEnumerator Rotations()
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
    }

    public void Manipulate(string movement)
    {
        switch (movement)
        {
            case "R":
                rightFace.Rotate();
                break;
            case "Ri":
                rightFace.Rotate(true);
                break;
            case "L":
                leftFace.Rotate(true);
                break;
            case "Li":
                leftFace.Rotate();
                break;
            case "B":
                rearFace.Rotate();
                break;
            case "Bi":
                rearFace.Rotate(true);
                break;
            case "D":
                bottomFace.Rotate(true);
                break;
            case "Di":
                bottomFace.Rotate();
                break;
            case "F":
                frontFace.Rotate(true);
                break;
            case "Fi":
                frontFace.Rotate();
                break;
            case "U":
                upFace.Rotate();
                break;
            case "Ui":
                upFace.Rotate(true);
                break;
        }
    }
}
