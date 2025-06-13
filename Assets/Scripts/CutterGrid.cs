using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class CutterGrid : MonoBehaviour // Codul pentru grila de butoane care controleaza taierea cuburilor mici
{
    public GameObject buttonPrefab;
    public GameObject gridPanel;
    public Cutter cutter;
    private bool[,] selectedGrid = new bool[3, 3];
    private Button[,] buttonGrid = new Button[3, 3];
    void Start()
    {
        GenerateGridButtons();
        gridPanel.SetActive(false);
    }
    public void ToggleGrid()
    {
        bool ok = false;
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            GameObject selected = EventSystem.current.currentSelectedGameObject;
            foreach (Button btn in buttonGrid)
            {
                if (btn != null && btn.gameObject == selected)
                {
                    ok = true;
                    break;
                }
            }
        }
        if (!ok)
        {
            gridPanel.SetActive(!gridPanel.activeSelf);
        }
    }

    void GenerateGridButtons()// Generarea butoanelor in grila 3x3
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                GameObject btnObj = Instantiate(buttonPrefab, gridPanel.transform);
                Button btn = btnObj.GetComponent<Button>();
                Text btnText = btnObj.GetComponentInChildren<Text>();
                btn.onClick.RemoveAllListeners();
                int r = row;
                int c = col;
                btn.image.color = Color.black;
                buttonGrid[r, c] = btn;

                btn.onClick.AddListener(() =>
                {
                    selectedGrid[r, c] = !selectedGrid[r, c];
                    cutter.ToggleTaieri(r, c);
                    btn.image.color = selectedGrid[r, c] ? Color.white : Color.black;
                    EventSystem.current.SetSelectedGameObject(null);
                });
            }
        }
    }
}
