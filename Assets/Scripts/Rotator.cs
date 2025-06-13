using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject squarePrefab1;
    public float gridWorldSpacing = 0.64f;
    private bool[,] v;
    void Start()
    {
        v = new bool[3, 3];
    }

    void Update()
    {

    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        yield return new WaitForSeconds(0.4f);

        BigCubeController bigCube = other.GetComponent<BigCubeController>();
        if (bigCube == null)
            yield break;
        v = new bool[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                v[i, j] = bigCube.visibleGrid[i, j];
            }
        }
        bigCube.visibleGrid = RotateMatrix90Clockwise(v);
        bigCube.UpdateVisibility();
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(ReenableTrigger());
    }

    private IEnumerator ReenableTrigger()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<Collider2D>().enabled = true;
    }

    private bool[,] RotateMatrix90Clockwise(bool[,] matrix)
    {
        int size = matrix.GetLength(0);
        bool[,] rotated = new bool[size, size];
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                rotated[j, size - 1 - i] = matrix[i, j];
        return rotated;
    }
}
