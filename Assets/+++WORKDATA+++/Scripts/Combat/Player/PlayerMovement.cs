using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float airControl = 6f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    private CharacterController controller;
    private PlayerInputs inputs;

    private Vector3 velocity;
    private Vector3 knockbackVelocity;

    private bool isGrounded;
    private float lastGroundedTime;

    private RaycastHit groundHit;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        inputs = GetComponent<PlayerInputs>();
    }

    void Update()
    {
        CheckGround();

        HandleMovement();
        HandleJump();
        ApplyGravity();
        ApplyKnockback();

        controller.Move(velocity * Time.deltaTime);
    }

    // ---------------- SMOOTH MOVEMENT ----------------
    void HandleMovement()
    {
        Vector2 input = inputs.MoveInput;

        Vector3 move =
            transform.right * input.x +
            transform.forward * input.y;

        if (isGrounded)
            move = Vector3.ProjectOnPlane(move, groundHit.normal);

        Vector3 targetHorizontal =
            move * speed;

        float control = isGrounded ? acceleration : airControl;

        Vector3 currentHorizontal =
            new Vector3(velocity.x, 0, velocity.z);

        Vector3 smoothed =
            Vector3.Lerp(currentHorizontal, targetHorizontal, control * Time.deltaTime);

        // add knockback influence
        smoothed += new Vector3(knockbackVelocity.x, 0, knockbackVelocity.z);

        velocity.x = smoothed.x;
        velocity.z = smoothed.z;
    }

    // ---------------- JUMP ----------------
    void HandleJump()
    {
        if (isGrounded)
            lastGroundedTime = Time.time;

        bool canJump =
            Time.time - lastGroundedTime <= coyoteTime;

        if (inputs.JumpInput && canJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastGroundedTime = -999f;
        }
    }

    // ---------------- GRAVITY ----------------
    void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
    }

    // ---------------- KNOCKBACK ----------------
    void ApplyKnockback()
    {
        knockbackVelocity =
            Vector3.Lerp(knockbackVelocity, Vector3.zero, 8f * Time.deltaTime);
    }

    public void AddKnockback(Vector3 force)
    {
        force.y = 0f;
        knockbackVelocity += force;
    }

    // ---------------- GROUND CHECK ----------------
    void CheckGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        isGrounded =
            Physics.SphereCast(ray, 0.5f, out groundHit, 0.6f, groundLayer);
    }
}