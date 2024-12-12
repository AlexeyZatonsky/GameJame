using System.Collections.Generic;
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
            foreach (EventObject _event in _events)
            {
                var lootData = _event.GetEventData().GetDataLootToFix;

                if (lootData != null)
                {
                    objectsToSpawn.Add(_event.GetEventData().GetDataLootToFix);
                }
            }
        }

        private void spawnLoot()
        {
            spawnPlaces.AddRange(spawnPlacesPull.GetComponentsInChildren<Transform>());

            foreach (LootData _lootData in objectsToSpawn)
            {
                int index = Random.Range(0, spawnPlaces.Count - 1);
                
                Transform spawnPlace = spawnPlaces[index];
                spawnPlaces.RemoveAt(index);
                
                Instantiate(_lootData.GetLootPrefab, spawnPlace);
            }
        }
    }
}