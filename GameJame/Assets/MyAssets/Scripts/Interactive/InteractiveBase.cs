using UnityEngine;



public interface InteractiveBase
{
    Animator animator { get; }

    bool canInteract { get; set; }

    bool isActive
    {
        get
        {
            return isActive;
        }
        set
        {
            isActive = value;
        }
    }

    LootBase loot { get; }
    
    
    
    
}
