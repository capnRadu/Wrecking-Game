using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Interact : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float lookDistance = 5f;
    [SerializeField] private LayerMask interactableLayer;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image interactImage;

    [Header("Inventory")]
    [SerializeField] private InventoryManager inventoryManager; // Reference to InventoryManager

    private Camera playerCamera;

    private void Start()
    {
        playerCamera = Camera.main;

        if (interactText != null)
            interactText.enabled = false;
        if (interactImage != null)
            interactImage.enabled = false;
    }

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, lookDistance, interactableLayer))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                DisplayInteraction("Press 'E' to pick up");

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem(hit.collider.gameObject);
                }
            }
            else if (hit.collider.CompareTag("Breakable"))
            {
                DisplayInteraction("");
                if (Input.GetMouseButtonDown(0))
                {
                    //add hammer animation
                    BreakObject(hit.collider.gameObject);
                }
            }
        }
        else
        {
            HideInteractionUI();
        }
    }

    private void DisplayInteraction(string message)
    {
        if (interactText != null)
        {
            interactText.text = message;
            interactText.enabled = true;
        }
        if (interactImage != null)
            interactImage.enabled = true;
    }

    private void HideInteractionUI()
    {
        if (interactText != null)
            interactText.enabled = false;
        if (interactImage != null)
            interactImage.enabled = false;
    }

    private void PickUpItem(GameObject item)
    {
        Item itemScript = item.GetComponent<Item>();
        if (itemScript != null && inventoryManager != null)
        {
            inventoryManager.AddToInventory(itemScript.ItemIndex);
            item.SetActive(false);
        }
    }

    
    private void BreakObject(GameObject breakableObject)
    {
        Breaking breakingScript = breakableObject.GetComponent<Breaking>();
        if (breakingScript != null)
        {
            breakingScript.SendMessage("Break");
        }
    }

    
}

