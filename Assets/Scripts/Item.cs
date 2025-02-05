using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int itemIndex;

    public int ItemIndex => itemIndex;
}
