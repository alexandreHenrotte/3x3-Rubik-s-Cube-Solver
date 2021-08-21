using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnUpdater : MonoBehaviour
{
    Face face;

    [Range(1, 3)]
    public int columnNumber;

    void Start()
    {
        columnNumber -= 1; // The array we use for rows start at 0
    }

    public void Init(Face face)
    {
        this.face = face;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (face != null && other.CompareTag("Cube"))
        {
            if (!face.columns[columnNumber].cubes.Contains(other.gameObject))
            {
                face.columns[columnNumber].cubes.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (face != null && other.CompareTag("Cube"))
        {
            face.columns[columnNumber].cubes.Remove(other.gameObject);
        }
    }
}