using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BulletDamage : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float damage = 20f;
    public float lifeTime = 3f;
    public float speed = 15f;
    public string targetTag = "Enemy";

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // --- Повреждение стандартных целей ---
        if (collision.CompareTag(targetTag))
        {
            if (targetTag == "Enemy")
            {
                EnemyHealth enemy = collision.GetComponent<EnemyHealth>();
                if (enemy != null)
                    enemy.TakeDamage(damage);
            }
            else if (targetTag == "Player")
            {
                PlayerHealth player = collision.GetComponent<PlayerHealth>();
                if (player != null)
                    player.TakeDamage(damage);
            }

            Destroy(gameObject);
            return;
        }

        // --- Проверяем бочки ---
        if (collision.CompareTag("Barrel"))
        {
            BarrelLootDrop barrel = collision.GetComponent<BarrelLootDrop>();
            if (barrel != null)
                barrel.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }
        if (collision.CompareTag("RedBarrel"))
        {
            RedBarrelLootDrop red = collision.GetComponent<RedBarrelLootDrop>();
            if (red != null)
                red.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // --- Проверяем стены и землю ---
        string layerName = LayerMask.LayerToName(collision.gameObject.layer);
        if (layerName == "Ground" || layerName == "Wall")
            Destroy(gameObject);
    }
}
