using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GridOverlay : MonoBehaviour
{
    [Header("Grid Settings")]
    public Shader lineShader;      
    public Color lineColor = Color.white;
    public float cellSize = 1f;
    public int width = 20;
    public int height = 12;
    public int highlightEvery = 5;
    public Color highlightColor = Color.yellow;

    private Material _lineMat;

    void Awake()
    {
        if (lineShader == null)
        {
            Debug.LogError("GridOverlay: please assign a shader.");
            enabled = false;
            return;
        }

        _lineMat = new Material(lineShader)
        {
            hideFlags = HideFlags.HideAndDontSave
        };
    }

    void OnDestroy()
    {
        if (_lineMat != null)
            DestroyImmediate(_lineMat);
    }

    void OnPostRender()
    {
        if (_lineMat == null) return;

        _lineMat.SetColor("_Color", lineColor);
        _lineMat.SetPass(0);

        GL.PushMatrix();

        GL.Begin(GL.LINES);

        for (int x = -width; x <= width; x++)
        {
            bool major = highlightEvery > 0 && x % highlightEvery == 0;
            GL.Color(major ? highlightColor : lineColor);
            float xp = x * cellSize;
            GL.Vertex3(xp, -height * cellSize, 0);
            GL.Vertex3(xp, height * cellSize, 0);
        }

        for (int y = -height; y <= height; y++)
        {
            bool major = highlightEvery > 0 && y % highlightEvery == 0;
            GL.Color(major ? highlightColor : lineColor);
            float yp = y * cellSize;
            GL.Vertex3(-width * cellSize, yp, 0);
            GL.Vertex3(width * cellSize, yp, 0);
        }

        GL.End();
        GL.PopMatrix();
    }
}
