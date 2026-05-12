using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip musicClip;

    void Start()
    {
        if (AudioManager.Instance != null && musicClip != null)
        {
            AudioManager.Instance.musicSource.clip = musicClip;
            AudioManager.Instance.musicSource.Play();
        }
    }
}
