using UnityEngine;


public class Breaking : MonoBehaviour
{
    [SerializeField] private GameObject intactObj;
    [SerializeField] private GameObject brokenObj;

    private BoxCollider bc;

    private void Awake()
    {
        intactObj.SetActive(true);
        brokenObj.SetActive(false);

        bc = GetComponent<BoxCollider>();
    }


    public void Break()
    {
        intactObj.SetActive(false);
        brokenObj.SetActive(true);
        bc.enabled = false;
    }

   

}
