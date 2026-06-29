using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Ermöglicht das Laden von Szenen

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [Tooltip("Das Hauptpanel mit Start, Settings, Credits und Exit.")]
    [SerializeField] private GameObject mainMenuPanel;
    
    [Tooltip("Das Panel für die Soundoptionen.")]
    [SerializeField] private GameObject settingsPanel;
    
    [Tooltip("Das Panel für die Credits.")]
    [SerializeField] private GameObject creditsPanel;

    [Header("Scene Configuration")]
    [Tooltip("Der exakte Name der Spielszene, die gestartet werden soll.")]
    [SerializeField] private string gameplaySceneName = "GameScene";

    private void Start()
    {
        // Stellt sicher, dass beim Spielstart nur das Hauptmenü sichtbar ist
        OpenMainMenu();
    }

    /// <summary>
    /// Lädt die eigentliche Spielszene.
    /// </summary>
    public void StartGame()
    {
        Debug.Log("Spiel wird gestartet. Lade Szene: " + gameplaySceneName);
        SceneManager.LoadScene(gameplaySceneName);
    }

    /// <summary>
    /// Aktiviert das Hauptmenü und blendet alle anderen Panels aus.
    /// </summary>
    public void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    /// <summary>
    /// Öffnet das Einstellungs-Panel.
    /// </summary>
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }

    /// <summary>
    /// Öffnet das Credits-Panel.
    /// </summary>
    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    /// <summary>
    /// Beendet das Spiel. Funktioniert im fertigen Build.
    /// </summary>
    public void ExitGame()
    {
        Debug.Log("Das Spiel wird beendet...");
        Application.Quit();
    }
}
