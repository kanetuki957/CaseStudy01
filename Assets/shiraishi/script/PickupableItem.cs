using UnityEngine;

public class PickupableItem : MonoBehaviour
{
    private bool pickedUp = false;

    [Tooltip("���̃A�C�e���̖��O�i��: 'key'�j")]
    public string itemName = "key";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        var inventory = other.GetComponent<PlayerInventory>();
        if (inventory == null) return;

        pickedUp = true;
        inventory.collectedItems.Add(itemName);
        Debug.Log($"[PickupableItem] {other.name} �� {itemName} ���E���܂���");

        gameObject.SetActive(false);
    }
}
