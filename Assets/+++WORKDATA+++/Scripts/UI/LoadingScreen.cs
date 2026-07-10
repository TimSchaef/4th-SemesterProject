using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Image loadingScreen;
    void Start()
    {
        loadingScreen.DOFade(1f, );
    }
}
