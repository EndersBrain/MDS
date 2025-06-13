using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour //Controlul camerei, avem zoom in/out, miscari stanga dreapta, sus-jos
{
    [Header("Zoom Settings")]
    public float zoomSpeed = 5f;
    public float minZoom = 2f;
    public float maxZoom = 20f;
    public float panSpeed = 0.5f;
    public Material gridMaterial;
    public float pixelsPerUnit = 100f;
    public float baseGridSpacing = 64f;
    public bool snapToCenter = true;
    private Camera cam;
    private Vector3 dragOrigin;
    public float gridWorldSpacing = 2.0f;
    void Start()
    {
        gridMaterial = new Material(gridMaterial);
        cam = GetComponent<Camera>();
        cam.orthographic = true;
        UpdateGrid();
        SnapAllObjects();
    }
    void Update()
    {
        HandleZoom();
        HandlePan();
    }
    void HandleZoom()//Controlul zoom-ului, scroll up/down
    {
        if (InputBlocker.blockScrollZoom)
            return;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Input.mousePosition);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += mouseWorldBefore - mouseWorldAfter;
            UpdateGrid();
        }
    }

    void HandlePan()//Controlul camerei, click si drag pentru a muta camera
    {
        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position += difference;
        }
    }
    void UpdateGrid()
    {
        if (gridMaterial != null)
        {
            float worldSpacing = gridWorldSpacing;
            float worldLineWidth = Mathf.Clamp(1.0f / pixelsPerUnit, 0.01f, 0.1f);
            gridMaterial.SetFloat("_GridSpacing", worldSpacing);
            gridMaterial.SetFloat("_LineWidth", worldLineWidth);
        }
    }
    public Vector3 GetSnappedPosition(Vector3 rawPosition)//Functie care returneaza pozitia snapuita a unui obiect, in functie de grid-ul setat
    {
        float cellSize = gridWorldSpacing;
        float halfCell = cellSize * 0.5f;
        float gridOriginX = Mathf.Floor(cam.transform.position.x / cellSize) * cellSize;
        float gridOriginY = Mathf.Floor(cam.transform.position.y / cellSize) * cellSize;
        float snappedX;
        float snappedY;
        if (snapToCenter)
        {
            snappedX = gridOriginX + (Mathf.Round((rawPosition.x - gridOriginX - halfCell) / cellSize) * cellSize) + halfCell;
            snappedY = gridOriginY + (Mathf.Round((rawPosition.y - gridOriginY - halfCell) / cellSize) * cellSize) + halfCell;
        }
        else
        {
            snappedX = gridOriginX + (Mathf.Round((rawPosition.x - gridOriginX) / cellSize) * cellSize);
            snappedY = gridOriginY + (Mathf.Round((rawPosition.y - gridOriginY) / cellSize) * cellSize);
        }
        Vector3 snapped = new Vector3(snappedX, snappedY, rawPosition.z);
        return snapped;
    }
    public void SnapAllObjects()//Functie de test, snapuieste toate obiectele cu tag-ul "Snappable" si "Spawner" la grid-ul setat
    {
        GameObject[] snappables = GameObject.FindGameObjectsWithTag("Snappable");
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        foreach (var obj in snappables)
        {
            Vector3 snappedPos = GetSnappedPosition(obj.transform.position);
            obj.transform.position = snappedPos;
        }
        foreach (var obj in spawners)
        {
            Vector3 snappedPos = GetSnappedPosition(obj.transform.position);
            obj.transform.position = snappedPos;
        }
    }
}
