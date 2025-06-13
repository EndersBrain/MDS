using UnityEngine;

[System.Serializable]
public class DecorOption
{
    public Sprite sprite;
    public float weight = 1f;
}

public class Decorator : MonoBehaviour // Codul pentru decorarea fundalului jocului cu obiecte decorative, ai niste procentaje de aparitie a fiecarui obiect decorativ in fundal
{
    public GameObject decorPrefab;
    public DecorOption[] decorOptions;
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float spacing = 1f;
    public Vector2 gridOrigin = new Vector2(-20f, -20f);
    public float placementChance = 0.2f;
    void Start()
    {
        PlaceDecor();
    }

    void PlaceDecor()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (Random.value <= placementChance)
                {
                    Vector2 worldPos = new Vector2(
                        gridOrigin.x + x * spacing + spacing * 0.5f,
                        gridOrigin.y + y * spacing + spacing * 0.5f
                    );

                    if (IsAreaOccupied(worldPos, 3))
                        continue;
                    GameObject obj = Instantiate(decorPrefab, worldPos, Quaternion.identity, transform);
                    obj.GetComponent<SpriteRenderer>().sprite = GetRandomWeightedSprite();
                }
            }
        }
    }

    bool IsAreaOccupied(Vector2 center, int radius)// Verifica daca zona din jurul unui punct este ocupata de alte obiecte
    {
        for (int dx = -radius; dx <= radius; dx++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                Vector2 checkPos = new Vector2(
                    center.x + dx * spacing,
                    center.y + dy * spacing
                );

                if (Physics2D.OverlapPoint(checkPos) != null)
                    return true;
            }
        }
        return false;
    }

    Sprite GetRandomWeightedSprite()// Alege un sprite decorativ in functie de procentajul de aparitie al fiecarui obiect decorativ
    {
        float sansa = 0f;
        foreach (var option in decorOptions)
            sansa += option.weight;
        float randomValue = Random.value * sansa;
        float cumulative = 0f;
        foreach (var option in decorOptions)
        {
            cumulative += option.weight;
            if (randomValue <= cumulative)
                return option.sprite;
        }
        return decorOptions[0].sprite;
    }
}
