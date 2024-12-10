using System.Collections.Generic;
using UnityEngine;

public class EventObjectManager : SingletonManager<EventObjectManager>
{
    [SerializeField] private List<EventObject> eventObjectsList;
    [SerializeField] private float ChanceToFixed=50;
    [SerializeField] private int fixedCount = 0;
    
    public int FixedCount => fixedCount;

    void Start()
    {
        //OnTimerStop
        TimeManager.Instance.OnTimerStop += CheckFixed;

        //FindEventObj
        eventObjectsList = new List<EventObject>(FindObjectsOfType<EventObject>());
        foreach (var eventObject in eventObjectsList)
        {
            float randomValue = Random.Range(0f, 100f);
            //Fix with Chance
            if (randomValue < ChanceToFixed)
            {
                eventObject.ChangeState();
                fixedCount++;
                Debug.Log($"EventObject name: {eventObject.name}, fixed");

            }
            Debug.Log($"EventObject name: {eventObject.name}, needtofix");
        }
    }

   public void CheckFixed()
    {
        if (eventObjectsList.Count == fixedCount )
        {
            Debug.Log("Win");
            Debug.Log($"ListCount:{eventObjectsList.Count}, FixedCount:{fixedCount}");
        }
        else if (eventObjectsList.Count - fixedCount <=2 && TimeManager.Instance.HaveChance)
        {
            Debug.Log("Have a Chance");
            Debug.Log($"ListCount:{eventObjectsList.Count}, FixedCount:{fixedCount}");
            TimeManager.Instance.AddTime(10);
        }
        else
        {
            Debug.Log("Lose");
            Debug.Log($"ListCount:{eventObjectsList.Count}, FixedCount:{fixedCount}");
        }
    }

    public void FixedCountPlus() { fixedCount++; }
}
