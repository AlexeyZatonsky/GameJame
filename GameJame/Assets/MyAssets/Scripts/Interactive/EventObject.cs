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
            // TODO: Проверка лута в руках из PlayerInventory.GetCurrentItem

          
            LootData currentLootData = playerInventory.GetCurrentItem?.GetLootData;
        
            List<EventData> eventsToFixFirst = eventData.GetDatasEventsToFixFirst;

            if (eventsToFixFirst.Count == 0)
                ChangeState();
            
            foreach (EventData eventNeedToFix in eventsToFixFirst)
            {
                if (eventNeedToFix.GetEventObjState == EventObjState.NeedToFix)
                {
                    Debug.LogError(eventNeedToFix.name + " is not fixing ");
                }
                else
                {
                    ChangeState();
                }
            }

            
            

        }

    }

    public void ChangeState()
    {
            /*if (eventNeedToFix.GetEventObjState == EventObjState.NeedToFix)
            {
                Debug.LogError(eventNeedToFix.name + " is not fixing ");
                return;
            }*/
        
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
