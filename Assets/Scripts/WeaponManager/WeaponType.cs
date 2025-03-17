using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewWeaponType", menuName = "Weapon/WeaponType")]
public class WeaponType : ScriptableObject
{
    public string weaponName;
    public int maxAmmo;
    public int currentAmmo;
    public int reserveAmmo;
    public Sprite weaponIcon;

    private void OnEnable() 
    {  
        currentAmmo = maxAmmo;
        reserveAmmo = 0;
    }

    public void DecreaseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
    }

    public void ReloadAmmo()
    {
        if (reserveAmmo > 0 && currentAmmo < maxAmmo)
        {
            int ammoNeeded = maxAmmo - currentAmmo;
            int ammoToReload = Mathf.Min(ammoNeeded, reserveAmmo);
            currentAmmo += ammoToReload;
            reserveAmmo -= ammoToReload;
        }
    }
}
