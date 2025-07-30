using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSubBurst : MonoBehaviour
{

    private ParticleSystem partSys;
    FireworkAudioManager fireworkAudioManager;
    private int previousParticleCount;
    bool isPlayed = false;
    bool hasStarted = false;
    
    private ParticleSystem.Particle[] mParticles;


    private int limit = 27;
    void Start()
    {
        partSys = transform.parent.Find("Burst/Trails").GetComponent<ParticleSystem>();
        previousParticleCount = partSys.particleCount;
        fireworkAudioManager = FireworkAudioManager.Instance;
    }
    void Update()
    {
        int currentCount = partSys.particleCount;
        
        //attend que le trails ait bien démmaré
        if (!hasStarted && currentCount > limit)
        {
            hasStarted = true;
        }
        
        //lance le son lorsque le nombre de particule diminue
        if (partSys.particleCount < previousParticleCount+10 && !isPlayed && hasStarted)
        {

            StartCoroutine(PlayClipRepeatedly());
           
            isPlayed = true;
        }
        previousParticleCount = partSys.particleCount;
    }
    
    // Joue le son plusieurs fois
    private IEnumerator PlayClipRepeatedly()
    {
        int random = Random.Range(8, 10);
        for (int i = 0; i < random; i++)
        {
            fireworkAudioManager.PlaySubExplosionSound();
            yield return new WaitForSeconds(0.2f);
        }
    }
}
