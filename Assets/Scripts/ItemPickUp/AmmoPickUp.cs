using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    public int ammoAmount = 10; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();
            if (weaponManager != null)
            {
                Weapon weaponScript = weaponManager.currentGun.GetComponent<Weapon>();
                weaponScript.weaponData.reserveAmmo += ammoAmount;
                FindObjectOfType<WeaponUIManager>().UpdateAmmoUI(weaponScript.weaponData);
                SoundManager.PlaySound(SoundType.WeaponPickUp);
                Destroy(gameObject); 
            }
        }
    }
}