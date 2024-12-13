using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private LayerMask interactionLayers;

    private Camera playerCamera;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        var playerCameraComponent = GetComponentInChildren<PlayerCamera>();
        if (playerCameraComponent != null)
        {
            playerCamera = playerCameraComponent.GetComponentInChildren<Camera>();
        }

        playerInventory = GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (InputManager.Instance.IsInteract())
        {
            TryInteract();
        }
    }

    private void TryInteract()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green, 2);

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayers))
        {
            Loot loot = hit.collider.GetComponent<Loot>();
            if (loot != null)
            {
                SoundManager.Instance.PlaySound("Povezlo");
                playerInventory.PickupItem(loot);
                return;
            }

            IInteractive interactive = hit.collider.GetComponent<IInteractive>();
            if (interactive != null)
            {
                interactive.Interact();
                SoundManager.Instance.PlaySound("Zachem");
            }
        }
    }
}