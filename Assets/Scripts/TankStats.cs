using UnityEngine;

[CreateAssetMenu(fileName = "NewTankStats", menuName = "Tank/TankStats")]
public class TankStats : ScriptableObject
{
    public float moveSpeed;
    public float rotationSpeed;
    public float fireRate;
    public float detectionRange;
    public float attackRange;
    public int health;
    public GameObject bulletPrefab;
}