using System;
using UnityEngine;

[CreateAssetMenu(fileName = "IntractiveObjectData", menuName = "Scriptable Objects/IntractiveObjectData")]
public class IntractiveObjectData : ScriptableObject
{
    [SerializeField] private String InteractObjectName {get => InteractObjectName;}
    [SerializeField] private String InteractObjectDescription {get => InteractObjectDescription;}
    
    
    //[SerializeField] private Sprite lootIcon;
    
    [Header("Links")]
    [SerializeField] private GameObject InteractObjectPrefab {get => InteractObjectPrefab;}
    [SerializeField] private AudioClip InteractObjectSound {get => InteractObjectSound;}
    [SerializeField] private AudioClip CantInteract {get => CantInteract;} // если с объектом невозможно взаимодействовать
    [SerializeField] private Animator InteractObjectAnimator {get => InteractObjectAnimator;}
    
    
}
