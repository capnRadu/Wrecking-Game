using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots; // UI slots for inventory images
    [SerializeField] private Sprite[] itemSprites; // Sprites for each item

    private int nextAvailableSlot = 0; // Tracks the next empty slot

    public void AddToInventory(int itemIndex)
    {
        if (nextAvailableSlot < inventorySlots.Length)
        {
            inventorySlots[nextAvailableSlot].sprite = itemSprites[itemIndex];
            inventorySlots[nextAvailableSlot].gameObject.SetActive(true);
            nextAvailableSlot++;
        }
    }
}
