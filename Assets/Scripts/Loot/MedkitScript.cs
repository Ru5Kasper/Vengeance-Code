using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPickup : MonoBehaviour
{
    public float healAmount = 40f;
    public float destroyDelay = 0.1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                bool healed = player.Heal(healAmount);
                if (healed)
                    Debug.Log("Player healed by " + healAmount);
                Destroy(gameObject, destroyDelay);
            }
        }
    }
}
