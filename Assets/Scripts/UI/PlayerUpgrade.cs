using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public PlayerHealth playerHealth;
    [Tooltip("Префаб пули игрока, на котором висит BulletDamage")]
    public GameObject playerBulletPrefab;

    [Header("UI Elements")]
    public TextMeshProUGUI damageLevelText;
    public TextMeshProUGUI hpLevelText;
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI hpCostText;
    public Button damageButton;
    public Button hpButton;

    [Header("Upgrade Settings")]
    public int baseCost = 10;
    public int maxLevel = 10;
    public int damagePerLevel = 10;
    public float hpPerLevel = 30f;

    private int damageLevel = 0;
    private int hpLevel = 0;

    private BulletDamage bulletDamage;

    void Start()
    {
        if (playerStats != null)
            playerStats.OnMoneyChanged += HandleMoneyChanged;

        if (playerBulletPrefab != null)
            bulletDamage = playerBulletPrefab.GetComponent<BulletDamage>();

        if (bulletDamage == null)
            Debug.LogWarning("[UPGRADE] Bullet prefab not assigned or missing BulletDamage component!");

        UpdateUI();
    }

    void OnEnable()
    {
        UpdateUI();
    }

    void OnDestroy()
    {
        if (playerStats != null)
            playerStats.OnMoneyChanged -= HandleMoneyChanged;
    }

    private void HandleMoneyChanged(int newMoney)
    {
        UpdateUI();
    }

    public void UpgradeDamage()
    {
        int cost = GetCost(damageLevel);

        if (playerStats.money >= cost && damageLevel < maxLevel)
        {
            playerStats.AddMoney(-cost);
            damageLevel++;

            // Обновляем урон и в PlayerStats, и в BulletDamage префабе
            playerStats.damage += damagePerLevel;

            if (bulletDamage != null)
            {
                bulletDamage.damage = playerStats.damage;
                Debug.Log($"[UPGRADE] Bullet prefab damage updated: {bulletDamage.damage}");
            }

            Debug.Log($"[UPGRADE] Damage upgraded to level {damageLevel} | " +
                      $"Cost: {cost} | Player Damage: {playerStats.damage} | Money left: {playerStats.money}");

            UpdateUI();
        }
        else
        {
            Debug.LogWarning("[UPGRADE] Not enough money or max damage level reached!");
        }
    }

    public void UpgradeHP()
    {
        int cost = GetCost(hpLevel);

        if (playerStats.money >= cost && hpLevel < maxLevel)
        {
            playerStats.AddMoney(-cost);
            hpLevel++;

            playerHealth.IncreaseMaxHealth(hpPerLevel);

            Debug.Log($"[UPGRADE] HP upgraded to level {hpLevel} | " +
                      $"Cost: {cost} | New Max HP: {playerHealth.maxHealth} | Money left: {playerStats.money}");

            UpdateUI();
        }
        else
        {
            Debug.LogWarning("[UPGRADE] Not enough money or max HP level reached!");
        }
    }

    private int GetCost(int currentLevel)
    {
        return baseCost * Mathf.RoundToInt(Mathf.Pow(2, currentLevel));
    }

    private void UpdateUI()
    {
        damageLevelText.text = $"Damage Lvl: {damageLevel}";
        hpLevelText.text = $"HP Lvl: {hpLevel}";

        damageCostText.text = (damageLevel < maxLevel)
            ? $"Cost: {GetCost(damageLevel)}"
            : "MAX";

        hpCostText.text = (hpLevel < maxLevel)
            ? $"Cost: {GetCost(hpLevel)}"
            : "MAX";

        damageButton.interactable = (playerStats.money >= GetCost(damageLevel) && damageLevel < maxLevel);
        hpButton.interactable = (playerStats.money >= GetCost(hpLevel) && hpLevel < maxLevel);
    }
}
