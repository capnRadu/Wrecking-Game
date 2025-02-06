using UnityEngine;
using System.Collections.Generic;

public class ManagerTimeReset : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToEnable;  
    [SerializeField] private List<GameObject> objectsToDisable; 
   
    void Update()
    {
        if (PieceControl.timereset) 
        {
            ResetObjects();
            PieceControl.timereset = false; 
        }
    }

    private void ResetObjects()
    {
        
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

        
        foreach (GameObject obj in objectsToDisable)
        {
            if (obj != null) obj.SetActive(false);
        }
    }
}
