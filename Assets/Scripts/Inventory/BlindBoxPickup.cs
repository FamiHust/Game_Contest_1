using UnityEngine;

public class BlindBoxPickup : MonoBehaviour
{
    public BlindBox blindBoxData;
    private bool isCollected = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isCollected) return;

        if (collision.CompareTag("Player"))
        {
            isCollected = true;
            InventoryManager.Instance.AddBlindBox(blindBoxData);
            Destroy(gameObject);
        }
    }
}
