using System.Collections;
using UnityEngine;
public class BooleanGridRuntimeRenderer : MonoBehaviour
{
    public float speed;
    private bool[,] grid = new bool[3, 3]
    {
        {true, true, true},
        {true, true, true},
        {true, true, true}
    };
    public GameObject activePrefab;
    public GameObject inactivePrefab;
    public float cellSize = 1.0f;
    private GameObject[,] instantiatedObjects = new GameObject[3, 3];
    private bool isUpdating = false;
    void Start()
    {
        instantiatedObjects = new GameObject[3, 3];
        UpdateGridRendering();
    }
    public void UpdateGridRendering()
    {
        if (isUpdating)
            return;
        StartCoroutine(UpdateGridRenderingCoroutine());
    }
    private IEnumerator UpdateGridRenderingCoroutine()
    {
        isUpdating = true;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (instantiatedObjects[x, y] != null)
                {
                    Destroy(instantiatedObjects[x, y]);
                    instantiatedObjects[x, y] = null;
                }
            }
        }
        yield return new WaitForEndOfFrame();
        Vector3 origin = transform.position;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject prefab = grid[y, x] ? activePrefab : inactivePrefab;
                Vector3 pos = origin + new Vector3((x - 1) * cellSize, (1 - y) * cellSize, 0); //era 2-y
                instantiatedObjects[x, y] = Instantiate(prefab, pos, Quaternion.identity, transform);
                instantiatedObjects[x, y].transform.localScale = new Vector3(cellSize, cellSize, cellSize);
            }
        }
        isUpdating = false;
    }
    public void setCell(int x, int y, bool value)
    {
        this.grid[x, y] = value;
    }
    public bool getCell(int x, int y)
    {
        return grid[x, y];
    }
    public void FillObject()
    {
        grid = new bool[3, 3]
        {
            {true, true, true},
            {true, true, true},
            {true, true, true}
        };
    }
    private void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
}