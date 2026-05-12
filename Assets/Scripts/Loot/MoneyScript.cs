using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MoneyPickup : MonoBehaviour
{
    public int moneyValue = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddMoney(moneyValue);
                Debug.Log($"Picked up {moneyValue} money!");
                Destroy(gameObject);
            }
        }
    }
}
