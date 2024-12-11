using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Ссылки на UI инвентаря")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private RawImage itemIconPanel;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    private void OnEnable()
    {
        playerInventory.OnItemChanged += UpdateUI;
    }

    private void OnDisable()
    {
        playerInventory.OnItemChanged -= UpdateUI;
    }

    private void UpdateUI(Loot loot)
    {
        if (loot == null)
        {
            inventoryPanel.SetActive(false);
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemIconPanel.texture = null;
            itemIconPanel.gameObject.SetActive(false);
        }
        else
        {
            inventoryPanel.SetActive(true);
            itemNameText.text = loot.GetLootData.GetLootName;
            itemDescriptionText.text = loot.GetLootData.GetLootDescription;
            itemIconPanel.texture = loot.GetLootData.GetLootIcon.texture;
            itemIconPanel.gameObject.SetActive(true);
        }
    }
}
