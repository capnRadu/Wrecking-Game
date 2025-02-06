using UnityEngine;
using System.Collections.Generic;

public class ManagerTimeReset : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToEnable;  // List of objects to activate
    [SerializeField] private List<GameObject> objectsToDisable; // List of objects to deactivate
   
    void Update()
    {
        if (PieceControl.timereset) // Check if timereset is true
        {
            ResetObjects();
            PieceControl.timereset = false; // Reset timereset after applying changes
        }
    }

    private void ResetObjects()
    {
        // Enable all objects in objectsToEnable list
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
            {
                obj.SetActive(true);
                BoxCollider collider = obj.GetComponent<BoxCollider>();
                if (collider != null)
                {
                    collider.enabled = true;
                }
            }
        }

        // Disable all objects in objectsToDisable list
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }
    }
}
