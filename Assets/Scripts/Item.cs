using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int itemIndex;
    private bool pickedUp = false;

    public int ItemIndex => itemIndex;
}
