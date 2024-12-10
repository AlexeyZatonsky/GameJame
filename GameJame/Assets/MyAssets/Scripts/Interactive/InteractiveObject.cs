using System.Collections.Generic;
using UnityEngine;


public class InteractiveObject : MonoBehaviour, IInteractive
{
    [SerializeField] private IntractiveObjectData intractiveObjectData;
    [SerializeField] private List<LootData> lootDatasList = new List<LootData>();

    private bool canInteract = false;

    public void Interact()
    {

        foreach (LootData toInteract in lootDatasList)
        {
            //TODO: проверяем совпадает ли лут в руках с одним из лутов для взаимодействия с объектом
        }
        
        //TODO: запускаем анимацию
        
        
        

        Debug.Log("Interact");

    }
}
