using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float interactionDistance = 5f;
    [SerializeField] private LayerMask interactionLayers;

    [SerializeField] private Camera playerCamera;

    private void Awake()
    {
        var playerCameraComponent = GetComponentInChildren<PlayerCamera>();
        if (playerCameraComponent != null)
        {
<<<<<<< Updated upstream
            this.playerCamera = playerCamera.GetComponentInChildren<Camera>();
=======
            playerCamera = playerCameraComponent.GetComponentInChildren<Camera>();
>>>>>>> Stashed changes
        }
    }

    private void Update()
    {
        if (InputManager.Instance.IsInteract())
        {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green,20f);
            //Debug.Log("sdsd");

            if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayers))
            {
<<<<<<< Updated upstream
=======
                IInteractive interactive = hit.collider.GetComponent<IInteractive>();
                if (interactive != null)
                {
                    interactive.Interact();
                    Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red,20f);
                }
>>>>>>> Stashed changes
            }
        }
        
    }
}