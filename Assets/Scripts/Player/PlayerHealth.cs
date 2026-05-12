using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Player Settings")]
    public float maxHealth = 100f;
    public Slider healthBar;

    [Header("Death Settings")]
    public float deathScreenDuration = 3f; // сколько секунд висит экран смерти

    private float currentHealth;
    private bool isDead = false;
    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    // Список компонентов, которые нужно отключать при смерти (настраивается в инспекторе)
    // Например: PlayerMovement, PlayerShooting, PlayerInput и т.д.
    public Behaviour[] componentsToDisableOnDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        if (healthBar)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            // инициируем последовательность смерти через ForceDeath
            ForceDeath();
        }
    }

    public bool Heal(float amount)
    {
        float old = currentHealth;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthBar();
        return currentHealth > old;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
    }

    // Публичный метод: форсированная смерть (например, зона смерти)
    public void ForceDeath()
    {
        if (isDead) return;
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        isDead = true;

        // 1) Отключаем управление и другие компоненты
        foreach (var comp in componentsToDisableOnDeath)
        {
            if (comp != null) comp.enabled = false;
        }

        // 2) Останавливаем физику — чтобы игрок не падал и оставался на месте
        // Сбрасываем скорость и замораживаем положение
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Оставляем коллайдер включённым, чтобы персонаж не проваливался и взаимодействовал с миром
        // col.enabled = false;  // НЕ выключаем коллайдер — иначе провалится
        if (anim != null) anim.SetTrigger("Die");

        // 3) Показываем экран смерти с таймером
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowDeathScreen(deathScreenDuration);
        }

        // ждём длительность экрана (UIManager показывает таймер)
        yield return new WaitForSeconds(deathScreenDuration);

        // 4) После таймера — передаём управление системе жизней
        PlayerStats stats = GetComponent<PlayerStats>();
        if (stats != null)
        {
            // PlayerStats.TakeLife() уже запустит респавн или игру окончит
            stats.TakeLife();
        }

        // 5) Сброс — внутри TakeLife() обычно происходит респавн / активация, 
        // но если нужно, мы могли бы сбрасывать локально здесь.
    }

    // Метод для восстановления (вызов при респавне)
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
        isDead = false;

        // Размораживаем физику
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // заморозим только поворот, чтобы игрок не крутился
        // Включаем компоненты управления обратно (если их включают при респавне — делать безопасно)
        foreach (var comp in componentsToDisableOnDeath)
        {
            if (comp != null) comp.enabled = true;
        }
    }
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // можно сразу полностью восстановить HP
        UpdateHealthBar();
    }


    public bool IsDead() => isDead;
}
