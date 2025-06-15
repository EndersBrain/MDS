using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour
{
    private GameObject preview;
    private float currentRotation = 0f;
    private GameObject ultimul;
    private bool skipPreviewUpdateThisFrame = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            skipPreviewUpdateThisFrame = true;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;
            Collider2D col = Physics2D.OverlapPoint(worldPosition);
            if (col != null && col.gameObject != preview)
            {
                Debug.Log("Deleting object: " + col.gameObject.name);
                Destroy(col.gameObject);
            }
            else
            {
                Debug.Log("Nothing to delete at " + worldPosition);
                ToolbarController.Instance.SelectSlot(-1);
                ClearPreview();
                currentRotation = 0f;
            }
        }
        if (skipPreviewUpdateThisFrame)
        {
            skipPreviewUpdateThisFrame = false;
            return;
        }
        if (ToolbarController.Instance.selectedPrefab == null)
        {
            InputBlocker.Instance.blockScrollZoom = false;
            ClearPreview();
            ultimul = null;
            return;
        }
        if (ToolbarController.Instance.selectedPrefab != ultimul || preview == null)
        {
            InputBlocker.Instance.blockScrollZoom = false;
            currentRotation = 0f;
            ultimul = ToolbarController.Instance.selectedPrefab;
            ClearPreview();
            UpdatePreview();
        }
        HandleRotation();
        UpdatePreview();
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;
            if (ToolbarController.Instance.selectedPrefab == null)
                return;
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0f;
            Vector3 snappedPosition = Camera.main.GetComponent<CameraController>().GetSnappedPosition(worldPosition);
            float prefabZ = ToolbarController.Instance.selectedPrefab.transform.position.z;
            snappedPosition.z = prefabZ;
            Instantiate(ToolbarController.Instance.selectedPrefab, snappedPosition, Quaternion.Euler(0, 0, currentRotation));
            bool eConveyor = false;
            if (ToolbarController.Instance.selectat >= 0 && ToolbarController.Instance.selectat < ToolbarController.Instance.staySelected.Count)
                eConveyor = ToolbarController.Instance.staySelected[ToolbarController.Instance.selectat];
            if (!eConveyor)
            {
                ToolbarController.Instance.SelectSlot(-1);
                ClearPreview();
                currentRotation = 0f;
            }
        }
    }

    void HandleRotation()
    {
        bool canRotate = false;
        if (ToolbarController.Instance.selectat >= 0 && ToolbarController.Instance.selectat < ToolbarController.Instance.staySelected.Count)
            canRotate = ToolbarController.Instance.staySelected[ToolbarController.Instance.selectat];
        if (!canRotate)
        {
            InputBlocker.Instance.blockScrollZoom = false;
            return;
        }
        InputBlocker.Instance.blockScrollZoom = true;
        if (Input.GetKeyDown(KeyCode.R))
            RotateBy(-90f);
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.01f)
            RotateBy(90f);
        else if (scroll < -0.01f)
            RotateBy(-90f);
    }
    void RotateBy(float angle)
    {
        currentRotation += angle;
        currentRotation = (currentRotation + 360f) % 360f;
    }
    void UpdatePreview()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0f;
        Vector3 snappedPosition = Camera.main.GetComponent<CameraController>().GetSnappedPosition(worldPosition);
        float prefabZ = ToolbarController.Instance.selectedPrefab.transform.position.z;
        snappedPosition.z = prefabZ;
        if (preview == null)
        {
            preview = Instantiate(ToolbarController.Instance.selectedPrefab, snappedPosition, Quaternion.Euler(0, 0, currentRotation));
            foreach (var collider in preview.GetComponentsInChildren<Collider2D>())
                collider.enabled = false;
            foreach (var button in preview.GetComponentsInChildren<Button>())
                button.interactable = false;
            foreach (var selectable in preview.GetComponentsInChildren<Selectable>())
                selectable.interactable = false;
            foreach (var graphic in preview.GetComponentsInChildren<Graphic>())
                graphic.raycastTarget = false;
        }
        else
        {
            preview.transform.position = snappedPosition;
            preview.transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
        bool shouldShowRed = false;
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            GameObject hitObject = hit.collider.gameObject;
            if (preview != null && hitObject != preview && hitObject.CompareTag("Snappable"))
                shouldShowRed = true;
        }
        SetPreviewAppearance(preview, shouldShowRed);
    }
    void SetPreviewAppearance(GameObject obj, bool isHovering)
    {
        Color color = isHovering ? new Color(1f, 0f, 0f, 0.5f) : new Color(0f, 1f, 0f, 0.5f);
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = color;
        foreach (var childSR in obj.GetComponentsInChildren<SpriteRenderer>())
            childSR.color = color;
    }
    void ClearPreview()
    {
        if (preview != null)
        {
            Debug.Log("destroyed");
            Destroy(preview);
            preview = null;
        }
    }
}