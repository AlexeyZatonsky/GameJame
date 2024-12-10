using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки ходьбы")]
    [SerializeField] private float walkSpeed = 2f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float smoothMoveTime = 0.1f;
    [SerializeField] private float smoothStopTime = 0.5f;

    [Header("Настройки приседания")]
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float crouchCenter = 0.5f;
    [SerializeField] private float standHeight = 1.8f;
    [SerializeField] private float standCenter = 0.9f;

    [Header("Настройки прыжка")]
    [SerializeField] private float jumpHeight = 2.0f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.3f;
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, 0.1f, 0);
    [SerializeField] private Color groundCheckGizmoColor = Color.red;
    [SerializeField] private bool showGroundCheckGizmo = true;

    [Header("Настройки диагонального движения")]
    [SerializeField] private float diagonalSpeedCoefficient = 1.41f;
    [SerializeField] private float animationSmoothTime = 0.1f;

    private Vector2 movementInput;
    private Vector3 currentDirection;
    private Vector3 targetDirection;
    private Vector3 velocity;

    private float currentSpeed;
    private float targetSpeed;

    private Transform cameraRootTransform;
    private CharacterController characterController;
    private Animator animator;

    private bool isRunning = false;
    private bool isCrouching = false;
    private bool isGrounded = true;

    private void Awake()
    {
        FindAllNeedComponents();
    }

    private void Update()
    {
        UpdateMovement();
        UpdateJump();
        UpdateAnimation();
    }

    private void FindAllNeedComponents()
    {
        var playerCamera = GetComponentInChildren<PlayerCamera>();
        if (playerCamera != null)
        {
            cameraRootTransform = playerCamera.transform;
        }

        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void UpdateMovement()
    {
        movementInput = InputManager.Instance.GetMovementInput();
        isRunning = InputManager.Instance.IsRunning();

        targetDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        targetSpeed = movementInput.magnitude > 0 ? (isRunning ? runSpeed : walkSpeed) : 0;

        if (movementInput.magnitude > 0)
        {
            Vector3 forward = cameraRootTransform.forward;
            forward.y = 0;
            forward.Normalize();
            Vector3 right = cameraRootTransform.right;
            right.y = 0;
            right.Normalize();

            Vector3 adjustedDirection = (forward * targetDirection.z + right * targetDirection.x).normalized;

            currentDirection = Vector3.Lerp(currentDirection, adjustedDirection, Time.deltaTime / smoothMoveTime);
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / smoothMoveTime);
        }
        else
        {
            currentDirection = Vector3.Lerp(currentDirection, Vector3.zero, Time.deltaTime / smoothStopTime);
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime / smoothStopTime);
        }

        Vector3 cameraForward = cameraRootTransform.forward;
        cameraForward.y = 0;
        transform.rotation = Quaternion.LookRotation(cameraForward, Vector3.up);

        Vector3 movement = currentDirection * currentSpeed * Time.deltaTime;

        characterController.Move(movement);

        if (isRunning && isCrouching)
        {
            UpdateCrouchState();
        }
    }


    private void UpdateJump()
    {
        Vector3 spherePosition = transform.position + groundCheckOffset;

        Debug.DrawLine(transform.position, spherePosition, Color.yellow);

        isGrounded = Physics.CheckSphere(spherePosition, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        /*
        if (isGrounded && InputManager.Instance.IsJump())
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        */

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateCrouchState()
    {
        isCrouching = !isCrouching;

        if (isCrouching)
        {
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0, crouchCenter, 0);
        }
        else
        {
            characterController.height = standHeight;
            characterController.center = new Vector3(0, standCenter, 0);
        }

        animator.SetBool("IS_Crouch", isCrouching);
    }

    private void UpdateAnimation()
    {
        Vector3 movement = currentDirection * currentSpeed;
        Vector3 localVelocity = transform.InverseTransformDirection(movement);

        float adjustedForwardBackwardSpeed = localVelocity.z;
        float adjustedLeftRightSpeed = localVelocity.x;

        if (Mathf.Abs(targetDirection.x) > 0 && Mathf.Abs(targetDirection.z) > 0)
        {
            adjustedForwardBackwardSpeed *= diagonalSpeedCoefficient;
            adjustedLeftRightSpeed *= diagonalSpeedCoefficient;
        }

        animator.SetFloat("FB_Velocity", adjustedForwardBackwardSpeed, animationSmoothTime, Time.deltaTime);
        animator.SetFloat("LR_Velocity", adjustedLeftRightSpeed, animationSmoothTime, Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        if (!showGroundCheckGizmo) return;

        Gizmos.color = groundCheckGizmoColor;
        Gizmos.DrawWireSphere(transform.position + groundCheckOffset, groundCheckRadius);

        if (Application.isPlaying && isGrounded)
        {
            Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius * 0.5f);
        }
    }
}