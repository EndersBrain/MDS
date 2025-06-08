using UnityEngine;

public class ColliderPosition : MonoBehaviour
{
    private void Update()
    {
        LineRenderer lineRenderer = GetComponentInParent<LineRenderer>();

        if (lineRenderer == null)
        {
            return;
        }

        if (lineRenderer.positionCount == 0)
        {
            return;
        }

        Vector3 startPoint = lineRenderer.GetPosition(0);
        //Debug.Log("Start point of LineRenderer: " + startPoint);
        transform.position = startPoint;

        if (startPoint != Vector3.zero)
            enabled = false;
    }
}
