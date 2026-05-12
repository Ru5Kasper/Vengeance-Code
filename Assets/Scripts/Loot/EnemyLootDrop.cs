using UnityEngine;

public class EnemyLootDrop : MonoBehaviour
{
    [Header("Loot Prefabs")]
    public GameObject moneyPrefab;
    public GameObject medkitPrefab;

    [Header("Drop Settings")]
    [Range(0f, 1f)] public float dropChance = 0.5f; // 50% шанс выпадения

    private bool hasDropped = false;

    public void DropLoot()
    {
        if (hasDropped) return; // защита от двойного вызова
        hasDropped = true;

        float rand = Random.value;

        // 50 на 50
        if (rand <= dropChance && moneyPrefab != null)
        {
            Instantiate(moneyPrefab, transform.position, Quaternion.identity);
        }
        else if (medkitPrefab != null)
        {
            Instantiate(medkitPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDestroy()
    {
        // не спавнить дубликаты, если сцена выгружается
        if (!Application.isPlaying) return;
        if (!hasDropped && !isQuitting)
            DropLoot();
    }

    private static bool isQuitting = false;
    private void OnApplicationQuit() => isQuitting = true;

}
