using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlacementManager : MonoBehaviour
{
    public Camera cam;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            PlaceSelectedItem();
        }
    }

    void PlaceSelectedItem()
    {
        Item selectedItem = ToolBarManager.instance.GetSelectedItem(true);
        if (selectedItem == null || selectedItem.prefabToPlace == null)
            return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        Instantiate(selectedItem.prefabToPlace, SnapToGrid(mousePos), Quaternion.identity);
    }

    Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
    }
}