using System;
using UnityEngine;

public class FireworkAudioManager : MonoBehaviour
{
    public static FireworkAudioManager Instance { get; private set; }
    
    [SerializeField] private AudioClip[] explosionSounds;
    private AudioSource audioSource;

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
    }
}
