using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSubBurst : MonoBehaviour
{
    private AudioSource audioSource;
    private List<AudioSource> audioPool;

    private ParticleSystem particleSystem;
    FireworkAudioManager fireworkAudioManager;
    private int previousParticleCount;
    bool isPlayed = false;
    bool hasStarted = false;
    
    private AudioClip clip;

    [SerializeField] private List<AudioClip> explosionSounds;
    
    private ParticleSystem.Particle[] mParticles;


    private int limit = 25;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        int randomIndex = Random.Range(0, explosionSounds.Count);
        clip = explosionSounds[randomIndex];
        particleSystem = transform.parent.GetComponent<ParticleSystem>();
        previousParticleCount = particleSystem.particleCount;
        fireworkAudioManager = FireworkAudioManager.Instance;
        clip = explosionSounds[randomIndex];
        
    }
    void Update()
    {
        int currentCount = particleSystem.particleCount;
        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }
        
        if (hasStarted &&particleSystem.particleCount < previousParticleCount+10 && !isPlayed)
        {
            if (clip != null)
            {
                StartCoroutine(PlayClipRepeatedly());
            }
            isPlayed = true;
        }
        
        previousParticleCount = particleSystem.particleCount;
    }
    
    private IEnumerator PlayClipRepeatedly()
    {
        int random = Random.Range(8, 10);
        for (int i = 0; i < random; i++)
        {
            audioSource.PlayOneShot(clip);
            yield return new WaitForSeconds(0.2f);
        }
    }
    
/*void Update()
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
}*/

private IEnumerator ParticleLifeEnding(float lifetime)
{
    yield return new WaitForSeconds(lifetime);

    if (clip != null)
    {
        fireworkAudioManager.PlaySound(clip);
    }
}

}
