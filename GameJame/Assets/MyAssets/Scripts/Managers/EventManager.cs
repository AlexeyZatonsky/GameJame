using System.Collections.Generic;
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
                eventObject.GetEventData().SetEventObjState(EventObjState.NeedToFix);
                eventObject.ChangeView();
                continue;
            }

            float randomValue = Random.Range(0f, 1f);
            Debug.LogError("чиcло = " + randomValue + " для " + eventObject.GetEventData().GetEventName);
            
            if (randomValue <= FixedEventChance)
                eventObject.GetEventData().SetEventObjState(EventObjState.Fixed);
            else
                eventObject.GetEventData().SetEventObjState(EventObjState.NeedToFix);
            
            eventObject.ChangeView();
        }
    }
}