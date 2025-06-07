using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    // using UnityEditor.Search;
    public float speed = 0.1f;
    [SerializeField] private LineRenderer _lineRenderer;
    private List<Transform> items = new List<Transform>();
    private List<float> progres = new List<float>();
    private void Update()
    {
        if (_lineRenderer == null)
            return;
        int n = _lineRenderer.positionCount;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != null)
            {
                progres[i] += 0.1f * Time.deltaTime;
                float lungime = 0f;
                List<float> lun = new List<float>();
                for (int j = 0; j < n - 1; j++)
                {
                    float len = Vector3.Distance(_lineRenderer.GetPosition(j), _lineRenderer.GetPosition(j + 1));
                    lun.Add(len);
                    lungime += len;
                }
                float nr = (progres[i] * lungime) % lungime;
                float x = 0f;
                for (int j = 0; j < n - 1; j++)
                {
                    float len = lun[j];
                    if (nr >= x && nr < x + len)
                    {
                        float segmentLerp = Mathf.InverseLerp(x, x + len, nr);
                        Vector3 start = _lineRenderer.GetPosition(j);
                        Vector3 end = _lineRenderer.GetPosition(j + 1);
                        items[i].position = Vector3.Lerp(start, end, segmentLerp);
                    }
                    x += len;
                }
            }
        }
    }
    public void AddItem(Transform item)
    {
        if (item == null)
            return;
        items.Add(item);
        progres.Add(0f);
    }
    public void RemoveItem(Transform item)
    {
        if (item == null)
            return;
        int index = items.IndexOf(item);
        if (index >= 0)
        {
            items.RemoveAt(index);
            progres.RemoveAt(index);
        }
    }
}
