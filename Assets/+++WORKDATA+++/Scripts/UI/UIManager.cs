using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private GameObject endPanel;
    [SerializeField] private CanvasGroup endPanelCanvasGroup;
    [SerializeField] private PlayerInputs playerInputs;

    [Header("Values")] 
    [SerializeField] private float endPanelDuration = 1f;


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
        endPanelCanvasGroup.DOFade(1, endPanelDuration);
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
