using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;
    public float attackCooldown = 2f;
    public float fireRate = 0f; // если нужно автострельбу (0 = одиночные)

    private Transform player;
    private float nextFireTime;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    public void FireAtPlayer()
    {
        if (player == null || bulletPrefab == null || firePoint == null) return;

        if (Time.time < nextFireTime) return;

        // Направление на игрока
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Создаём пулю
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();

        if (rbBullet != null)
            rbBullet.velocity = direction * bulletSpeed;

        // Поворот спрайта пули по направлению
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

        nextFireTime = Time.time + (fireRate > 0 ? 1f / fireRate : attackCooldown);
    }
}
