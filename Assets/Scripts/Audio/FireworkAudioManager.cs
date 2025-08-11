using System;
using UnityEngine;
using UnityEngine.Audio;

public class FireworkAudioManager : MonoBehaviour
{
    public static FireworkAudioManager Instance { get; private set; }

    [SerializeField]
    private AudioClip[] explosionSounds;

    [SerializeField]
    private AudioClip[] subExplosionSounds;

    private AudioSource audioSource;

    [SerializeField] private Animator character1Animator;
    [SerializeField] private Animator character2Animator;
    [SerializeField] private AudioMixerGroup fireworksMixerGroup;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void PlaySound(AudioClip clip)
    {
        // On crée un nouveau component (idéalement, il faudrait un pool)
        AudioSource tempSource = gameObject.AddComponent<AudioSource>();

        // Settings du nouveau AudioSource
        tempSource.volume = 0.3f;
        tempSource.spatialBlend = 0f;
        tempSource.playOnAwake = false;
        tempSource.outputAudioMixerGroup = fireworksMixerGroup;

        // On joue le clip, et on détruit après clip.length
        tempSource.PlayOneShot(clip);
        Destroy(tempSource, clip.length);
    }

    public void PlayExplosionSound()
    {
        if (explosionSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, explosionSounds.Length);
            AudioClip randomClip = explosionSounds[randomIndex];
            PlaySound(randomClip);
        }
        character1Animator.Play("CharacterJump");
        character2Animator.Play("CharacterJump");
    }

    public void PlaySubExplosionSound()
    {
        if (subExplosionSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, subExplosionSounds.Length);
            AudioClip randomClip = subExplosionSounds[randomIndex];
            PlaySound(randomClip);
        }
    }

}
