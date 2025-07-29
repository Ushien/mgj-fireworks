using System;
using UnityEngine;

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
        audioSource.PlayOneShot(clip);
        character1Animator.Play("CharacterJump");
        character2Animator.Play("CharacterJump");
    }

    public void PlayExplosionSound()
    {
        if (explosionSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, explosionSounds.Length);
            AudioClip randomClip = explosionSounds[randomIndex];
            PlaySound(randomClip);
        }
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
