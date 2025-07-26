using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBurstFirework : MonoBehaviour
{
    private ParticleSystem particleSystem;
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
        int randomIndex = Random.Range(0, explosionSounds.Count);
        clip = explosionSounds[randomIndex];

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
    
    private ParticleSystem.Particle[] mParticles;
 
    void Update()
    {
        if(mParticles == null || mParticles.Length < particleSystem.maxParticles)
        {
            mParticles = new ParticleSystem.Particle[particleSystem.maxParticles];
            Debug.Log(particleSystem.maxParticles);

        }
        int aliveParticles = particleSystem.GetParticles(mParticles);
        particleSystem.GetParticles(mParticles);
        for(int i = 0; i < aliveParticles; i++)
        {
            if (mParticles[i].remainingLifetime < 0.01f)
            {
                mParticles[i].remainingLifetime = 0f;
                StartCoroutine(ParticleLifeEnding(mParticles[i].remainingLifetime - 0.01f));
            }
        }
    }
    private IEnumerator ParticleLifeEnding(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
     
        if (clip != null)
        {
            fireworkAudioManager.PlaySound(clip);
        }
    }
}
