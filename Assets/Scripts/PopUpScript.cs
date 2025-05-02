using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    public GameObject popupPanel; // Panelul care conține butoanele
    public TextMeshProUGUI objectDataText; // Textul unde vom afișa datele obiectului
    private int objectData = 0; // Exemplu de date care vor fi modificate

    void Start()
    {
        popupPanel.SetActive(false); // Inițial ascundem pop-up-ul
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true); // Activează pop-up-ul
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false); // Dezactivează pop-up-ul
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
