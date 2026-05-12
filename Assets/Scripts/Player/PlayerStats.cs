using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Stats")]
    public int money = 0;
    public int maxLives = 5;
    public int lives = 3;

    [Header("Combat Stats")]
    public int damage = 10; // базовый урон игрока

    [Header("Respawn Settings")]
    public Transform respawnPoint;
    public float respawnDelay = 2f;

    [Header("UI Elements")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI livesText;

    [HideInInspector] public PlayerHealth playerHealth; // ✅ теперь видно снаружи

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>(); // ✅ инициализация

        UpdateUI();

        if (respawnPoint == null)
        {
            GameObject startPoint = GameObject.FindWithTag("StartPoint");
            if (startPoint != null)
                respawnPoint = startPoint.transform;
        }
    }
    public delegate void MoneyChangedHandler(int newMoney);
    public event MoneyChangedHandler OnMoneyChanged;
    
    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
        OnMoneyChanged?.Invoke(money); // уведомляем слушателей
    }


    public bool AddLives(int amount)
    {
        int oldLives = lives;
        lives = Mathf.Min(lives + amount, maxLives);
        UpdateUI();
        return lives > oldLives;
    }

    public void TakeLife()
    {
        lives--;
        UpdateUI();

        if (lives > 0)
        {
            RespawnManager.instance.RespawnPlayer(this); // ✅ вызываем через менеджер
        }
        else
        {
            UIManager.instance.ShowGameOver();
        }
    }

    public void SetCheckpoint(Transform newPoint)
    {
        respawnPoint = newPoint;
        Debug.Log("Контрольная точка обновлена: " + newPoint.name);
    }

    private void UpdateUI()
    {
        if (moneyText != null)
            moneyText.text = $"Money: {money}";

        if (livesText != null)
            livesText.text = $"Lives: {lives}";
    }
}
