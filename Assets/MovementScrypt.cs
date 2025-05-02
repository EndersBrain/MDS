using UnityEngine;

public class MovementScrypt : MonoBehaviour
{
    Camera cam;
    Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        cam= Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {

        Vector3 mouseWorld= cam.ScreenToWorldPoint( Input.mousePosition );
        mouseWorld.z=transform.position.z;
        offset= transform.position-mouseWorld;
    }
    private void OnMouseDrag()
    {
        Vector3 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;
        transform.position = mouseWorld+offset;
    }
    void OnMouseUp()
    {
        // Snap to grid if you have a GridSnapping2D component
        var gridSnap = GetComponent<GridSnapping2D>();
        if (gridSnap != null)
            gridSnap.SnapToGrid(transform.position);
    }
}
