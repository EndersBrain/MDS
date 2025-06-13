using UnityEngine;

public class PauseManager : MonoBehaviour // Codul pentru gestionarea pauzei in joc
{
    private bool isPaused = false;
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}