using UnityEngine;

public class RedBarrelLootDrop : MonoBehaviour
{
    [Header("Loot Prefabs")]
    public GameObject moneyPrefab;
    public GameObject medkitPrefab;
    public GameObject lifeTokenPrefab;

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

        DropLoot();

        // Можно добавить анимацию взрыва/звука
        Destroy(gameObject);
    }

    void DropLoot()
    {
        if (moneyPrefab != null)
            Instantiate(moneyPrefab, transform.position + Vector3.right * 0.3f, Quaternion.identity);
        if (medkitPrefab != null)
            Instantiate(medkitPrefab, transform.position + Vector3.left * 0.3f, Quaternion.identity);
        if (lifeTokenPrefab != null)
            Instantiate(lifeTokenPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
    }
}
