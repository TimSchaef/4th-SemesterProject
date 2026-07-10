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
    [SerializeField] private Image fadeImage;

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
        Time.timeScale = 1f;
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
            Time.timeScale = 1f;
            playerInputs.EnableInput();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pausePanelCanvasGroup.interactable = false;
            pausePanelCanvasGroup.blocksRaycasts = false;

            pausePanelCanvasGroup
                .DOFade(0, panelDuration)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    pausePanel.SetActive(false);
                });
        }
        else
        {
            pausePanel.SetActive(true);

            pausePanelCanvasGroup.interactable = true;
            pausePanelCanvasGroup.blocksRaycasts = true;

            playerInputs.DisableInput();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;

            pausePanelCanvasGroup
                .DOFade(1, panelDuration)
                .SetUpdate(true);
        }
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameCoroutine());
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator RestartGameCoroutine()
    {
        fadeImage.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
