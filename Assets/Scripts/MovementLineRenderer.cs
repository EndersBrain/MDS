using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class ConveyorPlacer : MonoBehaviour
{
    private GameObject currentPreview;
    private LineRenderer currentLine;
    public List<Vector3> points = new();
    private bool isPlacing = false;

    void Start()
    {
        currentLine = GetComponent<LineRenderer>();
        currentLine.positionCount = 0;
        currentLine.useWorldSpace = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Item selected = ToolBarManager.instance.GetSelectedItem(false);
            if (selected != null && selected.type == ItemType.ConveyorBelt)
            {
                if (isPlacing)
                {
                    CancelPlacement();
                    Debug.Log("Stopped conveyor belt placement.");
                }
                else
                {
                    isPlacing = true;
                    StartPlacing(selected);
                    Debug.Log("Started conveyor belt placement.");
                }
            }
        }

        if (!isPlacing || EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.RoundToInt(mousePos.x);
            mousePos.y = Mathf.RoundToInt(mousePos.y);
            mousePos.z = 0f;

            int pcnt = points.Count;

            // if(pcnt > 0)
            //     Debug.Log(points[pcnt - 1]);

            if(pcnt == 0 || points[pcnt - 1].x == mousePos.x || points[pcnt - 1].y == mousePos.y)
            {
                points.Add(mousePos);
                currentLine.positionCount = points.Count;
                currentLine.SetPositions(points.ToArray());
                Debug.Log(points[pcnt - 1]);
            }

            // points.Add(mousePos);
            // currentLine.positionCount = points.Count;
            // currentLine.SetPositions(points.ToArray());
        }

        if (Input.GetMouseButtonDown(1))
        {
            FinishPlacement();
            Debug.Log("Placement canceled.");
        }
    }

    void StartPlacing(Item item)
    {
        if (currentPreview != null)
            Destroy(currentPreview);
        currentPreview = Instantiate(item.prefabToPlace);
        currentLine = currentPreview.GetComponent<LineRenderer>();
        points.Clear();
        currentLine.positionCount = 0;
    }

    void FinishPlacement()
    {
        isPlacing = false;
        currentPreview = null;
        currentLine = null;
        points.Clear();
    }

    void CancelPlacement()
    {
        if (currentPreview != null)
            Destroy(currentPreview);

        isPlacing = false;
        currentPreview = null;
        currentLine = null;
        points.Clear();
        Destroy(currentLine);
    }
}