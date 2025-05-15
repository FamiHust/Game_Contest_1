// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.EventSystems;


// public class Weapon : MonoBehaviour
// {
//     public GameObject bullet;
//     public Transform[] firePos;
//     private Animator anim;
//     public WeaponType weaponData;

//     [SerializeField] private float TimeBtwFire = 0.2f;
//     [SerializeField] private float bulletForce;
//     [SerializeField] private float rotationSmoothness = 5f;
//     private float timeBtwFire;

//     [Header("UI Button & Auto Aim")]
//     public Button fireButton;
//     public float autoAimRadius = 5f;
//     public LayerMask enemyLayer;

//     private Transform nearestEnemy;
//     private bool isAutoAiming = false;
//     private Quaternion targetRotation;
//     private bool isFiring = false;

//     private void Awake()
//     {
//         anim = GetComponent<Animator>();

//         if (fireButton == null)
//         {
//             GameObject buttonObj = GameObject.FindGameObjectWithTag("FireButton");
//             if (buttonObj != null)
//             {
//                 fireButton = buttonObj.GetComponent<Button>();
//             }
//         }

//         if (fireButton != null)
//         {
//             fireButton.onClick.AddListener(OnFireButtonPressed);
//             fireButton.GetComponent<EventTrigger>().triggers.Clear();

//             EventTrigger trigger = fireButton.gameObject.AddComponent<EventTrigger>();
//             EventTrigger.Entry entryPress = new EventTrigger.Entry
//             {
//                 eventID = EventTriggerType.PointerDown
//             };
//             entryPress.callback.AddListener((eventData) => { isFiring = true; });

//             EventTrigger.Entry entryRelease = new EventTrigger.Entry
//             {
//                 eventID = EventTriggerType.PointerUp
//             };
//             entryRelease.callback.AddListener((eventData) => { isFiring = false; });

//             trigger.triggers.Add(entryPress);
//             trigger.triggers.Add(entryRelease);
//         }

//         PlayerController.Instance.PlayerRotated += UpdateRotation;
//     }

//     private void OnDestroy()
//     {
//         if (PlayerController.Instance != null)
//         {
//             PlayerController.Instance.PlayerRotated -= UpdateRotation;
//         }
        
//         if (fireButton != null)
//         {
//             fireButton.onClick.RemoveListener(OnFireButtonPressed);
//         }
//     }

//     void Update()
//     {
//         timeBtwFire -= Time.deltaTime;

//         nearestEnemy = GetNearestEnemyInRange();

//         if (nearestEnemy != null)
//         {
//             isAutoAiming = true;
//             RotateTowardsEnemy(nearestEnemy);
//         }
//         else
//         {
//             isAutoAiming = false;
//         }

//         transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness * 2f);

//         if (isFiring && timeBtwFire <= 0 && weaponData.currentAmmo > 0)
//         {
//             FireBullet();
//             timeBtwFire = TimeBtwFire;
//         }
//     }

//     void UpdateRotation(Quaternion newRotation)
//     {
//         if (!isAutoAiming)
//         {
//             float currentZ = newRotation.eulerAngles.z;
//             targetRotation = Quaternion.Euler(0, 0, currentZ + 90f);
//         }
//     }

//     void OnFireButtonPressed()
//     {
//         if (timeBtwFire <= 0 && weaponData.currentAmmo > 0)
//         {
//             FireBullet();
//             timeBtwFire = TimeBtwFire;
//         }
//     }

//     public void Reload()
//     {
//         SoundManager.PlaySound(SoundType.WeaponPickUp);
//         StartCoroutine(ReloadCoroutine());
//     }

//     IEnumerator ReloadCoroutine()
//     {
//         while (weaponData.reserveAmmo > 0 && weaponData.currentAmmo < weaponData.maxAmmo)
//         {
//             weaponData.ReloadAmmo();
//             FindObjectOfType<WeaponUIManager>().UpdateAmmoUI(weaponData);
//             yield return new WaitForSeconds(0.1f);
//         }
//     }

//     void FireBullet()
//     {
//         anim.SetTrigger("isShooting");
//         anim.transform.rotation = transform.rotation;
//         SoundManager.PlaySound(SoundType.PlayerBullet);

//         foreach (Transform fire in firePos)
//         {
//             GameObject bulletTmp = Instantiate(bullet, fire.position, transform.rotation);
//             Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
//             rb.AddForce(fire.right * bulletForce, ForceMode2D.Impulse);
//         }

//         weaponData.DecreaseAmmo();
//         FindObjectOfType<WeaponUIManager>().UpdateAmmoUI(weaponData);
//     }

//     void RotateTowardsEnemy(Transform target)
//     {
//         Vector2 direction = target.position - transform.position;
//         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//         targetRotation = Quaternion.Euler(0, 0, angle);
//     }

