using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Cutter cutter;

    public void SetCutterReference(Cutter c)
    {
        cutter = c;
    }

    public void ButtonClick00() { OnButtonClick(0, 0); }
    public void ButtonClick01() { OnButtonClick(0, 1); }
    public void ButtonClick02() { OnButtonClick(0, 2); }
    public void ButtonClick10() { OnButtonClick(1, 0); }
    public void ButtonClick11() { OnButtonClick(1, 1); }
    public void ButtonClick12() { OnButtonClick(1, 2); }
    public void ButtonClick20() { OnButtonClick(2, 0); }
    public void ButtonClick21() { OnButtonClick(2, 1); }
    public void ButtonClick22() { OnButtonClick(2, 2); }

    public void OnButtonClick(int row, int col)
    {
        
        if (cutter != null)
        {
            cutter.ToggleTaieri(row, col);
        }
    }
}
