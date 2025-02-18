using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Obstacle") || other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
