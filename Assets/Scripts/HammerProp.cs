using UnityEngine;

public class HammerProp : MonoBehaviour
{
    public PlayerController player;
    public GameObject playerHamer;

    public void PickUp()
    {
        playerHamer.SetActive(true);
        player.IsActive = true;
        Destroy(gameObject);
    }
}
