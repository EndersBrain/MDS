using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour // Codul pentru meniul principal al jocului, care permite incarcarea urmatoarei scene si iesirea din joc
{
    private AsyncOperation asyncLoad;

    private void Start()
    {
        StartCoroutine(PreloadNextScene());
    }

    public void PlayGame()
    {
        if (asyncLoad != null)
            asyncLoad.allowSceneActivation = true;
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator PreloadNextScene()
    {
        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;
        while (asyncLoad.progress < 0.9f)
            yield return null;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}