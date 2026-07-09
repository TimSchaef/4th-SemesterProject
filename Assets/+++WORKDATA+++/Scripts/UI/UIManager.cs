using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject endPanel;
    [SerializeField] private PlayerInputs playerInputs;

    private void Start()
    {
        endPanel.SetActive(false);
    }

    public void ShowEndPanel()
    {
        playerInputs.DisableInput();
        Time.timeScale = 0f;
        endPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
