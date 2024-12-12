using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EventObject : InteractiveObject
{
    [SerializeField] private EventData eventData;
    [SerializeField] private PlayerInventory playerInventory;
    
    [SerializeField] private Renderer objectRenderer;
    


    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        ChangeView();
        
        eventData.SetEventObjState(EventObjState.NeedToFix);
        
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public override void Interact()
    {
        base.Interact();
        if (eventData.GetEventObjState == EventObjState.Fixed)
        {
            return;
        }
        else
        {

            LootData NeedLootData = eventData.GetDataLootToFix;
            LootData currentLootData = playerInventory.GetCurrentItem?.GetLootData;
            List<EventData> eventsToFixFirst = eventData.GetDatasEventsToFixFirst;

            bool isCanFixOfLoot = _canFixOfLoot(NeedLootData, currentLootData);
            bool isCanFixOfFirstEvents = _canFixOfMiniEvents(eventsToFixFirst);

            if (isCanFixOfLoot && isCanFixOfFirstEvents)
                ChangeState();
            
        }

    }



    /// <summary>
    /// Чисто приватный метод для проверки
    /// того, выполнил ли игрок все мини евенты перед
    /// финальным 
    /// </summary>
    /// <param name="eventsToFixFirst">SO евентов которые предстоит
    /// выполнить перед основным</param>
    /// <returns>Bool</returns>
    private bool _canFixOfMiniEvents(List<EventData> eventsToFixFirst)
    {
        foreach (EventData eventNeedToFix in eventsToFixFirst)
        {
            if (eventNeedToFix.GetEventObjState == EventObjState.NeedToFix)
            {
                Debug.LogError("Сначала почини" + eventNeedToFix.name);
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Подходит ли лут в руках к луту нужному для починки
    /// </summary>
    /// <param name="needLootData">Нужный лут для починки</param>
    /// <param name="inInventoryLootData">Лут в руках</param>
    /// <returns>Bool</returns>
    private bool _canFixOfLoot(LootData needLootData, LootData inInventoryLootData)
    {
        if(needLootData == null) return true;
        
        if (needLootData != inInventoryLootData)
        {
            Debug.LogError("У вас нет подходящего предмета для починки");
            return false;
        }
        
        playerInventory.DestroyItem();
        return true;
    }

    public void ChangeState()
    {
        eventData.SetEventObjState(EventObjState.Fixed);
        canInteract = false;
        ChangeView();
        Debug.Log("Fixed");
    }

    public void ChangeView()
    {
        if (objectRenderer != null)
        {
            if (eventData.GetEventObjState == EventObjState.Fixed)
            {
                objectRenderer.material.color = Color.green;
            }
            else
            {
                objectRenderer.material.color = Color.red;
            }
        }

    }

    public void OnMouseOver()
    {
        
        Debug.Log("OnMouseOver"); 
        //Debug.Log(interactiveObjectData.GetInteractObjectName);
        //String objectDescription = interactiveObjectData.GetInteractObjectDescription;
        float TestDistance = 3f;

        if (eventData.GetEventObjState == EventObjState.Fixed)
        {
            return;
        }

        // тут можно подсветку ебануть как подсказку
        objectRenderer.material.color = Color.yellow;

        //HintUI.Instance.ShowHint(objectDescription);
    }

    public void OnMouseExit()
    {
        ChangeView(); // Восстановление цвета объекта

        // if (HintUI.Instance != null)
        // {
        //     HintUI.Instance.HideTooltip();
        // }
        // else
        // {
        //     Debug.LogError("HintUI instance is not initialized!");
        // }
    }
    
    
}
