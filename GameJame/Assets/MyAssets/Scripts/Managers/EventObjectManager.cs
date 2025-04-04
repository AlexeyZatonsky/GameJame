using System.Collections.Generic;
using UnityEngine;

public class EventObjectManager : SingletonManager<EventObjectManager>
{
    [SerializeField] private List<EventObject> eventObjectsList = new List<EventObject>();
    [SerializeField] private int ChanceToFixed=50;
    [SerializeField] private int fixedCount = 0;
    

    void Start()
    {
        //FindEventObj
        eventObjectsList = new List<EventObject>(FindObjectsOfType<EventObject>());
        /*
        foreach (var eventObject in eventObjectsList)
        {
            int randomValue = Random.Range(0, 100);
            //Fix with Chance
            if (randomValue < ChanceToFixed)
            {
                //eventObject.ChangeState();
                fixedCount++;
                Debug.Log($"EventObject name: {eventObject.name}, fixed");

            }
            Debug.Log($"EventObject name: {eventObject.name}, needtofix");
        }
        */
    }

    /*
    public void CheckFixed()
    {
        if (eventObjectsList.Count == fixedCount )
        {
            Debug.Log("Win");
            Debug.Log($"ListCount:{eventObjectsList.Count}, FixedCount:{fixedCount}");
        }
        else if (eventObjectsList.Count - fixedCount <=2)
        {
            Debug.Log("Have a Chance");
            Debug.Log($"ListCount:{eventObjectsList.Count}, FixedCount:{fixedCount}");
        }
        else
        {
            Debug.Log("Lose");
            Debug.Log($"ListCount:{eventObjectsList.Count}, FixedCount:{fixedCount}");
        }
    }
    */

    public void SetFixedCount(int count)
    {
        fixedCount = count;
    }

    public List<EventObject> EventObjectsList => eventObjectsList;
    public int FixedCount => fixedCount;
    public void FixedCountPlus() { fixedCount++; }
}
