using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    [SerializeField] private Transform holdPoint; // Точка, где держится предмет (например, в руке)
    [SerializeField] private float interactionDistance = 2f; // Максимальная дистанция подбора
    [SerializeField] private Transform playerCamera; // Камера игрока для определения направления взгляда

    private GameObject heldItem = null; // Предмет, который игрок держит

    private void Update()
    {
        // Проверка на нажатие кнопки взаимодействия
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldItem == null)
            {
                TryPickUp();
            }
            else
            {
                DropItem();
            }
        }
    }

    private void TryPickUp()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        // Проверяем, попал ли луч в предмет
        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Loot lootItem = hit.collider.GetComponent<Loot>();
            if (lootItem != null)
            {
                PickUpItem(lootItem);
            }
        }
    }

    private void PickUpItem(Loot lootItem)
    {
        // Сохраняем ссылку на предмет
        heldItem = lootItem.gameObject;

        // Отключаем физику для предмета
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Перемещаем предмет в точку удержания
        heldItem.transform.SetParent(holdPoint);
        heldItem.transform.localPosition = Vector3.zero;
        heldItem.transform.localRotation = Quaternion.identity;
    }

    private void DropItem()
    {
        if (heldItem != null)
        {
            // Открепляем предмет
            heldItem.transform.SetParent(null);

            // Включаем физику
            Rigidbody rb = heldItem.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = false;

            // Добавляем небольшую силу для броска вперед
            rb?.AddForce(holdPoint.forward * 2f, ForceMode.Impulse);

            Debug.Log($"Dropped: {heldItem.name}");

            // Очищаем ссылку
            heldItem = null;
        }
    }
}
