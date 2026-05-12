using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    private SpriteRenderer playerSprite;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Вычисляем направление к курсору
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (mousePos - firePoint.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;

        // Поворачиваем FirePoint
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        // Переворачиваем персонажа, если мышь слева или справа
        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

        // Стрельба
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            Shoot();
            FindObjectOfType<PlayerAudioController>()?.PlayShootSound();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
