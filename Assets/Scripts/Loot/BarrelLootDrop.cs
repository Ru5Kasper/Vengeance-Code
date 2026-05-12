using UnityEngine;

public class BarrelLootDrop : MonoBehaviour
{
    [Header("Loot Prefabs")]
    public GameObject moneyPrefab;
    public GameObject medkitPrefab;

    [Header("Health Settings")]
    public float maxHealth = 20f;
    private float currentHealth;

    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (isDestroyed) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DestroyBarrel();
        }
    }

    void DestroyBarrel()
    {
        if (isDestroyed) return;
        isDestroyed = true;

        // Дроп гарантированный
        if (moneyPrefab != null)
            Instantiate(moneyPrefab, transform.position + Vector3.left * 0.3f, Quaternion.identity);

        if (medkitPrefab != null)
            Instantiate(medkitPrefab, transform.position + Vector3.right * 0.3f, Quaternion.identity);

        // Можно добавить анимацию взрыва/звука
        Destroy(gameObject);
    }
}
