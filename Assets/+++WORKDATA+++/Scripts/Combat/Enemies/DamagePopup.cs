using TMPro;
using DG.Tweening;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    
    [SerializeField] private float scaleDuration = 0.15f;
    [SerializeField] private float moveDuration = 0.7f;
    [SerializeField] private float fadeDuration = 0.7f;

    public void Show(float damage)
    {
        text.text = damage.ToString();
        transform.LookAt(Camera.main.transform);
        
        text.alpha = 1;
        text.color = Color.white;

        transform.localScale = Vector3.zero;

        transform.DOScale(0.01f, scaleDuration);
        transform.DOMoveY(transform.position.y + 1, moveDuration);

        text.DOFade(0.3f, fadeDuration)
            .OnComplete(() =>
            {
                DamageManager.Instance.Return(this);
            });
    }
}