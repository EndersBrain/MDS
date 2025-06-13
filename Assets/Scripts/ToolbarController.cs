using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarController : MonoBehaviour
{
    public List<Button> slotButtons;
    public List<GameObject> slotPrefabs;
    public List<GameObject> popupObjects;
    public Transform popupParent;
    public Vector3 popupPosition = Vector3.zero;
    public List<bool> staySelected;
    public int selectat = -1;
    public GameObject selectedPrefab = null;
    private GameObject currentPopup = null;
    public static ToolbarController Instance;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            int capturedIndex = i;
            slotButtons[i].onClick.AddListener(() => OnSlotButtonClicked(capturedIndex));
        }
        foreach (GameObject popup in popupObjects)
        {
            if (popup != null)
                popup.SetActive(false);
        }
        UpdateToolbarVisuals();
    }

    void Update()
    {
        for (int i = 0; i < slotButtons.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                AfisareSlot(i);
            }
        }
    }
    public void OnSlotButtonClicked(int index)
    {
        AfisareSlot(index);
    }
    void AfisareSlot(int index)
    {
        if (selectat == index && !staySelected[index])
            DeselectCurrentTool();
        else
            SelectSlot(index);
    }
    public void SelectSlot(int index)
    {
        foreach (GameObject popup in popupObjects)
        {
            if (popup != null)
                popup.SetActive(false);
        }
        selectat = index;
        selectedPrefab = (index >= 0 && index < slotPrefabs.Count) ? slotPrefabs[index] : null;
        if (index >= 0 && index < popupObjects.Count && popupObjects[index] != null)
        {
            popupObjects[index].SetActive(true);
            currentPopup = popupObjects[index];
        }
        UpdateToolbarVisuals();
    }
    public void DeselectCurrentTool()
    {
        if (currentPopup != null)
        {
            currentPopup.SetActive(false);
            currentPopup = null;
        }
        selectat = -1;
        selectedPrefab = null;
        UpdateToolbarVisuals();
    }
    void UpdateToolbarVisuals()
    {
        Color culoare = new Color(0xD6 / 255f, 0xF5 / 255f, 0xDD / 255f, 1f);
        for (int i = 0; i < slotButtons.Count; i++)
        {
            ColorBlock colors = slotButtons[i].colors;
            if (i == selectat)
            {
                colors.normalColor = culoare;
                colors.highlightedColor = culoare;
                colors.pressedColor = culoare * 0.9f;
            }
            else
            {
                colors.normalColor = Color.white;
                colors.highlightedColor = Color.white * 1.1f;
                colors.pressedColor = Color.white * 0.9f;
            }
            slotButtons[i].colors = colors;
        }
    }
}
