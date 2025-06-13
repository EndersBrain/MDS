using UnityEngine;
using System;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject bigCubePrefab;
    public float spawnInterval = 4f;
    public float moveSpeed = 1f;
    public event Action<GameObject> OnObjectSpawned;
    void Awake()
    {
        if (bigCubePrefab == null)
        {
            return;
        }
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (IsConveyorNearby())
            {
                yield return new WaitForSeconds(0.5f);
                SpawnAndMoveCube();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    bool IsConveyorNearby()
    {
        Vector2[] directions = new Vector2[]
        {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
        };

        foreach (Vector2 dir in directions)
        {
            Vector2 checkPos = new Vector2(transform.position.x, transform.position.y) + dir;
            Collider2D hit = Physics2D.OverlapCircle(checkPos, 0.1f);
            if (hit != null && hit.GetComponent<ConveyorBelt>() != null)
                return true;
        }
        return false;
    }
    void SpawnAndMoveCube()
    {
        Vector3 startPos = transform.position + new Vector3(0, 0, 1f);
        GameObject cube = Instantiate(bigCubePrefab, startPos, Quaternion.identity);
        CubeMover mover = cube.AddComponent<CubeMover>();
        mover.speed = moveSpeed;
        OnObjectSpawned?.Invoke(cube);
    }

}