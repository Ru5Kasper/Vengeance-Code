using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float maxHealth = 30f;
    private float currentHealth;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isHit = false;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} получил урон: {amount}. Осталось HP: {currentHealth}");

        // Если есть анимация получения урона
        if (isHit)
            StartCoroutine(HitRoutine());

        if (currentHealth <= 0)
        {
            Die();
            GetComponent<EnemyLootDrop>()?.DropLoot();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log($"{gameObject.name} погиб!");

        animator.SetTrigger("Die");

        // Отключаем физику и уничтожаем после задержки
        if (rb != null)
            rb.simulated = false;

        Destroy(gameObject, 0.5f); // время на проигрывание анимации
    }
        private IEnumerator HitRoutine()
    {
        isHit = true;
        animator.SetBool("Hit",isHit);
        yield return new WaitForSeconds(1f); // Кулдаун, чтобы Hit сбросился
        isHit = false;
    }
}
