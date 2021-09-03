using UnityEngine;

public class RowUpdater : MonoBehaviour
{
    Face face;

    [Range(1, 3)]
    public int rowNumber;

    void Start()
    {
        rowNumber -= 1; // The array we use for rows start at 0
    }

    public void Init(Face face)
    {
        this.face = face;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (face != null && other.CompareTag("Cube"))
        {
            if (!face.rows[rowNumber].cubes.Contains(other.gameObject))
            {
                face.rows[rowNumber].cubes.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (face != null && other.CompareTag("Cube"))
        {
            face.rows[rowNumber].cubes.Remove(other.gameObject);
        }
    }
}