using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CutterButtonFollower : MonoBehaviour //Codul pentru a face butonul de taiere sa urmeze pozitia cutter-ului in spatiu
{
    public Transform cutterTransform;
    public RectTransform buttonRect;
    public Camera cam;

    void Update()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(cutterTransform.position);
        buttonRect.position = screenPos;
    }
}