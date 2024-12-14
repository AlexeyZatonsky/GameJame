using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class EventObject : InteractiveObject
{
    [SerializeField] private EventData eventData;
    [SerializeField] private PlayerInventory playerInventory;
    
    [SerializeField] private Renderer objectRenderer;

    [SerializeField] private bool _isdoor=false;
    [SerializeField] private ObjMove objMove;


    public EventData GetEventData() => eventData;

    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        ChangeView();
        
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public override void Interact()
    {
        base.Interact();
        
        
        
        if (eventData.GetEventObjState == EventObjState.Fixed)
        {
            Debug.LogError("Fixed");
            if (_isdoor)
            {
                objMove.Move();
            }
            return;
        }
        else
        {
            Debug.LogError("Interacting with EventObject");

            LootData NeedLootData = eventData.GetDataLootToFix;
            LootData currentLootData = playerInventory.GetCurrentItem?.GetLootData;
            List<EventData> eventsToFixFirst = eventData.GetDatasEventsToFixFirst;

            bool isCanFixOfFirstEvents = _canFixOfMiniEvents(eventsToFixFirst);
            bool isCanFixOfLoot = _canFixOfLoot(NeedLootData, currentLootData, isCanFixOfFirstEvents);

            if (isCanFixOfLoot && isCanFixOfFirstEvents)
            {
                SoundManager.Instance.PlaySound(eventData.GetInteractObjectSound);
                ChangeStateFix();
                EventObjectManager.Instance.FixedCountPlus();
                return;
            }
            
            SoundManager.Instance.PlaySound(eventData.GetCantInteractSound);

            
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
        Debug.LogError("EventObject can't be fixed");
        foreach (EventData eventNeedToFix in eventsToFixFirst)
        {
            Debug.LogError("EventObject can't be fixed Foreach");

            if (eventNeedToFix.GetEventObjState == EventObjState.NeedToFix)
            {
                SubtitlesManager.Instance.PlayDialogueNewText("D_Fix", "Сначала почини " + eventNeedToFix.name);
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
    private bool _canFixOfLoot(LootData needLootData, LootData inInventoryLootData, bool canFixOfEvents)
    {
        if(needLootData == null) return true;
        
        if (needLootData != inInventoryLootData)
        {
            Debug.LogError("У вас нет подходящего предмета для починки");
            SubtitlesManager.Instance.PlayDialogueNewText("D_Fix", "У вас нет подходящего предмета для починки");
            return false;
        }
        
        if (canFixOfEvents)
            playerInventory.DestroyItem();
        return true;
    }

    public void ChangeStateFix()
    {
        eventData.SetEventObjState(EventObjState.Fixed);
        canInteract = false;
        ChangeView();
        Debug.Log("Fixed");
    }

    public void ChangeStateNeedFix()
    {
        eventData.SetEventObjState(EventObjState.NeedToFix);
        canInteract = true;
        ChangeView();
        Debug.Log("NeedFix");
    }

    public void ChangeView()
    {
        
        
            if (eventData.GetEventObjState == EventObjState.Fixed)
            {
                objectRenderer.material.color = Color.green;
                return;
            }

            objectRenderer.material.color = Color.red;
            
        

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
