using UnityEngine;

public class MovementScrypt : MonoBehaviour
{
    Camera cam;
    Vector3 offset;
    
    void Awake()
    {
        cam= Camera.main;
    }

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
        var gridSnap = GetComponent<GridSnapping2D>();
        if (gridSnap != null)
            gridSnap.SnapToGrid(transform.position);
    }
}
