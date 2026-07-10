using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePausePanel();
    }

    public void ShowEndPanel()
    {
        playerInputs.DisableInput();
        endPanel.SetActive(true);
        endPanelCanvasGroup.DOFade(1, panelDuration);
    }

    public void TogglePausePanel()
    {
        bool isOpen = pausePanel.activeSelf;

        if (isOpen)
        {
            pausePanelCanvasGroup.DOFade(0, panelDuration).OnComplete(() =>
                {
                    pausePanel.SetActive(false);
                });

            pausePanelCanvasGroup.interactable = false;
            pausePanelCanvasGroup.blocksRaycasts = false;

            playerInputs.EnableInput();
        }
        else
        {
            pausePanel.SetActive(true);

            pausePanelCanvasGroup.DOFade(1, panelDuration);

            pausePanelCanvasGroup.interactable = true;
            pausePanelCanvasGroup.blocksRaycasts = true;

            playerInputs.DisableInput();
        }
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
