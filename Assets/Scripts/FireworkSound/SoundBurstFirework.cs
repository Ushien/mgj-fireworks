using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBurstFirework : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] mParticles;
    FireworkAudioManager fireworkAudioManager;
    bool isPlayed = false;
    bool hasStarted = false;

    private int limit = 27;

    private AudioClip clip;

    [SerializeField] private List<AudioClip> explosionSounds;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        fireworkAudioManager = FireworkAudioManager.Instance;
        clip = explosionSounds[Random.Range(0, explosionSounds.Count)];
    }

    /*void Update()
    {
        int currentCount = particleSystem.particleCount;
        Debug.Log(particleSystem.IsAlive());
        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }
        
        if (!particleSystem.IsAlive() && !isPlayed && hasStarted)
        {
            if (clip != null)
            {
                fireworkAudioManager.PlaySound(clip);
            }
            isPlayed = true;
        }
    }*/
    void Update()
    {
        mParticles = new ParticleSystem.Particle[particleSystem.maxParticles];
        
        // Verifie pour chaque particule si sa fin de vie est atteinte
        for(int i = 0; i < particleSystem.GetParticles(mParticles); i++)
        {
            if (mParticles[i].remainingLifetime < 0.01f)
            {
                mParticles[i].remainingLifetime = 0f;
                StartCoroutine(ParticleLifeEnding(mParticles[i].remainingLifetime - 0.01f));
            }
        }
    }
    
    // Coroutine pour jouer le son à la fin de la vie de la particule
    private IEnumerator ParticleLifeEnding(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        if (clip != null)
        {
            fireworkAudioManager.PlaySound(clip);
        }
    }
}
