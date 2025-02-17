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

    // Start is called before the first frame update
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
    }

    void Update()
    {
        SwitchGuns();
    }

    private void SwitchGuns()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            guns[currentWeaponIndex].SetActive(false);
            currentWeaponIndex = (currentWeaponIndex + 1) % totalWeapons; // Chuyển về đầu khi vượt quá
            guns[currentWeaponIndex].SetActive(true);
            currentGun = guns[currentWeaponIndex];
        }

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     guns[currentWeaponIndex].SetActive(false);
        //     currentWeaponIndex = (currentWeaponIndex - 1 + totalWeapons) % totalWeapons; // Chuyển về cuối khi lùi quá
        //     guns[currentWeaponIndex].SetActive(true);
        //     currentGun = guns[currentWeaponIndex];
        // }
    }


    public void AddWeapon(GameObject weaponPrefab)
    {
        // Tạo bản sao của prefab trong game
        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder.transform);

        // Thêm vào danh sách vũ khí
        List<GameObject> weaponList = new List<GameObject>(guns);
        weaponList.Add(newWeapon);
        guns = weaponList.ToArray();

        newWeapon.SetActive(false); // Ẩn súng mới nhặt
        totalWeapons++;
    }

}
