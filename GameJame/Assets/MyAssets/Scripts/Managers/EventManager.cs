using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



/*
Это маленькие и ключевые эвенты которые нужно закрыть
 маленькие евенты то, что нужно выполнить пользователю для выполнения основного евента
 нужен UI в котором будет показываться дерево заданий

!Основной евент для которого может понадобится loot (пока маленькие евенты не выполнены большой выполнить нельзя)
  |---- мини евент для которого может понадобится loot
  |---- мини евент для которого может понадобится loot
  ...
  
!Основной евент для которого может понадобится loot (пока маленькие евенты не выполнены большой выполнить нельзя)
  ...
*/

/// <summary>
/// Это маленькие и ключевые эвенты которые нужно закрыть
/// маленькие евенты то, что нужно выполнить пользователю
/// для выполнения основного евента
/// Синглтончик 
/// 
/// </summary>
public class EnableEventManager : SingletonManager<EnableEventManager>
{
    ///<summary> основные евенты </summary>
    [SerializeField] private List<EventObject> events = new List<EventObject>();

    [SerializeField] private GameObject eventsPull;

    /// <summary>Актуальные объекты для вывода в UI</summary>
    [SerializeField] private List<EventObject> eventsPush = new List<EventObject>();

    [SerializeField] private float FixedEventChance = 0.5f;



    

    private void Awake()
    {
        events.AddRange(eventsPull.GetComponentsInChildren<EventObject>());
        setRandomStateToEvents();
    }

    


    private void setRandomStateToEvents()
    {
        foreach (EventObject eventObject in events)
        {
            if (eventObject.GetEventData().GetDatasEventsToFixFirst.Count > 0)
            {
                setOriginState(eventObject, EventObjState.NeedToFix);
                continue;
            }

            float randomValue = Random.Range(0f, 1f);
            Debug.LogError("чиcло = " + randomValue + " для " + eventObject.GetEventData().GetEventName);
            
            if (randomValue <= FixedEventChance)
                setOriginState(eventObject, EventObjState.Fixed);
            else
                setOriginState(eventObject, EventObjState.NeedToFix);
        }
    }
    
    private void setOriginState(EventObject eventObject, EventObjState state)
    {
        if (state == EventObjState.Fixed)
        {
            eventObject.ChangeStateFix();
            events.Remove(eventObject);
            eventObject.ChangeView();
            return;
        }
        
        eventObject.ChangeStateNeedFix();
        eventObject.ChangeView();
        
    }
}