using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Face
{
    public static float rotatingSpeed = 400f;

    public enum FaceType
    {
        FRONT,
        REAR,
        LEFT,
        RIGHT,
        BOTTOM,
        UP
    }

    public enum Color
    {
        RED,
        ORANGE,
        GREEN,
        BLUE,
        YELLOW,
        WHITE
    }

    public enum RotatingAxe
    {
        X,
        Y,
        Z
    }

    public Row[] rows = { new Row(), new Row(), new Row()};
    public Column[] columns = { new Column(), new Column(), new Column() };
    public FaceUpdater faceUpdater;
    public GameObject rotatingParent;
    public RotatingAxe rotatingAxe;
    public bool rotatingInverted;
    Rotation currentRotation;

    public Face(FaceUpdater faceUpdater, GameObject rotatingParent, RotatingAxe rotatingAxe, bool rotatingInverted)
    {
        this.faceUpdater = faceUpdater;
        this.rotatingParent = rotatingParent;
        this.rotatingAxe = rotatingAxe;
        this.rotatingInverted = rotatingInverted;

        faceUpdater.Init(this);
    }

    public void Rotate(RubiksCube rubiksCube, bool inverted=false)
    {
        if (rubiksCube.AllFacesAreStatic())
        {
            this.currentRotation = new Rotation(this, inverted);
        }  
    }

    public void RotateIfNecessary()
    {
        if (RotationFinished())
        {
            return;
        }

        SetParent();
        DoRotation();
    }

    void SetParent()
    {
        // Rows
        foreach (Row row in rows)
        {
            foreach (GameObject cube in row.cubes)
            {
                cube.transform.SetParent(rotatingParent.transform);
            }
        }
        // Columns
        foreach (Column column in columns)
        {
            foreach (GameObject cube in column.cubes)
            {
                cube.transform.SetParent(rotatingParent.transform);
            }
        }
    }

    void DoRotation()
    {
        rotatingParent.transform.rotation = Quaternion.RotateTowards(rotatingParent.transform.rotation, currentRotation.quaternionToReach, rotatingSpeed * Time.deltaTime);

        if (rotatingParent.transform.rotation == currentRotation.quaternionToReach)
        {
            currentRotation.finished = true;
        }
    }

    public bool RotationFinished()
    {
        return currentRotation == null || currentRotation.finished;
    }

    public GameObject GetCube(int rowNumber, int columnNumber)
    {
        Row row = rows[rowNumber - 1];
        Column column = columns[columnNumber - 1];
        GameObject cube = row.cubes.Intersect(column.cubes).ToArray()[0];
        return cube;
    }

    public List<GameObject> GetAllCubes()
    {
        List<GameObject> cubes = new List<GameObject>();

        // Row
        foreach(Row row in rows)
        {
            foreach(GameObject cube in row.cubes)
            {
                if (!cubes.Contains(cube))
                {
                    cubes.Add(cube);
                }
            }
        }

        // Column
        foreach (Column column in columns)
        {
            foreach (GameObject cube in column.cubes)
            {
                if (!cubes.Contains(cube))
                {
                    cubes.Add(cube);
                }
            }
        }

        return cubes;
    }
}