//     Transform GetNearestEnemyInRange()
//     {
//         Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, autoAimRadius, enemyLayer);
//         Transform closest = null;
//         float minDistance = Mathf.Infinity;

//         foreach (Collider2D enemy in enemies)
//         {
//             float distance = Vector2.Distance(transform.position, enemy.transform.position);
//             if (distance < minDistance)
//             {
//                 minDistance = distance;
//                 closest = enemy.transform;
//             }
//         }

//         return closest;
//     }

//     private void OnDrawGizmosSelected()
//     {
//         Gizmos.color = Color.red;
//         Gizmos.DrawWireSphere(transform.position, autoAimRadius);
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform[] firePos;
    private Animator anim;
    public WeaponType weaponData;

    [SerializeField] private float TimeBtwFire = 0.2f;
    [SerializeField] private float bulletForce;
    [SerializeField] private float rotationSmoothness = 5f;
    private float timeBtwFire;

    [Header("UI Button & Auto Aim")]
    public Button fireButton;
    public float autoAimRadius = 5f;
    public LayerMask enemyLayer;

    private Transform nearestEnemy;
    private bool isAutoAiming = false;
    private Quaternion targetRotation;
    private bool isFiring = false;
    public WeaponManager weaponManager;

    // private void Awake()
    // {
    //     anim = GetComponent<Animator>();

    //     if (fireButton == null)
    //     {
    //         GameObject buttonObj = GameObject.FindGameObjectWithTag("FireButton");
    //         if (buttonObj != null)
    //         {
    //             fireButton = buttonObj.GetComponent<Button>();
    //         }
    //     }

    //     if (fireButton != null)
    //     {
    //         fireButton.onClick.AddListener(OnFireButtonPressed);
    //         fireButton.GetComponent<EventTrigger>().triggers.Clear();

    //         EventTrigger trigger = fireButton.gameObject.AddComponent<EventTrigger>();
    //         EventTrigger.Entry entryPress = new EventTrigger.Entry
    //         {
    //             eventID = EventTriggerType.PointerDown
    //         };
    //         entryPress.callback.AddListener((eventData) => { isFiring = true; });

    //         EventTrigger.Entry entryRelease = new EventTrigger.Entry
    //         {
    //             eventID = EventTriggerType.PointerUp
    //         };
    //         entryRelease.callback.AddListener((eventData) => { isFiring = false; });

    //         trigger.triggers.Add(entryPress);
    //         trigger.triggers.Add(entryRelease);
    //     }

    //     PlayerController.Instance.PlayerRotated += UpdateRotation;
    // }

    private void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        anim = GetComponent<Animator>();

        if (fireButton == null)
        {
            GameObject buttonObj = GameObject.FindGameObjectWithTag("FireButton");
            if (buttonObj != null)
            {
                fireButton = buttonObj.GetComponent<Button>();
            }
        }

        if (fireButton != null)
        {
            fireButton.onClick.AddListener(OnFireButtonPressed);
            fireButton.GetComponent<EventTrigger>().triggers.Clear();

            EventTrigger trigger = fireButton.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry entryPress = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown
            };
            entryPress.callback.AddListener((eventData) => { isFiring = true; });

            EventTrigger.Entry entryRelease = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerUp
            };
            entryRelease.callback.AddListener((eventData) => { isFiring = false; });

            trigger.triggers.Add(entryPress);
            trigger.triggers.Add(entryRelease);
        }

        PlayerController.Instance.PlayerRotated += UpdateRotation;
    }

    private void OnDestroy()
    {
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.PlayerRotated -= UpdateRotation;
        }

        if (fireButton != null)
        {
            fireButton.onClick.RemoveListener(OnFireButtonPressed);
        }
    }

    void Update()
    {
        timeBtwFire -= Time.deltaTime;

        nearestEnemy = GetNearestEnemyInRange();

        if (nearestEnemy != null)
        {
            isAutoAiming = true;
            RotateTowardsEnemy(nearestEnemy);
        }
        else
        {
            isAutoAiming = false;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothness * 2f);

        if (isFiring && timeBtwFire <= 0 && weaponData.currentAmmo > 0)
        {
            FireBullet();
            timeBtwFire = TimeBtwFire;
        }
    }

    void UpdateRotation(Quaternion newRotation)
    {
        if (!isAutoAiming)
        {
            float currentZ = newRotation.eulerAngles.z;
            targetRotation = Quaternion.Euler(0, 0, currentZ + 90f);
        }
    }

    void OnFireButtonPressed()
    {
        if (timeBtwFire <= 0 && weaponData.currentAmmo > 0)
        {
            FireBullet();
            timeBtwFire = TimeBtwFire;
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

    void RotateTowardsEnemy(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0, 0, angle);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, autoAimRadius);
    }
}