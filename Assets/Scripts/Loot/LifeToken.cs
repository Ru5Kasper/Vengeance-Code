using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LifeToken : MonoBehaviour
{
    public int lifeAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats stats = collision.GetComponent<PlayerStats>();
            if (stats != null)
            {
                bool added = stats.AddLives(lifeAmount);
                if (added)
                    Debug.Log("Extra life gained!");
                Destroy(gameObject);
            }
        }
    }
}
