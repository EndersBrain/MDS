using System.Collections;
using UnityEngine;

public class EndPointMistery : MonoBehaviour //Codul pentru un mistery endpoint care verifica daca patratul 3x3 este cel dorit si afiseaza un popup daca este corect
{
    private Endpoint endpointComponent;
    void Start()
    {
        GameObject endpointObject = GameObject.Find("EndPoint");
        endpointComponent = endpointObject.GetComponent<Endpoint>();
    }
    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {
        yield return new WaitForSeconds(0.6f);

        BigCubeController bigCube = other.GetComponent<BigCubeController>();
        if (bigCube == null)
            yield break;
        bool ok = true;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (bigCube.visibleGrid[i, j] != endpointComponent.matrix[i].values[j])
                {
                    ok = false;
                    break;
                }
            }
            if (!ok)
                break;
        }
        if (!ok)
        {
            Destroy(other.gameObject);
        }
        else
        {
            endpointComponent.ShowPopup();
        }
    }
}
