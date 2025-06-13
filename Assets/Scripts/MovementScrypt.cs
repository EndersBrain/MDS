using UnityEngine;

public class MovementScrypt : MonoBehaviour // Codul pentru a face obiectul sa fie miscat cu mouse-ul si sa se alinieze cu gridul
{
    private Camera cam;
    private Vector3 offset;
    private CameraController cameraController;
    private bool drag = false;
    void Awake()
    {
        cam = Camera.main;
        cameraController = FindAnyObjectByType<CameraController>();
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pozMouse = cam.ScreenToWorldPoint(Input.mousePosition);
            pozMouse.z = transform.position.z;
            offset = transform.position - pozMouse;
            Collider2D col = GetComponent<Collider2D>();
            if (col != null && col.OverlapPoint(pozMouse))
                drag = true;
        }
        if (Input.GetMouseButton(1) && drag)
        {
            Vector3 pozMouse = cam.ScreenToWorldPoint(Input.mousePosition);
            pozMouse.z = transform.position.z;
            transform.position = pozMouse + offset;
        }
        if (Input.GetMouseButtonUp(1) && drag)
        {
            Vector3 posBeforeSnap = transform.position;
            if (cameraController != null)
            {
                Vector3 snappedPos = cameraController.GetSnappedPosition(transform.position);
                snappedPos.z = transform.position.z;
                transform.position = snappedPos;
            }
            drag = false;
        }
    }
}