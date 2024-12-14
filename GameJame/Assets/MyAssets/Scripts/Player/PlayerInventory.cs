using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject itemHolder;

    private Loot currentItem;
    public Loot GetCurrentItem => currentItem; //Для проверки предмета в руке см. EventObject.cs
    public event Action<Loot> OnItemChanged;

    private void Start()
    {
        OnItemChanged?.Invoke(null);
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (InputManager.Instance.IsDropItem())
        {
            DropItem(true, 2f);
        }
    }

    public void PickupItem(Loot loot)
    {
        if (currentItem != null)
        {
            DropItem(true, 2f);
        }

        currentItem = loot;
        OnItemChanged?.Invoke(loot);
        
        Rigidbody rb = loot.GetComponent<Rigidbody>();
        Collider collider = loot.GetComponent<Collider>();
        
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.None;
        }
        
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        
        loot.transform.SetParent(itemHolder.transform);
        loot.transform.localPosition = loot.GetLootData.GetLootPositionInHolder;
        loot.transform.localRotation = Quaternion.Euler(loot.GetLootData.GetLootRotationInHolder);
        SoundManager.Instance.PlaySound("PickUp");
        
    }

    public void DropItem(bool useForce, float throwForce)
    {
        if (currentItem == null) return;
        
        currentItem.transform.SetParent(null);
        
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        Collider collider = currentItem.GetComponent<Collider>();
        
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            
            if (useForce)
            {
                rb.AddForce(transform.forward * throwForce, ForceMode.Impulse);
            }
        }
        
        if (collider != null)
        {
            collider.isTrigger = false;
        }
        
        SoundManager.Instance.PlaySound("DropItem");
        currentItem = null;
        OnItemChanged?.Invoke(null); //вызвать в конце
    }
    
    
    
    //TODO: Метод для удаления предмета 
    
    
    public void DestroyItem()
    {
        if (currentItem == null) return;
       
        //Типа предмет рассыпается или что-то в этом роде
        //currentItem.GetLootData.GetLootDestroyAnimator?. //анимация 
        //currentItem.GetLootData.GetLootDestroySound?. //звук
        
        
        Destroy(currentItem.gameObject);
        
        currentItem = null;
        OnItemChanged?.Invoke(null); //вызвать в конце
    }
}

