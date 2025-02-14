using UnityEngine;

public class PlayerBonus : MonoBehaviour
{
    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform firePoint;

    [Header("Bonus Settings")]
    [Range(0, 1)] public float diagonalProbability = 0.3f;
    [SerializeField] private float diagonalAngle = 45f;
    
    private float _nextFireTime;
    private PointSystem _pointSystem;

    private void Start()
    {
        _pointSystem = PointSystem.Instance;
        if (_pointSystem == null)
        {
            Debug.LogError("PointSystem instance not found!");
        }
        _nextFireTime = Time.time + fireRate;
    }

    private void Update()
    {
        // Only shoot if Bonus2 is active
        if (_pointSystem.Bonus2.isActive && Time.time >= _nextFireTime)
        {
            Shoot();
            _nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        // Always shoot straight when bonus is active
        CreateBullet(Vector2.up);
        
        // Chance for diagonal shots
        if (Random.value <= diagonalProbability)
        {
            CreateBullet(Quaternion.Euler(0, 0, diagonalAngle) * Vector2.up);
            CreateBullet(Quaternion.Euler(0, 0, -diagonalAngle) * Vector2.up);
        }
    }

    private GameObject CreateBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction.normalized * bulletSpeed;
        return bullet;
    }
}