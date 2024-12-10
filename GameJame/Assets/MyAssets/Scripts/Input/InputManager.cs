using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private PlayerInputs playerInput;

    private InputAction playerMovementAction;
    private InputAction playerRunAction;
    private InputAction playerCameraRotateAction;
    private InputAction playerInteractAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        playerInput = new PlayerInputs();

        playerMovementAction = playerInput.Player.Move;
        playerRunAction = playerInput.Player.Run;
        playerCameraRotateAction = playerInput.Player.CameraRotate;
        playerInteractAction = playerInput.Player.Interact;
    }

    private void OnEnable()
    {
        playerMovementAction.Enable();
        playerRunAction.Enable();
        playerCameraRotateAction.Enable();
        playerInteractAction.Enable();
    }

    private void OnDisable()
    {
        playerMovementAction.Disable();
        playerRunAction.Disable();
        playerCameraRotateAction.Disable();
        playerInteractAction.Disable();
    }


    public Vector2 GetMovementInput() => playerMovementAction.ReadValue<Vector2>();
    public Vector2 GetCameraRotateInput() => playerCameraRotateAction.ReadValue<Vector2>();
    public bool IsRunning() => playerRunAction.IsPressed();
    public bool IsInteract() => playerInteractAction.WasPressedThisFrame();
}