using System;
using System.Collections.Generic;
using UnityEngine;

public enum EventObjState
{
    NeedToFix,Fixed
}

public class EventObject : InteractiveObject
{
    [SerializeField] private List<LootData> lootDatasList = new List<LootData>();
    [SerializeField] private EventObjState state;
    [SerializeField] private IntractiveObjectData interactiveObjectData;

    [SerializeField] private Renderer objectRenderer;
    public EventObjState State => state;


    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        ChangeView();
    }

    public override void Interact()
    {
        base.Interact();
        if (state == EventObjState.Fixed)
        {
            return;
        }
        else
        {
            //foreach (LootData toInteract in lootDatasList)
            //{
            //    //TODO: ��������� ��������� �� ��� � ����� � ����� �� ����� ��� �������������� � ��������
            //}

            // I dont understand chto za voprositel]nue znaki
            EventObjectManager.Instance.FixedCountPlus();
            ChangeState();

        }

    }

    public void ChangeState()
    {
        state = EventObjState.Fixed;
        canInteract = false;
        ChangeView();
        Debug.Log("Fixed");
    }

    public void ChangeView()
    {
        if (objectRenderer != null)
        {
            if (state == EventObjState.Fixed)
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
        Debug.Log(interactiveObjectData.GetInteractObjectName);
        //String objectDescription = interactiveObjectData.GetInteractObjectDescription;
        float TestDistance = 3f;

        if (state == EventObjState.Fixed)
        {
            return;
        }

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
