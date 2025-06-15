using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EndpointTest : MonoBehaviour
{
    public GameObject popupPanel;
    public Button nextLevelButton;
    public Button tryAgainButton;
    public TMP_Text progressText;
    [System.Serializable]
    public class BoolRow
    {
        public bool[] values = new bool[3];
    }

    public BoolRow[] matrix = new BoolRow[3]
    {
        new BoolRow(),
        new BoolRow(),
        new BoolRow()
    };

    public int nr;
    public int incercate = 0;

    public BigCubeController bigCubeController;

    private void Start()
    {
        UpdateBigCubeVisibility();
        progressText.text = $"Received {0} / {nr} of expected";
        progressText.color = Color.cyan;
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("CubEndpoint"))
            return;
        BigCubeController bigCube = other.GetComponent<BigCubeController>();
        if (bigCube == null)
        {
            return;
        }
        bool ok = true;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (bigCube.visibleGrid[i, j] != matrix[i].values[j])
                {
                    ok = false;
                    break;
                }
            }
        }
        if (ok == false)
        {
            Destroy(bigCube.gameObject);
            Vector3 centerPos = transform.position + new Vector3(0f, 0.2f, -1.1f);
            GameObject redQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            redQuad.name = "RedQuad";
            redQuad.transform.position = centerPos;
            redQuad.transform.rotation = Quaternion.identity;
            redQuad.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            Material redMat = new Material(Shader.Find("Unlit/Color"));
            redMat.color = Color.red;
            Renderer rend = redQuad.GetComponent<Renderer>();
            rend.material = redMat;
            rend.sortingOrder = 32767;

            progressText.text = $"Test failed!";
            progressText.color = Color.red;
            return;
        }
        else if (nr != incercate)
        {
            incercate++;
            Destroy(bigCube.gameObject);
            if (progressText != null)
            {
                progressText.text = $"Received {incercate} / {nr} of expected"; ;
                progressText.color = Color.cyan;
            }
        }
        if (nr == incercate)
        {

            Destroy(bigCube.gameObject);
            Vector3 centerPos = transform.position + new Vector3(0f, 0.2f, -1.1f);
            GameObject redQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            redQuad.name = "RedQuad";
            redQuad.transform.position = centerPos;
            redQuad.transform.rotation = Quaternion.identity;
            redQuad.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            Material greenMat = new Material(Shader.Find("Unlit/Color"));
            greenMat.color = Color.green;
            Renderer rend = redQuad.GetComponent<Renderer>();
            rend.material = greenMat;
            rend.sortingOrder = 32767;
            progressText.text = $"Test Passed!";
            progressText.color = Color.green;
            return;
        }
        Destroy(bigCube.gameObject);
    }

    public void UpdateBigCubeVisibility()
    {
        if (bigCubeController != null)
        {
            bool[,] visibility = new bool[3, 3];
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    visibility[r, c] = matrix[r].values[c];
                }
            }
            bigCubeController.visibleGrid = visibility;
            bigCubeController.UpdateVisibility();
        }
    }
    public static bool[,] RotateMatrix90Clockwise(bool[,] original)
    {
        bool[,] rotated = new bool[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                rotated[j, 2 - i] = original[i, j];
            }
        }
        return rotated;
    }
}