using UnityEngine;

public class ShutThemeSound : MonoBehaviour
{
    [SerializeField] private AudioSource themeSound;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private AudioClip themeAtelier;

    public void ShutSound()
    {
        StartCoroutine(FadeOut(themeSound, fadeDuration));
    }

    public void useSound()
    {
        StartCoroutine(FadeIn(themeSound, fadeDuration));
    }

    public void switchSounds()
    {
        StartCoroutine(FadeOutStart(themeSound, fadeDuration));
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

    private System.Collections.IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float targetVolume = 1f; 
        audioSource.volume = 0f;
        audioSource.Play();
        float time = 0f;
        while (time < duration)
        {
            audioSource.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    private System.Collections.IEnumerator FadeOutStart(AudioSource audioSource, float duration)
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
        switchMusic();
        StartCoroutine(FadeIn(themeSound, fadeDuration));
    }

    public void switchMusic()
    {
        themeSound.clip = themeAtelier;
        themeSound.Play();
    }
}
