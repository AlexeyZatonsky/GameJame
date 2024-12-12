using System;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "LootData", menuName = "Scriptable Objects/LootData")]
public class LootData : ScriptableObject
{
    [SerializeField] private String lootName; 
    [SerializeField] private String lootDescription; 
    [SerializeField] private Sprite lootIcon;
    [SerializeField] private Vector3 lootPositionInHolder;
    [SerializeField] private Vector3 lootRotationInHolder;
    
    
    [Header("Links")]
    [SerializeField] private GameObject lootPrefab; 
    
    [SerializeField] private AudioClip lootSoundTake; 
    [SerializeField] private AudioClip lootDestroySound;
    
    [SerializeField] private Animator lootAnimator;
    [SerializeField] private Animator lootDestroAnimator;
    
    
    public String GetLootName => lootName;
    public String GetLootDescription => lootDescription;
    public Sprite GetLootIcon => lootIcon;
    public Vector3 GetLootPositionInHolder => lootPositionInHolder;
    public Vector3 GetLootRotationInHolder => lootRotationInHolder;
    
    public GameObject GetLootPrefab => lootPrefab;
    
    public AudioClip GetLootSoundTake => lootSoundTake;
    public AudioClip GetLootDestroySound => lootDestroySound;
    
    public Animator GetLootAnimator => lootAnimator;
    public Animator GetLootDestroAnimator => lootDestroAnimator;
}
