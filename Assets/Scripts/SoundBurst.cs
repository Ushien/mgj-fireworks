using System.Collections.Generic;
using UnityEngine;

public class SoundBurst : MonoBehaviour
{
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    private int previousParticleCount;
    bool isPlayed = false;
    bool hasStarted = false;

    private int limit = 27;
    
    private AudioClip clip;

    [SerializeField] private List<AudioClip> explosionSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        int randomIndex = UnityEngine.Random.Range(0, explosionSounds.Count);
        clip = explosionSounds[randomIndex];
    }
    
    void Update()
    {
        int currentCount = particleSystem.particleCount;

        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }

        if (hasStarted && currentCount <= limit && !isPlayed)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
            isPlayed = true;
        }
    }

}
