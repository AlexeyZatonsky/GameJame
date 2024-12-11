using System;
using System.Collections.Generic;
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

            if (playerInventory.GetCurrentItem is not null)
            {
                Debug.LogError(playerInventory.GetCurrentItem.ToString());
            }
            
            EventObjectManager.Instance.FixedCountPlus();
            ChangeState();

        }

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
