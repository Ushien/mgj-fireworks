using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBurstFirework : MonoBehaviour
{
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    bool isPlayed = false;
    bool hasStarted = false;

    private int limit = 27;
    
    private AudioClip clip;

    [SerializeField] private List<AudioClip> explosionSounds;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        particleSystem = GetComponent<ParticleSystem>();
        int randomIndex = Random.Range(0, explosionSounds.Count);
        clip = explosionSounds[randomIndex];
    }


    void Update()
    {
        int currentCount = particleSystem.particleCount;

        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }
        
        else if (hasStarted && particleSystem.particleCount <= limit && !isPlayed)
        {
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
            isPlayed = true;
        }
    }
}
