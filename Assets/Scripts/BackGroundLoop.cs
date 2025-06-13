using UnityEngine;
using UnityEngine.UI;

public class BackGroundLoop : MonoBehaviour //Codul pentru fundalul animat al jocului
{
    public RawImage backgroundImage;
    private Vector2 loop = Vector2.zero;
    void Update()
    {
        loop += new Vector2(0.055f, 0f) * Time.deltaTime;
        backgroundImage.uvRect = new Rect(loop, backgroundImage.uvRect.size);
    }
}