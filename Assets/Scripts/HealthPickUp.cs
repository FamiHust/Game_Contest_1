using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    [SerializeField] private int healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!PlayerHealth.Instance.IsFullHealth()) 
            {
                PlayerHealth.Instance.Heal(healthAmount);
                SoundManager.PlaySound(SoundType.HealthPickUp);
                Destroy(gameObject);
            }
        }
    }
}