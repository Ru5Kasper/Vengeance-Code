using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Панель паузы")]
    public GameObject pauseMenuUI;

    [Header("Окно улучшений")]
    public GameObject upgradesMenuUI;

    private bool isPaused = false;

    void Update()
    {
        // Открытие/закрытие меню по Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Если открыто окно улучшений — закрываем его и возвращаемся к паузе
            if (upgradesMenuUI != null && upgradesMenuUI.activeSelf)
            {
                CloseUpgrades();
                return;
            }

            // Включаем/выключаем паузу
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // Останавливает время
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenUpgrades()
    {
        if (upgradesMenuUI != null)
        {
            upgradesMenuUI.SetActive(true);
            pauseMenuUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Окно улучшений не назначено в инспекторе!");
        }
    }

    public void CloseUpgrades()
    {
        if (upgradesMenuUI != null)
        {
            upgradesMenuUI.SetActive(false);
            pauseMenuUI.SetActive(true);
        }
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f; // Возвращаем время перед переходом
        SceneManager.LoadScene("MainMenu");
    }
}
