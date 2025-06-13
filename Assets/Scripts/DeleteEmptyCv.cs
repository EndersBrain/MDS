using UnityEngine;

public class DeleteEmptyCv : MonoBehaviour
{
    private float initializeTime;
    private float lifetime = 3f;
    private LineRenderer lineRenderer;

    void Start()
    {
        initializeTime = Time.time;
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Time.time > initializeTime + lifetime)
        {
            if (lineRenderer.positionCount == 0)
            {
                Debug.Log("Conveyor belt destroyed due to no points.");
                Destroy(gameObject);
            }
        }
    }
}