using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_UI : MonoBehaviour
{
    public static Weapon_UI  instance;
    [SerializeField] private Weapon weaponRef;
    
    [SerializeField] private TextMeshProUGUI ammoText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public void UpdateAmmo()
    {
        ammoText.text = weaponRef.ammo.ToString();
    }

}
