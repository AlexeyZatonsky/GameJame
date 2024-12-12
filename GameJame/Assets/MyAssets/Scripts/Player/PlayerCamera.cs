using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private Transform playerHeadTransform;
    [SerializeField] private Transform cameraRootTransform;

    [Header("Настройки")]
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    private float cameraPitch = 0f;

    private Vector2 rotateInput;

    private bool canRotate = true;
    private bool canFollowHead = false;

    private void Awake()
    {
        FindPlayerHead();
        FindCameraRoot();
    }

    private void LateUpdate()
    {
        HandleInput();
        UpdateCamPosAndRot();
    }
    
    private void Start()
    {
        GameManager.Instance.OnPlayerStateChanged += ChangeCanRotate;
        GameManager.Instance.OnGameStateChanged += CanFollowHead;
    }   

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerStateChanged -= ChangeCanRotate;
        GameManager.Instance.OnGameStateChanged -= CanFollowHead;
    }

    private void ChangeCanRotate(PlayerState state)
    {
        if (state == PlayerState.Action)
        {
            canRotate = true;
        }
        else
        {
            canRotate = false;
        }
    }

    private void CanFollowHead(GameState state)
    {
        if (state == GameState.InBed)
        {
            canFollowHead = true;
        }
        else
        {
            canFollowHead = false;
        }
    }

    private void HandleInput()
    {
        if (canRotate)
        {
            rotateInput = InputManager.Instance.GetCameraRotateInput();
        }
        else
        {
            rotateInput = Vector2.zero;
        }
    }

    private void UpdateCamPosAndRot()
    {
        if (canRotate)
        {
            cameraRootTransform.position = playerHeadTransform.position;

            float mouseX = rotateInput.x * sensitivity * 0.25f;
            float mouseY = rotateInput.y * sensitivity * 0.25f;

            cameraPitch -= mouseY;
            cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
            transform.parent.Rotate(Vector3.up * mouseX);
            cameraRootTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
        else if (canFollowHead)
        {
            cameraRootTransform.position = playerHeadTransform.position;
            cameraRootTransform.rotation = playerHeadTransform.rotation;
        }
    }

    private void FindCameraRoot()
    {
        PlayerCamera cameraComponent = gameObject.GetComponent<PlayerCamera>();
        if (cameraComponent != null)
        {
            cameraRootTransform = cameraComponent.transform;
        }
    }

    private void FindPlayerHead()
    {
        GameObject headObject = GameObject.FindGameObjectWithTag("PlayerHead");
        if (headObject != null)
        {
            playerHeadTransform = headObject.transform;
        }
    }
}
