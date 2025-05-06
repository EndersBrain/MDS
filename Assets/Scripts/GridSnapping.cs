using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D))]
public class GridSnapping2D : MonoBehaviour
{
    public int widthInCells = 1;
    public int heightInCells = 1;

    private Tilemap _tilemap;

    void Awake()
    {
        _tilemap = FindAnyObjectByType<Tilemap>();
        if (_tilemap == null)
            Debug.LogError("No Tilemap found in scene!");
    }

    public void SnapToGrid(Vector3 rawWorldPos)
    {
        Vector3Int cell = _tilemap.WorldToCell(rawWorldPos);

        // Offset so multi-cell footprints center properly
        float halfX = (widthInCells - 1) * 0.5f;
        float halfY = (heightInCells - 1) * 0.5f;
        Vector3Int anchor = new Vector3Int(
            cell.x - Mathf.FloorToInt(halfX),
            cell.y - Mathf.FloorToInt(halfY),
            cell.z
        );

        // Snap your object’s pivot to the exact center of that block of cells
        transform.position = _tilemap.GetCellCenterWorld(anchor);
    }

    void OnMouseUp() => SnapToGrid(transform.position);
}
