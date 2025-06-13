using UnityEngine;

public class BigCubeController : MonoBehaviour //Codul pentru patratul 3x3 care contine cuburile mici
{
    public SpriteRenderer[,] smallCubes = new SpriteRenderer[3, 3];
    public bool[,] visibleGrid = new bool[3, 3]
    {
        { true, true, true },
        { true, true, true },
        { true, true, true }
    };
    void Awake()//Initializarea patratelor mici
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                int index = row * 3 + col;
                string childName = "Cub" + index;
                Transform child = transform.Find(childName);
                if (child != null)
                    smallCubes[row, col] = child.GetComponent<SpriteRenderer>();
            }
        }
    }
    public void UpdateVisibility()//Actualizarea vizibilitatii cuburilor mici
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if (smallCubes[row, col] != null)
                {
                    smallCubes[row, col].gameObject.SetActive(visibleGrid[row, col]);
                }
            }
        }
    }
    void Start()
    {
        UpdateVisibility();
    }
}

