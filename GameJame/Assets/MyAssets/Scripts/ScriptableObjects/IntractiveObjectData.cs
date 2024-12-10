using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntractiveObjectData", menuName = "Scriptable Objects/IntractiveObjectData")]
public class IntractiveObjectData : ScriptableObject
{
    [SerializeField] private String interactObjectName;
    [SerializeField] private String interactObjectDescription;
    
    
    //[SerializeField] private Sprite lootIcon;
    
    [Header("Links")]
    [SerializeField] private GameObject interactObjectPrefab;
    [SerializeField] private AudioClip interactObjectSound;
    [SerializeField] private AudioClip cantInteractSound; // если с объектом невозможно взаимодействовать
    [SerializeField] private Animator interactObjectAnimator;
    
    public String GetInteractObjectName => interactObjectName;
    public String GetInteractObjectDescription => interactObjectDescription;
    public GameObject GetInteractObjectPrefab => interactObjectPrefab;
    public AudioClip GetInteractObjectSound => interactObjectSound;
    public AudioClip GetCantInteractSound => cantInteractSound;
    public Animator GetInteractObjectAnimator => interactObjectAnimator;
    
}
