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
                Weapon weaponScript = weaponPrefab.GetComponent<Weapon>();
                if (weaponScript != null)
                {
                    string newWeaponName = weaponScript.weaponData.weaponName;
                    bool alreadyHasWeapon = false;
                    
                    foreach (GameObject gun in weaponManager.guns)
                    {
                        Weapon existingWeapon = gun.GetComponent<Weapon>();
                        if (existingWeapon != null && existingWeapon.weaponData.weaponName == newWeaponName)
                        {
                            alreadyHasWeapon = true;
                            break;
                        }
                    }
                    
                    if (!alreadyHasWeapon)
                    {
                        SoundManager.PlaySound(SoundType.WeaponPickUp);
                        weaponManager.AddWeapon(weaponPrefab);
                        Destroy(gameObject); 
                    }
                }
            }
        }
    }
}
