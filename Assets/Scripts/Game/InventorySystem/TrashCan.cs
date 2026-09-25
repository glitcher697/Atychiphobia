using UnityEngine;
using UnityEngine.Events;

public class TrashCan : MonoBehaviour, IAction {
    [SerializeField] private InventorySystem inventory;
    public void StartAction(UnityAction<Results> onComplete) {
        TypeItems currentItem = inventory.GetCurrentItem();

        if (currentItem == TypeItems.Null) 
            onComplete(Results.HasNoItem); 
        
        inventory.RemoveItem(currentItem);

        onComplete(Results.Success);
    }
}
