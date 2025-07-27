using UnityEngine;

public class ShutThemeSound : MonoBehaviour
{
    [SerializeField] private AudioSource themeSound;
    [SerializeField] private float fadeDuration = 1f;

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
}
