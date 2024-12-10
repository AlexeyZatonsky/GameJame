using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask interactionLayers;

    private Camera playerCamera;

    private void Awake()
    {
        var playerCamera = GetComponentInChildren<PlayerCamera>();
        if (playerCamera != null)
        {
            this.playerCamera = playerCamera.GetComponentInChildren<Camera>();
        }
    }

    private void Update()
    {
        if (InputManager.Instance.IsInteract())
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayers))
            {
            }
        }
        
    }
}