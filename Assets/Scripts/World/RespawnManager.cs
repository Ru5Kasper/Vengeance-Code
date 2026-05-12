using UnityEngine;
using System.Collections;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    public float respawnDelay = 2f;

    private void Awake()
    {
        instance = this;
    }

    public void RespawnPlayer(PlayerStats player)
    {
        UIManager.instance.ShowRespawnMessage("В этот раз повезёт!");
        player.gameObject.SetActive(false);

        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(PlayerStats player)
    {
        yield return new WaitForSeconds(respawnDelay);

        player.playerHealth.ResetHealth();
        player.transform.position = player.respawnPoint.position;
        player.gameObject.SetActive(true);
        UIManager.instance.HideRespawnMessage();
    }
}
