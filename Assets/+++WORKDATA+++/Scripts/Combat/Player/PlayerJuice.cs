using UnityEngine;

public class PlayerJuice : MonoBehaviour
{
    public static PlayerJuice Instance;

    [SerializeField] private Transform cameraPivot;
    
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilReturnSpeed = 8f;
    [SerializeField] private float recoilKick = 1.2f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        CameraReturnPivot();
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
