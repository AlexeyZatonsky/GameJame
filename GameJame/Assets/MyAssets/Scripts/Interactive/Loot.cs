using System;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private LootData lootData;
    
    void OnMouseEnter() { /* Подсветка */ }
    void OnMouseExit() { /* Убираем подсветку */ }
}
