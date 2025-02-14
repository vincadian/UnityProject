using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Existing bullet settings
    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    [Range(0, 1)] public float spawnProbability = 0.3f;
    public float bulletSpeed = 5f;
    [Header("Optional Settings")]
    public float bulletLifetime = 2f;

    // Reference to PointSystem
    private PointSystem _pointSystem;

    private void Start()
    {
        _pointSystem = PointSystem.Instance;
        if (_pointSystem == null)
        {
            Debug.LogError("PointSystem instance not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            HandleBulletHit(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            HandleBulletHit(collision.gameObject);
        }
    }

    void HandleBulletHit(GameObject bullet)
    {
        Destroy(bullet);

        // Only spawn bullets if Bonus 1 is active
        if (_pointSystem != null && _pointSystem.Bonus1.isActive)
        {
            if (Random.value <= spawnProbability)
            {
                SpawnRandomBullet();
            }
        }
    }

    // Existing SpawnRandomBullet and BulletLife classes remain the same
    void SpawnRandomBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector2 direction = Random.insideUnitCircle.normalized;
        
        Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        if (bulletLifetime > 0)
        {
            BulletLife bl = newBullet.GetComponent<BulletLife>() ?? newBullet.AddComponent<BulletLife>();
            bl.lifetime = bulletLifetime;
        }
    }
}

// BulletLife class remains unchanged
public class BulletLife : MonoBehaviour
{
    public float lifetime = 2f;

    private void Start()
    {
        if (lifetime > 0)
        {
            Destroy(gameObject, lifetime);
        }
    }
}