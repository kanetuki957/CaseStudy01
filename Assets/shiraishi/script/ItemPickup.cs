using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private bool pickedUp = false;

    public string itemName = "key";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        var aitem = other.GetComponent<aitem>();
        if (aitem == null) return;

        pickedUp = true;
        aitem.collectedItems.Add(itemName);
        Destroy(gameObject);
    }
}
