using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.15f;

    [Header("Camera")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float mouseSensitivity = 0.15f;
    [SerializeField] private float maxLookAngle = 85f;

    private Rigidbody rb;
    private Collider playerCollider;
    private Player_InputRecive input;

    private float cameraPitch;
    private float pendingYaw;
    private bool jumpRequested;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<Collider>();
        input = GetComponent<Player_InputRecive>();

        rb.freezeRotation = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (input.hasJumped) jumpRequested = true;

        HandleCamera();
    }

    private void FixedUpdate()
    {
        Quaternion targetRotation = rb.rotation * Quaternion.Euler(0f, pendingYaw, 0f);

        rb.MoveRotation(targetRotation);
        pendingYaw = 0f;

        Movement(targetRotation);

        if (jumpRequested && IsGrounded()) Jump();

        jumpRequested = false;
    }

    private void Movement(Quaternion orientation)
    {
        Vector3 direction = new Vector3(input.moveInput.x, 0f, input.moveInput.y).normalized;

        direction = orientation * direction;

        Vector3 velocity = rb.linearVelocity;

        velocity.x = direction.x * moveSpeed;
        velocity.z = direction.z * moveSpeed;

        rb.linearVelocity = velocity;
    }

    private void HandleCamera()
    {
        Vector2 look = input.lookInput * mouseSensitivity;

        pendingYaw += look.x;

        cameraPitch -= look.y;

        cameraPitch = Mathf.Clamp( cameraPitch, -maxLookAngle, maxLookAngle);

        cameraHolder.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, whatIsGround);
    }
}
