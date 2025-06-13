using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Endpoint : MonoBehaviour // Codul pentru gestionarea punctului final al nivelului
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
        progressText.text = $"{0} / {nr}";
        progressText.color = Color.cyan;
        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) // Logica pentru gestionarea coliziunii cu patratele mari
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
            Destroy(bigCube.gameObject); //Daca gaseste patratul, il distruge
        }
        else if (nr != incercate) //Daca gaseste ce cauta, incrementeaza numarul de incercari
        {
            incercate++;
            Destroy(bigCube.gameObject);
            if (progressText != null)
            {
                progressText.text = $"{incercate} / {nr}";
                progressText.color = Color.cyan;
            }
        }
        if (nr == incercate)
        {
            ShowPopup();
        }
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
    public void ShowPopup() // Afisarea pop-up-ului la finalul nivelului, pentru a oferi optiuni jucatorului
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);

            nextLevelButton.onClick.RemoveAllListeners();
            tryAgainButton.onClick.RemoveAllListeners();

            nextLevelButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            });

            tryAgainButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            });
        }
    }
}