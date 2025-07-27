using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBurstFirework : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] mParticles;
    FireworkAudioManager fireworkAudioManager;
    
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        fireworkAudioManager = FireworkAudioManager.Instance;
    }
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

        fireworkAudioManager.PlayExplosionSound();
    }
}
