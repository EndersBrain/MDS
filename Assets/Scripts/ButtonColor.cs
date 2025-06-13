using UnityEngine;
using UnityEngine.UI;

public class ButtonColor : MonoBehaviour
{
    private Button button;
    private Image buttonImage;
    private Color originalColor;
    private bool isToggled = false;
    public Color toggledColor = Color.green;

    void Start()
    {
        button = GetComponent<Button>();
        buttonImage = button.GetComponent<Image>();
        originalColor = buttonImage.color;
        button.onClick.AddListener(ToggleColor);
    }

    void ToggleColor()
    {
        isToggled = !isToggled;
        buttonImage.color = isToggled ? toggledColor : originalColor;
    }
}
