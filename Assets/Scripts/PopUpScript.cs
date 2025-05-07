using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel;
    public TextMeshProUGUI objectDataText;
    private int objectData = 0;

    void Start()
    {
        popupPanel.SetActive(false);
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }

    public void TogglePopup()
    {
        popupPanel.SetActive(!popupPanel.activeSelf);
    }

    public void UpdateObjectData(int value)
    {
        objectData = value;
        objectDataText.text = "Data: " + objectData.ToString();
    }
}
