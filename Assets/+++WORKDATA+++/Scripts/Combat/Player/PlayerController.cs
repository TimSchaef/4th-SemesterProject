using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveSpeed;
    private float jumpHeight;
    private float gravity = -9.81f;
    
    private CharacterController characterController;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
