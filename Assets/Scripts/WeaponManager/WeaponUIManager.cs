using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUIManager : MonoBehaviour
{
    public static WeaponUIManager Instance {get; private set;}

    public Image weaponUIImage;  
    public TextMeshProUGUI yourAmmoText;  

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateWeaponUI(WeaponType weapon)
    {
        if (weaponUIImage != null && weapon != null)
        {
            weaponUIImage.sprite = weapon.weaponIcon;
        }

        UpdateAmmoUI(weapon);
    }

    public void UpdateAmmoUI(WeaponType weapon)
    {
        if (yourAmmoText != null && weapon != null)
        {
            yourAmmoText.text = weapon.currentAmmo.ToString() + "/" + weapon.maxAmmo.ToString();
        }
    }
}
