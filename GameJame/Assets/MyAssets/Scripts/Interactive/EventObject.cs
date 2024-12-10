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

    [SerializeField] private Renderer objectRenderer;
    public EventObjState State=>state;


    private void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        ChangeView();
    }
    public override void Interact()
    {
        base.Interact();
        if (state == EventObjState.Fixed) { return; }
        else
        {
            //foreach (LootData toInteract in lootDatasList)
            //{
            //    //TODO: проверяем совпадает ли лут в руках с одним из лутов для взаимодействия с объектом
            //}
            EventObjectManager.Instance.FixedCountPlus();
            ChangeState();
            
        }
        
    }
    public void ChangeState()
    {
        state= EventObjState.Fixed;
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
}
