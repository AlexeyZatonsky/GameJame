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

    private void Awake()
    {
        HideCursor();
        FindPlayerHead();
        FindCameraRoot();
    }

    private void LateUpdate()
    {
        if (playerHeadTransform == null) return;

        UpdateCamPosAndRot();
    }

    private void UpdateCamPosAndRot()
    {
        cameraRootTransform.position = playerHeadTransform.position;

        Vector2 rotateInput = InputManager.Instance.GetCameraRotateInput();
        float mouseX = rotateInput.x * sensitivity * 0.25f;
        float mouseY = rotateInput.y * sensitivity * 0.25f;

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);
        transform.parent.Rotate(Vector3.up * mouseX);
        cameraRootTransform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
