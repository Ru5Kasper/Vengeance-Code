using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEndTrigger : MonoBehaviour
{
    [Header("Настройки конца уровня")]
    public string nextSceneName; // Имя сцены следующего уровня
    public float delayBeforeUI = 1.5f; // Задержка перед экраном победы

    private bool levelEnded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelEnded) return;

        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            levelEnded = true;
            StartCoroutine(EndLevelSequence(player));
        }
    }

    private IEnumerator EndLevelSequence(PlayerStats player)
    {
        // Отключаем управление игроком
        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        if (movement != null)
            movement.enabled = false;

        // Останавливаем физику, чтобы не упал
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        // Немного подождём (для эффекта окончания уровня)
        yield return new WaitForSeconds(delayBeforeUI);

        // Показываем экран победы
        if (UIManager.instance != null)
            UIManager.instance.ShowLevelCompleteUI();

        Debug.Log("🎉 Уровень пройден!");
    }

    // --- Кнопки UI ---

    public void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("Не указано имя следующей сцены!");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Название главного меню
    }
}
