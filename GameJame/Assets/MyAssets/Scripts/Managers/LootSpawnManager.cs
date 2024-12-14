using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;

namespace MyAssets.Managers
{
    public class LootSpawnManager: SingletonManager<LootSpawnManager>
    {
        [SerializeField] private GameObject spawnPlacesPull;
        [SerializeField] private List<Transform> spawnPlaces = new List<Transform>();
        
        
        [SerializeField] private List<LootData> objectsToSpawn = new List<LootData>();


        private void Start()
        {
            initializeLootDataToSpawn();
            spawnLoot();
        }

        private void initializeLootDataToSpawn()
        {
            List<EventObject> _events =  EnableEventManager.Instance.GetEvents;
            
            
            for(int i = 0; i<= _events.Count - 1; i++)
            {
                LootData lootData = _events[i].GetEventData().GetDataLootToFix;

                if (lootData != null)
                {
                    objectsToSpawn.Add(_events[i].GetEventData().GetDataLootToFix);
                    //Debug.LogError("LootSpawnManager initializeLootDataToSpawn LootData Is nottt NuLL");
                }
                //else { Debug.LogError("LootSpawnManager initializeLootDataToSpawn LootData Is NuLL"); }
            }
        }
        

        private void spawnLoot()
        {
            spawnPlaces.AddRange(spawnPlacesPull.GetComponentsInChildren<Transform>());
            
            for (int i = 0; i <= objectsToSpawn.Count - 1; ++i)
            {
                int index = Random.Range(0, spawnPlaces.Count - 1);

                Transform spawnPlace = spawnPlaces[index];
                spawnPlaces.RemoveAt(index);
                //if()
                Instantiate(objectsToSpawn[i].GetLootPrefab, spawnPlace);
            }
            
            
        }
    }
}