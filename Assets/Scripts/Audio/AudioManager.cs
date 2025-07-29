using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Liste de tous les sons
    // ======================
    public AudioSource sandSound;
    [SerializeField]
    private AudioSource themeSound;
    [SerializeField]
    private AudioSource takeSound;
    [SerializeField]
    private AudioSource offSound;
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        Instance = this;
    }

    public void ShutSound()
    {
        StartCoroutine(FadeOut(themeSound, fadeDuration));
    }

    private System.Collections.IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        float time = 0f;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 0f;
        audioSource.Stop();
    }

    public void PlayTakeSound()
    {
        takeSound.Play();
    }
    public void PlayOffSound()
    {
        offSound.Play();
    }
}
