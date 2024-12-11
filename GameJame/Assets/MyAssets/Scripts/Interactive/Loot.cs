using System;
using UnityEngine;

public class Loot : MonoBehaviour, IInteractive
{
    [SerializeField] private LootData lootData;
    
    public void Interact()
    {
        Debug.Log("Interact");
    }

    // Эта логика вообще походу не сюда
    void OnMouseEnter() { /* Подсветка */ }
    void OnMouseExit() { /* Убираем подсветку */ }

    public LootData GetLootData => lootData;
}
