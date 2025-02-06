using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Image[] inventorySlots; 
    [SerializeField] private Sprite[] itemSprites; 

    private int nextAvailableSlot = 0; 

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
