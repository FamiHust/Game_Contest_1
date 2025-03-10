using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private int totalWeapons = 1;
    [SerializeField] private int currentWeaponIndex;

    public GameObject[] guns;
    public GameObject weaponHolder;
    public GameObject currentGun;
    private WeaponUIManager weaponUIManager;

    void Start()
    {
        totalWeapons = weaponHolder.transform.childCount;
        guns = new GameObject[totalWeapons];

        for (int i = 0; i < totalWeapons; i++)
        {
            guns[i] = weaponHolder.transform.GetChild(i).gameObject;
            guns[i].SetActive(false);
        }

        guns[0].SetActive(true);
        currentGun = guns[0];
        currentWeaponIndex = 0;

        weaponUIManager = FindObjectOfType<WeaponUIManager>();
        UpdateWeaponUI(); // Hiển thị UI ban đầu
    }

    void Update()
    {
        SwitchGuns();
    }

    private void SwitchGuns()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SoundManager.PlaySound(SoundType.WeaponPickUp);
            guns[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = (currentWeaponIndex + 1) % totalWeapons;
            guns[currentWeaponIndex].SetActive(true);
            currentGun = guns[currentWeaponIndex];

            UpdateWeaponUI();
        }
    }

    private void UpdateWeaponUI()
    {
        Weapon weaponScript = currentGun.GetComponent<Weapon>();
        if (weaponScript != null && weaponUIManager != null)
        {
            weaponUIManager.UpdateWeaponUI(weaponScript.weaponData);
            weaponUIManager.UpdateAmmoUI(weaponScript.weaponData);
        }
    }

    public void AddWeapon(GameObject weaponPrefab)
    {
        Weapon weaponScript = weaponPrefab.GetComponent<Weapon>();
        if (weaponScript == null) return;

        string newWeaponName = weaponScript.weaponData.weaponName;
        foreach (GameObject gun in guns)
        {
            Weapon existingWeapon = gun.GetComponent<Weapon>();
            if (existingWeapon != null && existingWeapon.weaponData.weaponName == newWeaponName)
            {
                return; 
            }
        }

        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder.transform);

        List<GameObject> weaponList = new List<GameObject>(guns);
        weaponList.Add(newWeapon);
        guns = weaponList.ToArray();

        newWeapon.SetActive(false); 
        totalWeapons++;
    }
}
