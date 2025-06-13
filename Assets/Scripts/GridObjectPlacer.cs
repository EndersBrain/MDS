using UnityEngine;

public class GridObjectPlacer : MonoBehaviour
{
    [SerializeField] private GameObject placeablePrefab;
    private CameraController cameraController;
    private Camera mainCamera;

    void Start()
    {
        Debug.Log("merg1e");
        cameraController = FindObjectOfType<CameraController>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        Debug.Log("merge");
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetMouseWorldPosition();
            Vector3 snappedPos = cameraController.GetSnappedPosition(mousePos);
            snappedPos.z = 0f; // Force to Z=0 plane
            Debug.Log(placeablePrefab.tag);
            if (placeablePrefab.CompareTag("Spawner"))
            {
                snappedPos.z = -1f; // Spawner in front
            }
            Instantiate(placeablePrefab, snappedPos, Quaternion.identity);
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Debug.Log("m3erge");
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = -mainCamera.transform.position.z; // For orthographic camera
        return mainCamera.ScreenToWorldPoint(mouseScreenPos);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Debug.Log("merge");
        Debug.Log("aici");
        if (!Application.isPlaying) return;

        if (cameraController == null) cameraController = FindObjectOfType<CameraController>();
        if (mainCamera == null) mainCamera = Camera.main;

        float cellSize = cameraController.baseGridSpacing / cameraController.pixelsPerUnit;
        Gizmos.color = new Color(0, 1, 1, 0.3f);

        Vector3 mousePos = GetMouseWorldPosition();
        Vector3 snappedPos = cameraController.GetSnappedPosition(mousePos);
        snappedPos.z = 0f;
        Debug.Log(placeablePrefab.tag);
        if (placeablePrefab.CompareTag("Spawner"))
        {
            snappedPos.z = -1f;
        }
        Gizmos.DrawWireCube(snappedPos, Vector3.one * cellSize * 0.95f);
    }
#endif
}