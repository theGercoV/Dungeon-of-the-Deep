using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject settingsPanel; // Optionales Settings Panel

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false); // Panel beim Start verstecken
    }

    // Start Game Button
    public void StartGame()
    {
        // Szene per Name oder Index laden
        SceneManager.LoadScene("Level1"); // Name deiner Spielszene
    }

    // Settings öffnen
    public void OpenSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }

    // Settings schließen
    public void CloseSettings()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // Quit Button
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
