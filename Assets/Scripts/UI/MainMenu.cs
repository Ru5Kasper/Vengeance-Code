using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Названия сцен")]
    public string gameSceneName = "Level_Prototype"; // Название сцены с уровнем

    // Кнопка "Начать игру"
    public void StartGame()
    {
        Debug.Log("Запуск уровня: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    // Кнопка "Выход"
    public void QuitGame()
    {
        Debug.Log("Игра закрыта");
        Application.Quit();

#if UNITY_EDITOR
        // Чтобы работало в редакторе
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
