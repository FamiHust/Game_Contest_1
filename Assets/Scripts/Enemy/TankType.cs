using UnityEngine;

[CreateAssetMenu(fileName = "NewTankType", menuName = "Tank/TankType")]
public class TankType : ScriptableObject
{
    public float moveSpeed;
    public float rotationSpeed;
    public float fireRate;
    public float detectionRange;
    public float attackRange;
    public int health;
    public GameObject bulletPrefab;
}