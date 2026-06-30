using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_UI : MonoBehaviour
{
    public static Weapon_UI  instance;
    [SerializeField] private Weapon weaponRef;
    
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] public Image reloadImage;
    [SerializeField] public Image healthImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        reloadImage.gameObject.SetActive(false);
    }
    
    public void UpdateAmmo()
    {
        ammoText.text = weaponRef.ammo.ToString();
    }

}
