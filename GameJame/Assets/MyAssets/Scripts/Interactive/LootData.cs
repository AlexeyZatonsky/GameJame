using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "LootData", menuName = "Scriptable Objects/LootData")]
public class LootData : ScriptableObject
{
    [SerializeField] private String lootName {get => lootName;}
    [SerializeField] private String lootDescription {get => lootDescription;}
    
    
    //[SerializeField] private Sprite lootIcon;
    
    [Header("Links")]
    [SerializeField] private GameObject lootPrefab {get => lootPrefab;}
    [SerializeField] private AudioClip lootSoundTake {get => lootSoundTake;}
    [SerializeField] private Animator lootAnimator {get => lootAnimator;}
    [SerializeField] private IntractiveObjectData objectToInteract {get => objectToInteract;}
    
}
