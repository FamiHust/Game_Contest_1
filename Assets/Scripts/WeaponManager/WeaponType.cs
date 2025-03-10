using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "NewWeaponType", menuName = "Weapon/WeaponType")]
public class WeaponType : ScriptableObject
{
    public string weaponName;
    public int maxAmmo;
    public int currentAmmo;
    public Sprite weaponIcon;

    private void Awake() 
    {
        currentAmmo = maxAmmo;    
    }

    public void DecreaseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
        }
    }

    public void UpdateAmmoUI(TextMeshProUGUI ammoText)
    {
        if (ammoText != null)
        {
            ammoText.text =  currentAmmo.ToString() + "/" + maxAmmo.ToString();
        }
    }

    public void ReloadAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo); 
    }
}
