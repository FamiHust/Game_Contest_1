using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public GameObject weaponPrefab; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                SoundManager.PlaySound(SoundType.PickUp);
                weaponManager.AddWeapon(weaponPrefab);
                Destroy(gameObject); 
            }
            
        }
    }
}