using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPickUp : MonoBehaviour
{
    [SerializeField] private int armorAmount = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance.AddArmor(armorAmount);
            SoundManager.PlaySound(SoundType.HealthPickUp);
            Destroy(gameObject);
        }
    }
}
