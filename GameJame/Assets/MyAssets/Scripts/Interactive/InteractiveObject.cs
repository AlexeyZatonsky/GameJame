using System.Collections.Generic;
using UnityEngine;


//Это объекты объекты эвентов которфые нужно выполнить
public class InteractiveObject : MonoBehaviour, IInteractive
{
    [SerializeField] protected IntractiveObjectData intractiveObjectData;
    //[SerializeField] private List<LootData> lootDatasList = new List<LootData>();

    [SerializeField] protected bool canInteract = false;

    public virtual void Interact()
    {
        if (!canInteract) { return; }


        Debug.Log("Interact");

    }
}
