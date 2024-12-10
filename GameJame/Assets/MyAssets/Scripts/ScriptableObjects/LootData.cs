using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "LootData", menuName = "Scriptable Objects/LootData")]
public class LootData : ScriptableObject
{
    [SerializeField] private String lootName; 
    [SerializeField] private String lootDescription; 
    
    
    //[SerializeField] private Sprite lootIcon;
    
    [Header("Links")]
    [SerializeField] private GameObject lootPrefab; 
    [SerializeField] private AudioClip lootSoundTake; 
    [SerializeField] private Animator lootAnimator; 
    [SerializeField] private IntractiveObjectData objectToInteract; 
    
    
    public String GetLootName => lootName;
    public String GetLootDescription => lootDescription;
    public GameObject GetLootPrefab => lootPrefab;
    public AudioClip GetLootSoundTake => lootSoundTake;
    public Animator GetLootAnimator => lootAnimator;
    public IntractiveObjectData GetLObjectToInteract => objectToInteract;
}
