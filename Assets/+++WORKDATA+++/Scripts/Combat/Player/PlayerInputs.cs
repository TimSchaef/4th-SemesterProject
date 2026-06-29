using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    private InputSystem_Actions _inputActions;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _lookAction;
    private InputAction _interactAction;
    private InputAction _shootAction;
    private InputAction _shootTwoAction;

    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private bool _shootInput;
    private bool _shootTwoInput;
    private bool _jumpInput;
    private bool _interactInput;

    public Vector2 LookInput => _lookInput;
    public Vector2 MoveInput => _moveInput;

    public bool ShootInput => _shootInput;
    public bool ShootTwoInput => _shootTwoInput;

    public bool JumpInput => _jumpInput;


    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _moveAction = _inputActions.Player.Move;
        _jumpAction = _inputActions.Player.Jump;
        _lookAction = _inputActions.Player.Look;
        _interactAction = _inputActions.Player.Interact;
        _shootAction = _inputActions.Player.Shoot;
        _shootTwoAction = _inputActions.Player.ShootTwo;
    }

    void OnEnable()
    {
        EnableInput();
        SubscribeInputs();
    }

    private void OnDisable()
    {
        DisableInput();
        UnsubscribeInputs();
    }

    private void SubscribeInputs()
    {        
        _interactAction.performed += Interact;
        
        _lookAction.performed += Look;
        _lookAction.canceled += Look;
        
        _moveAction.performed += Move;
        _moveAction.canceled += Move;

        _shootAction.started += Shoot;
        _shootAction.canceled += Shoot;

        _shootTwoAction.started += ShootTwo;
        _shootTwoAction.canceled += ShootTwo;
        
        _jumpAction.performed += Jump;
        _jumpAction.canceled += Jump;
    }

    private void UnsubscribeInputs()
    {
        _interactAction.performed -= Interact;
        
        _lookAction.performed -= Look;
        _lookAction.canceled -= Look;
        
        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;

        _shootAction.started -= Shoot;
        _shootAction.canceled -= Shoot;
        
        _shootTwoAction.started -= ShootTwo;
        _shootTwoAction.canceled -= ShootTwo;
        
        _jumpAction.performed -= Jump;
        _jumpAction.canceled -= Jump;
    }

    public void EnableInput()
    {
        _inputActions.Enable();
    }
    
    public void DisableInput()
    {
        _inputActions.Disable();
    }

    public void DisableZiplineInput()
    {
        
    }

    public void EnableZiplineInput()
    {
        
    }
    
       private void Interact(InputAction.CallbackContext ctx)
        {
            PlayerInteraction.OnInteract?.Invoke();
            Debug.Log("Interact");
        }
        
    private void Look(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

    private void Move(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        _shootInput = context.ReadValueAsButton();
    }

    private void ShootTwo(InputAction.CallbackContext context)
    {
        _shootTwoInput = context.ReadValueAsButton();
    }

    private void Jump(InputAction.CallbackContext context)
    {
        _jumpInput = context.ReadValueAsButton();
    }
}
