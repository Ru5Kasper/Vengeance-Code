using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject respawnMessage;        // "В этот раз повезёт!" (если используется)
    public TextMeshProUGUI respawnText;

    [Header("Death Screen")]
    public GameObject deathPanel;            // панель смерти
    public TextMeshProUGUI deathTimerText;   // цифры таймера на панели

    [SerializeField] private GameObject levelCompletePanel;

    private void Awake()
    {
        instance = this;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ShowRespawnMessage(string message)
    {
        if (respawnMessage != null && respawnText != null)
        {
            respawnText.text = message;
            respawnMessage.SetActive(true);
        }
    }

    public void HideRespawnMessage()
    {
        if (respawnMessage != null) respawnMessage.SetActive(false);
    }

    // Показывает панель смерти и запускает локальный таймер (для визуалки)
    public void ShowDeathScreen(float duration)
    {
        if (deathPanel == null || deathTimerText == null) return;
        deathPanel.SetActive(true);
        StartCoroutine(DeathCountdown(duration));
    }

    public void HideDeathScreen()
    {
        if (deathPanel != null) deathPanel.SetActive(false);
        StopAllCoroutines(); // остановим таймеры UI, если были
    }

    private IEnumerator DeathCountdown(float duration)
    {
        float t = duration;
        while (t > 0f)
        {
            int seconds = Mathf.CeilToInt(t);
            deathTimerText.text = seconds.ToString();
            yield return new WaitForSeconds(1f);
            t -= 1f;
        }
        deathTimerText.text = "0";
        // скрывать панель лучше после респавна — PlayerHealth / RespawnManager сделает HideRespawnMessage/HideDeathScreen при завершении
    }

    public void ShowLevelCompleteUI()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
    }
    public void HideLevelCompleteUI()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }
}
