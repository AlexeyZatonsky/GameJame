using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private List<EventObject> events;

    [SerializeField] private GameObject eventsPull;
    

    [SerializeField] private int FixedEventChance = 10;

    public List<EventObject> GetEvents => events;

    

    protected override void Awake()
    {
        base.Awake();

        EventObject[] local_events = GetComponentsInChildren<EventObject>();
        events.AddRange(eventsPull.GetComponentsInChildren<EventObject>());
        
        
        setRandomStateToEvents();
    }

    


    private void setRandomStateToEvents()
    {
        
        if (events.Count < 1) Debug.LogError("EventManager: setRandomStateToEvents: events.Count < 1");
        
        for (int i = 0; i <= events.Count() - 1; i++)
        {
            //Debug.LogError("EnableEventManager : SetRandomStateToEvents : Foreach");
            if (events[i].GetEventData().GetDatasEventsToFixFirst.Count > 0)
            {
                setOriginState(events[i], EventObjState.NeedToFix);
                continue;
            }

            int randomValue = 100;//Random.Range(0, 100);

            if (FixedEventChance == randomValue)
                setOriginState(events[i], EventObjState.Fixed);
            else
                setOriginState(events[i], EventObjState.NeedToFix);
        }
    }
    
    private void setOriginState(EventObject eventObject, EventObjState state)
    {
        if (state == EventObjState.Fixed)
        {
            if(eventObject.GetEventData().GetEventObjState == EventObjState.Fixed)
                 return;
            
            eventObject.ChangeStateFix();
            events.Remove(eventObject);
            return;
        }
        
        // if (eventObject.GetEventData().GetEventObjState == EventObjState.NeedToFix)
        //     return;
        
        eventObject.ChangeStateNeedFix();
        
    }
}