using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceUpdater : MonoBehaviour
{
    public Face.FaceType selectedfaceType = new Face.FaceType();
    Face face;
    private RowUpdater[] rowUpdaters = new RowUpdater[3];
    private ColumnUpdater[] columnUpdaters = new ColumnUpdater[3];

    void Start()
    {
        SetUpdaters();
    }

    void SetUpdaters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject group = transform.GetChild(i).gameObject;
            if (group.name == "rows")
            {
                SetRowUpdaters(group);
            }
            else if (group.name == "columns")
            {
                SetColumnUpdaters(group);
            }
        }
    }

    void SetRowUpdaters(GameObject group)
    {
        for (int i = 0; i < group.transform.childCount; i++)
        {
            rowUpdaters[i] = group.transform.GetChild(i).GetComponent<RowUpdater>();
        }
    }

    void SetColumnUpdaters(GameObject group)
    {
        for (int i = 0; i < group.transform.childCount; i++)
        {
            columnUpdaters[i] = group.transform.GetChild(i).GetComponent<ColumnUpdater>();
        }
    }

    public void Init(Face face)
    {
        foreach(RowUpdater rowUpdater in rowUpdaters)
        {
            rowUpdater.Init(face);
        }

        foreach(ColumnUpdater columnUpdater in columnUpdaters)
        {
            columnUpdater.Init(face);
        }
    }
}
