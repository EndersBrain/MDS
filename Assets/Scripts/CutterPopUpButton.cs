using UnityEngine;
using UnityEngine.UI;

public class CutterPopUpButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            PopupController popupController = FindFirstObjectByType<PopupController>();
            if (popupController != null)
            {
                button.onClick.AddListener(popupController.ShowPopup);
            }
            else
            {
                Debug.LogWarning("PopupController not found in scene.");
            }
        }
    }
}
