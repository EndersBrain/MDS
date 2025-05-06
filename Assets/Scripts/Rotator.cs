using UnityEngine;

public class Rotator : MonoBehaviour
{
    public enum RotatorType
    {
        Stanga, Dreapta
    }
    public RotatorType rotatorType;
    private bool[,] v;
    private bool[,] c;
    void Start()
    {
        v = new bool[3, 3];
        c = new bool[3, 3];
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pick"))
        {
            UnityEngine.Debug.Log("PickRotator");
            ConveyorBelt conveyorBelt = Object.FindFirstObjectByType<ConveyorBelt>();
            if (conveyorBelt != null)
            {
                conveyorBelt.RemoveItem(other.transform);
            }
            BooleanGridRuntimeRenderer a = other.GetComponent<BooleanGridRuntimeRenderer>();
            v = new bool[3, 3];
            c = new bool[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    v[i, j] = a.getCell(i, j);
            if (rotatorType == RotatorType.Stanga)
            {
                UnityEngine.Debug.Log("Stanga");
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        c[i, j] = v[j, 2 - i];
            }
            else if (rotatorType == RotatorType.Dreapta)
            {
                UnityEngine.Debug.Log("Dreapta");
                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        c[i, j] = v[2 - j, i];
            }
            Vector3 origin = transform.position;
            other.transform.position = origin + new Vector3(1, 0, 0);
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    other.GetComponent<BooleanGridRuntimeRenderer>().setCell(i, j, c[i, j]);
            other.GetComponent<BooleanGridRuntimeRenderer>().UpdateGridRendering();
        }
    }
    void Update()
    {

    }
}
