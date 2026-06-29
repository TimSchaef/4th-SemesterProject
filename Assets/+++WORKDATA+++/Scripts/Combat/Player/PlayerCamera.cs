using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 10f;
    [SerializeField] private Transform cameraPivot;
    
    private float xRotation = 0f;
    private Vector2 mouseDelta;
    private PlayerInputs _playerInputs;

    void Awake()
    {
        _playerInputs = GetComponent<PlayerInputs>();
    }
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        mouseDelta = _playerInputs.LookInput;
        Look();
    }

    void Look()
    {
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
