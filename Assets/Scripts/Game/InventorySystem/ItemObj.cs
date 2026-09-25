using UnityEngine;
using UnityEngine.Events;

public class ItemObj : MonoBehaviour, IAction {
    public TypeItems item;
    public InventorySystem Inventory;

    public void StartAction(UnityAction<Results> onComplete) {
        Inventory.AddItem(item);
        Destroy(this.gameObject);

        onComplete(Results.Success);
    }
}
