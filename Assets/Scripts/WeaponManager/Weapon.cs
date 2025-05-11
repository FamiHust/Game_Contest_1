using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform[] firePos;
    private Animator anim;
    public WeaponType weaponData;

    [SerializeField] private float TimeBtwFire = 0.2f;
    [SerializeField] private float bulletForce;
    private float timeBtwFire;

    [Header("Joystick & Auto Aim")]
    public Joystick aimJoystick;
    public float autoAimRadius = 5f;
    public LayerMask enemyLayer;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        if (aimJoystick == null)
        {
            GameObject joystickObj = GameObject.FindGameObjectWithTag("Joystick");
            if (joystickObj != null)
            {
                aimJoystick = joystickObj.GetComponent<Joystick>();
            }
        }
    }

    void Start()
    {
        weaponData.currentAmmo = weaponData.maxAmmo;
    }

    void Update()
    {
        timeBtwFire -= Time.deltaTime;

        Transform nearestEnemy = GetNearestEnemyInRange();
        if (nearestEnemy != null)
        {
            RotateTowardsEnemy(nearestEnemy);
        }
        else
        {
            RotateByJoystick();
        }

        Vector2 shootInput = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);

        if (shootInput.magnitude > 0.5f) // Joystick đang được đẩy mạnh
        {
            if (timeBtwFire <= 0 && weaponData.currentAmmo > 0)
            {
                FireBullet();
                timeBtwFire = TimeBtwFire;
            }
        }

        // if (Input.GetKeyDown(KeyCode.Q))
        // {
        //     SoundManager.PlaySound(SoundType.WeaponPickUp);
        //     StartCoroutine(ReloadCoroutine());
        // }
    }

    public void Reload()
    {
        SoundManager.PlaySound(SoundType.WeaponPickUp);
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        while (weaponData.reserveAmmo > 0 && weaponData.currentAmmo < weaponData.maxAmmo)
        {
            weaponData.ReloadAmmo();
            FindObjectOfType<WeaponUIManager>().UpdateAmmoUI(weaponData);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FireBullet()
    {
        anim.SetTrigger("isShooting");
        anim.transform.rotation = transform.rotation;
        SoundManager.PlaySound(SoundType.PlayerBullet);

        foreach (Transform fire in firePos)
        {
            GameObject bulletTmp = Instantiate(bullet, fire.position, transform.rotation);
            Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
            rb.AddForce(fire.right * bulletForce, ForceMode2D.Impulse);
        }

        weaponData.DecreaseAmmo();
        FindObjectOfType<WeaponUIManager>().UpdateAmmoUI(weaponData);
    }

    void RotateByJoystick()
    {
        Vector2 input = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical);
        if (input.magnitude > 0.1f)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void RotateTowardsEnemy(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    Transform GetNearestEnemyInRange()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, autoAimRadius, enemyLayer);
        Transform closest = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }

    // Vẽ bán kính auto aim trong Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoAimRadius);
    }
}
