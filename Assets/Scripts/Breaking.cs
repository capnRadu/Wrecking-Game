using UnityEngine;

public class Breaking : MonoBehaviour
{
    [SerializeField] GameObject intactObj;
    [SerializeField] GameObject brokenObj;

    BoxCollider bc;

    private void Awake()
    {
        intactObj.SetActive(true);
        brokenObj.SetActive(false);

        bc = GetComponent<BoxCollider>();
    }
    
    private void OnMouseDown()
    {
        Break();
    }
    
    private void Break()
    {

        intactObj.SetActive(false);
        brokenObj.SetActive(true);

        bc.enabled = false;
    }

}
