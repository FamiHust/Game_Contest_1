using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public GameObject bullet;
    public Transform[] firePos;
    private Animator anim;

    public WeaponType weaponData;
    public float TimeBtwFire = 0.2f;
    public float bulletForce;
    private float timeBtwFire;
    private float originalSpeed;
    
    private bool isShooting = false;

    private void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        originalSpeed = PlayerController.Instance.moveSpeed;
        weaponData.currentAmmo = weaponData.maxAmmo;
    }

       void Update()
    {
        RotateGun();
        timeBtwFire -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            if (timeBtwFire <= 0 && weaponData.currentAmmo > 0)
            {
                FireBullet();
                timeBtwFire = TimeBtwFire; 
            }

            if (!isShooting) 
            {
                PlayerController.Instance.moveSpeed *= 0.5f;
                isShooting = true;
            }
        }
        else
        {
            PlayerController.Instance.moveSpeed = originalSpeed; 
            isShooting = false; 
        }
    }

    void RotateGun()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = mousePos - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = rotation;
    }

    void FireBullet()
    {
        anim.SetTrigger("isShooting");
        anim.transform.rotation = transform.rotation;
        SoundManager.PlaySound(SoundType.FireBullet);

        foreach (Transform fire in firePos)
        {
            GameObject bulletTmp = Instantiate(bullet, fire.position, transform.rotation);
            Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
            rb.AddForce(fire.right * bulletForce, ForceMode2D.Impulse);
        }

        weaponData.DecreaseAmmo();
        FindObjectOfType<WeaponUIManager>().UpdateAmmoUI(weaponData);
    }
}
