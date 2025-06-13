using UnityEngine;
using UnityEngine.UIElements;

public class UIHotbarLoader : MonoBehaviour
{
    public UIDocument uiDocument;
    public StyleSheet hotbarStyleSheet;

    void Start()
    {
        if (uiDocument != null && hotbarStyleSheet != null)
        {
            var root = uiDocument.rootVisualElement;
            root.styleSheets.Add(hotbarStyleSheet);

            Debug.Log("[DEBUG] Hotbar style applied!");

            var button1 = root.Q<Button>("HotbarButton1");
            var button2 = root.Q<Button>("HotbarButton2");
            var button3 = root.Q<Button>("HotbarButton3");

            button1.clicked += () => Debug.Log("Factory button clicked!");
            button2.clicked += () => Debug.Log("Saw button clicked!");
            button3.clicked += () => Debug.Log("Rotator button clicked!");
        }
        else
        {
            Debug.LogWarning("[DEBUG] Missing UI Document or StyleSheet!");
        }
    }
}