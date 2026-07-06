using System;
using System.Collections;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerJuice : MonoBehaviour
{
    public static PlayerJuice Instance;

    [SerializeField] private Transform cameraPivot;
    
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilReturnSpeed = 8f;
    [SerializeField] private float recoilKick = 1.2f;
    [SerializeField] private Volume damageVolume;
    
    [Header("Damage Animation")]
    [SerializeField] private float damageAnimationSpeed = 0.05f;

    private float targetValue;

    private bool _isHit;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Update()
    {
        if (damageVolume.weight <= 0f)
        {
            StartCoroutine(DamageAnimation());
        }
        CameraReturnPivot();
    }

    public void GetDamage()
    {
        damageVolume.weight = 1f;
        StartCoroutine(DamageAnimation());
    }

    private IEnumerator DamageAnimation()
    {
        while (damageVolume.weight > 0)
        {
            damageVolume.weight -= 0.05f;
            yield return new WaitForSeconds(0.2f);
        }

        damageVolume.weight = 0;
    }
    public void CameraKick()
    {
        recoilX += recoilKick;
    }
    

    private void CameraReturnPivot()
    {
        recoilX = Mathf.Lerp(recoilX, 0f, recoilReturnSpeed * Time.deltaTime);
        
        cameraPivot.localRotation = Quaternion.Euler(-recoilX, 0, 0f);
    }
}
