using System;
using System.Collections.Generic;
using UnityEngine;



public enum EventObjState
{
    NeedToFix,Fixed
}

[CreateAssetMenu(fileName = "EventData", menuName = "Scriptable Objects/EventData")]

/// <summary>
/// Описание для мини квестов
/// и основных квестов
/// <summary>
public class EventData : ScriptableObject
{
    [SerializeField] private String eventName;
    [SerializeField] private Vector3 eventPositionInHolder;
    [SerializeField] private Vector3 eventRotationInHolder;
    
    [Header("Links")]
    [SerializeField] private GameObject interactObjectPrefab;
    [SerializeField] private AudioClip interactObjectSound;
    [SerializeField] private AudioClip cantInteractSound; // если с объектом невозможно взаимодействовать
    [SerializeField] private Animator interactObjectAnimator;
    
    ///<summary>мини евенты которые должны быть выполнены до основного</summary>>
    [SerializeField] private List<EventData> datasEventsToFixFirst;
    [SerializeField] private EventObjState eventObjState = EventObjState.NeedToFix;
    
    [SerializeField] private LootData dataLootToFix;
    
    public String GetEventName => eventName;
    public Vector3 GetEventPositionInHolder => eventPositionInHolder;
    public Vector3 GetEventRotationInHolder => eventRotationInHolder;
    
    public GameObject GetInteractObjectPrefab => interactObjectPrefab;
    public AudioClip GetInteractObjectSound => interactObjectSound;
    public AudioClip GetCantInteractSound => cantInteractSound;
    public Animator GetInteractObjectAnimator => interactObjectAnimator;
    public List<EventData> GetDatasEventsToFixFirst => datasEventsToFixFirst;
    public EventObjState GetEventObjState => eventObjState;
    
    public void SetEventObjState(EventObjState value) => eventObjState = value;
    
    
    
}
