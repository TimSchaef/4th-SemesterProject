using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private GameObject endPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private CanvasGroup endPanelCanvasGroup;
    [SerializeField] private CanvasGroup pausePanelCanvasGroup;
    [SerializeField] private PlayerInputs playerInputs;

    [FormerlySerializedAs("endPanelDuration")]
    [Header("Values")] 
    [SerializeField] private float panelDuration = 1f;

    private bool isActive;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        endPanel.SetActive(false);
    }

    public void ShowEndPanel()
    {
        playerInputs.DisableInput();
        endPanel.SetActive(true);
        endPanelCanvasGroup.DOFade(1, panelDuration);
    }

    public void OpenPausePanel()
    {
        playerInputs.DisableInput();
        !pausePanel.activeSelf = isActive;
        pausePanelCanvasGroup.DOFade(1, panelDuration);
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
